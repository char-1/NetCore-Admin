using AutoMapper;
using Hydra.Admin.IServices;
using Hydra.Admin.Models;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Models.View;
using Hydra.Admin.Utility.Exceptions;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Utility.iViewControl;
using Plugin.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Admin.Services
{
    public class AdminService : BaseService<Admins>, IAdminService
    {
        private readonly IMenuService IMenuService;
        private readonly ILogsService ILogsService;
        public AdminService(IMenuService IMenuService, ILogsService ILogsService)
        {
            this.IMenuService = IMenuService;
            this.ILogsService = ILogsService;
        }
        public AdminLoginView Login(AdminLoginQuery query)
        {
            if (query != null && string.IsNullOrEmpty(query.LoginName)) throw new ArgumentEmptyException("帐号不能为空");
            if (query != null && string.IsNullOrEmpty(query.PassWord)) throw new ArgumentEmptyException("密码不能为空");
            var admin = DbFunction((db) =>
            {
                return db.Queryable<Admins>().Single(x => x.LoginName.Equals(query.LoginName));
            });
            if (admin == null) throw new NotFoundException("账号不存在");
            if (!admin.IsEnable) throw new NotFoundException("帐号不允许登录");
            if (!EncryptPassword(query.PassWord, admin.PassSalt, false).EqualsIgnoreCase(admin.PassWord)) throw new NotEqualException("登录密码错误");
            if (query.UpdateLoginToken)
            {
                var newToken = Guid.NewGuid().ToString();
                Cache.Remove(string.Format(CacheKeys.Cache_Admin_Token, admin.LastLoginToken));
                UpdateLoginToken(admin.Id, newToken, DateTime.Now, admin);
                admin.LastLoginToken = newToken;
            }
            if (admin.LastLoginToken.IsNullOrBlank())
                throw new ArgumentEmptyException("token失效");
            var data = Mapper.Map<Admins, AdminLoginView>(admin);
            return data;
        }

        public List<PermissionView> GetAdminPermission(string adminId)
        {
            var data = new List<PermissionView>();
            var allMenus = IMenuService.GetAdminPermissions(adminId);
            foreach (var item in allMenus.Where(s => s.ParentId == Guid.Empty.ToString()))
            {
                data.Add(new PermissionView
                {
                    parent = new MenuOrButtonView
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        Router = item.Router,
                        Icon = item.Icon
                    },
                    children = allMenus.Where(ss => ss.ParentId == item.Id && ss.MenuType == 0).Select(sss => new MenuOrButtonView
                    {
                        Id = sss.Id,
                        ParentId = sss.ParentId,
                        Name = sss.Name,
                        Router = sss.Router,
                        Icon = sss.Icon
                    }).ToList()
                });
            }
            return data;
        }

        public IViewTable<Admins> GetAdminGrid(BaseQuery query)
        {
            var grid = new IViewTable<Admins>();
            var where = PredicateBuilderUtility.True<Admins>();
            if (!string.IsNullOrEmpty(query.keyword))
                where = where.And(s => s.LoginName.Contains(query.keyword));
            grid.rows = DbFunction((db) =>
            {
                return db.Queryable<Admins>().Where(where).OrderBy(it => it.Id).ToPageList(query.p.Value, query.size, ref total);
            });
            grid.total = total;
            return grid;
        }

        public bool ModifyPass(AdminChangePassQuery query)
        {
            if (query != null && string.IsNullOrEmpty(query.AdminId)) throw new ArgumentEmptyException("管理员编号不能为空");
            if (query != null && string.IsNullOrEmpty(query.OldPass)) throw new ArgumentEmptyException("旧密码不能为空");
            if (query != null && string.IsNullOrEmpty(query.NewPass)) throw new ArgumentEmptyException("新密码不能为空");
            var admin = DbFunction((db) =>
            {
                return db.Queryable<Admins>().Single(x => x.Id.Equals(query.AdminId));
            });
            if (admin == null) throw new NotFoundException("账号不存在");
            if (!admin.IsEnable) throw new NotFoundException("帐号已被禁用");
            if (!EncryptPassword(query.OldPass, admin.PassSalt, false).EqualsIgnoreCase(admin.PassWord)) throw new NotEqualException("旧密码错误");
            var password = SecurityHelper.Md5(admin.PassSalt + query.NewPass);
            return DbFunction((db) =>
            {
                return db.Updateable<Admins>()
                .UpdateColumns(s => new Admins
                {
                    PassWord = password
                })
                .Where(w => w.Id == query.AdminId).ExecuteCommand() > 0;
            });
        }

        #region private methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passsalt"></param>
        /// <param name="isMD5"></param>
        /// <returns></returns>
        private static string EncryptPassword(string password, string passsalt, bool hasMd5 = false)
        {
            if (hasMd5)
                return SecurityHelper.Md5(passsalt + SecurityHelper.Md5(password));
            else
                return SecurityHelper.Md5(passsalt + password);
        }
        /// <summary>
        /// 更新Token
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="loginToken"></param>
        /// <param name="lastLoginTime"></param>
        private void UpdateLoginToken(string adminId, string loginToken, DateTime lastLoginTime, object cacheAdmin)
        {
            var hostIP = NetHelper.GetIPAddress();
            DbAction((db) =>
            {
                db.Updateable<Admins>()
                .UpdateColumns(u => new Admins
                {
                    LastLoginToken = loginToken,
                    LastLoginTime = lastLoginTime,
                    LastLoginIp = hostIP
                })
                .Where(s => s.Id.Equals(adminId)).ExecuteCommandAsync();
                ILogsService.Insert(new Logs
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    User = ((Admins)cacheAdmin).LoginName,
                    IP = hostIP,
                    Types = "管理员登录",
                    Controller = "Admin",
                    Action = "Login",
                    Remark = "管理员登录 登录日志记录"
                });
                Cache.Add(string.Format(CacheKeys.Cache_Admin_Token, loginToken), cacheAdmin, TimeSpan.MinValue, TimeSpan.FromDays(7));
            });
        }
        #endregion
    }
}
