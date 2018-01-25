using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.Exceptions;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Utility.iViewControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hydra.Admin.Models.View;
using Hydra.Admin.Models;
using System.Linq.Expressions;
using Plugin.Cache;

namespace Hydra.Admin.Services
{
    public class RoleService : BaseService<Roles>, IRoleService
    {
        private readonly IMenuService IMenuService;
        public RoleService(IMenuService IMenuService)
        {
            this.IMenuService = IMenuService;
        }
        public List<IViewTree> GetRolePermission(string roleId, bool isMenu = false)
        {
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentEmptyException("角色编号不能为空");
            var permission = Cache.Get<List<IViewTree>>(string.Format(CacheKeys.Cache_Role_Permission_HasBtn, roleId, isMenu));
            if (permission != null && permission.Any())
                return permission;
            var data = new List<IViewTree>();
            if (!isMenu)
            {
                var userMenu = DbFunction((db) =>
                {
                    return db.Queryable<RoleMenu>().Where(s => s.RoleId == roleId).ToList();
                }) as List<RoleMenu>;
                var allMenus = IMenuService.GetAllPermissions(s => s.IsEnable);
                foreach (var item in allMenus.Where(s => s.ParentId == Guid.Empty.ToString()).OrderBy(it => it.SortNumber))
                {
                    data.Add(new IViewTree
                    {
                        Id = item.Id,
                        Title = item.Name,
                        Levele = 1,
                        //Checked = userMenu.Any(ss => ss.MenuId.Equals(item.Id)),
                        Icon = item.Icon,
                        ParentId = item.ParentId,
                        Children = allMenus.Where(ss => ss.ParentId == item.Id && ss.MenuType == EMenuType.Menu).OrderBy(it => it.SortNumber).Select(sss => new IViewTree
                        {
                            Id = sss.Id,
                            Title = sss.Name,
                            Checked = userMenu.Any(ss => ss.MenuId.Equals(sss.Id)),
                            Levele = 2,
                            ParentId = sss.ParentId,
                            Router = sss.Router,
                            RouterName = sss.RouterName,
                            Children = allMenus.Where(ssss => ssss.ParentId == sss.Id && ssss.MenuType == EMenuType.Button).OrderBy(it => it.SortNumber).Select(ssss => new IViewTree
                            {
                                Id = ssss.Id,
                                Title = ssss.Name,
                                Checked = userMenu.Any(ss => ss.MenuId.Equals(ssss.Id)),
                                Levele = 3,
                                ParentId = ssss.ParentId,
                            }).ToList()
                        }).ToList()
                    });
                }
            }
            else
            {
                var allMenus = IMenuService.GetRolePermissions(roleId);
                foreach (var item in allMenus.Where(s => s.ParentId == Guid.Empty.ToString()).OrderBy(it => it.SortNumber))
                {
                    data.Add(new IViewTree
                    {
                        Id = item.Id,
                        Title = item.Name,
                        Levele = 1,
                        Icon = item.Icon,
                        ParentId = item.ParentId,
                        Children = allMenus.Where(ss => ss.ParentId == item.Id && ss.MenuType == EMenuType.Menu).OrderBy(it => it.SortNumber).Select(sss => new IViewTree
                        {
                            Id = sss.Id,
                            Title = sss.Name,
                            Levele = 2,
                            ParentId = sss.ParentId,
                            Router = sss.Router,
                            RouterName = sss.RouterName

                        }).ToList()
                    });
                }
            }
            Cache.Add(string.Format(CacheKeys.Cache_Role_Permission_HasBtn, roleId, isMenu), data, TimeSpan.MinValue, TimeSpan.FromHours(2));
            return data;
        }

        public IViewTable<Roles> GetRoleGrid(BaseQuery query)
        {
            var grid = new IViewTable<Roles>();
            var where = PredicateBuilderUtility.True<Roles>();
            if (!string.IsNullOrEmpty(query.keyword))
                where = where.And(s => s.Name.Contains(query.keyword));
            grid.rows = DbFunction((db) =>
            {
                return db.Queryable<Roles>().OrderBy(it => it.Id).ToPageList(query.p.Value, query.size, ref total);
            });
            grid.total = total;
            return grid;
        }

        public List<IViewSelect> GetSelectRole(Expression<Func<Roles, bool>> where)
        {
            return DbFunction((db) =>
             {
                 return db.Queryable<Roles>().Where(where).OrderBy(it => it.SortNumber).Select(s => new IViewSelect
                 {
                     Key = s.Name,
                     Value = s.Id
                 }).ToList();
             });
        }

        public bool SetRolePermission(string roleId, IEnumerable<string> menuIds)
        {
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentEmptyException("角色编号不能为空");
            if (!menuIds.Any()) throw new ArgumentEmptyException("待分配权限不能为空");
            return DbFunction((db) =>
            {
                db.Ado.BeginTran();
                try
                {
                    db.Deleteable<RoleMenu>(s => s.RoleId == roleId).ExecuteCommand();
                    var roleMenus = menuIds.Select(s => new RoleMenu
                    {
                        RoleId = roleId,
                        MenuId = s
                    }).ToList();
                    var flag = db.Insertable(roleMenus).ExecuteCommand() > 0;
                    db.Ado.CommitTran();
                    return flag;
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    Log.Error(ex);
                    return false;
                }
            });
        }
    }
}
