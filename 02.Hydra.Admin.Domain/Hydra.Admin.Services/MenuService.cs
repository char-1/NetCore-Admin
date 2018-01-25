using Hydra.Admin.IServices;
using Hydra.Admin.Models;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Utility.iViewControl;
using Plugin.Cache;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hydra.Admin.Services
{
    public class MenuService : BaseService<Menus>, IMenuService
    {
        public MenuService()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public List<Menus> GetAdminPermissions(string adminId)
        {
            return DbFunction((db) =>
            {
                var data = db.Queryable<Menus, RoleMenu, Admins>((t1, t2, t3) => new object[] {
                    JoinType.Inner,t1.Id==t2.MenuId,
                    JoinType.Inner,t2.RoleId==t3.RoleId
                })
                .Where((t1, t2, t3) => t3.Id == adminId)
                .ToList();
                return data;
            });
        }

        public List<Menus> GetAllPermissions(Expression<Func<Menus, bool>> where)
        {
            var menus = Cache.Get<List<Menus>>(CacheKeys.Cache_System_Menu);
            if (menus != null && menus.Any())
                return menus;
            else
                return DbFunction((db) =>
                {
                    var data = db.Queryable<Menus>()
                    .Where(where)
                    .ToList();
                    Cache.Add(CacheKeys.Cache_System_Menu, data, TimeSpan.MinValue, TimeSpan.FromHours(2));
                    return data;
                });
        }

        public List<IViewCascader> GetIViewCascaderPermissions(Expression<Func<Menus, bool>> where)
        {
            var data = new List<IViewCascader>();
            var allMenus = this.GetAllPermissions(where);
            foreach (var item in allMenus.Where(s => s.ParentId == Guid.Empty.ToString()).OrderBy(s => s.SortNumber))
            {
                data.Add(new IViewCascader
                {
                    Value = item.Id,
                    Label = item.Name,
                    Children = allMenus.Where(ss => ss.ParentId == item.Id && ss.MenuType == EMenuType.Menu).OrderBy(s => s.SortNumber).Select(sss => new IViewCascader
                    {
                        Value = sss.Id,
                        Label = sss.Name,
                        Children = allMenus.Where(ssss => ssss.ParentId == sss.Id && ssss.MenuType == EMenuType.Button).OrderBy(s => s.SortNumber).Select(ssss => new IViewCascader
                        {
                            Value = ssss.Id,
                            Label = ssss.Name,
                        }).ToList()
                    }).ToList()
                });
            }
            return data;
        }

        public List<IViewTree> GetIViewTreePermissions(Expression<Func<Menus, bool>> where)
        {
            var data = new List<IViewTree>();
            var allMenus = this.GetAllPermissions(where);
            foreach (var item in allMenus.Where(s => s.ParentId == Guid.Empty.ToString()).OrderBy(s => s.SortNumber))
            {
                data.Add(new IViewTree
                {
                    Id = item.Id,
                    Title = item.Name,
                    Children = allMenus.Where(ss => ss.ParentId == item.Id && ss.MenuType == EMenuType.Menu).Select(sss => new IViewTree
                    {
                        Id = sss.Id,
                        Title = sss.Name,
                        Children = allMenus.Where(ssss => ssss.ParentId == sss.Id && ssss.MenuType == EMenuType.Button).Select(ssss => new IViewTree
                        {
                            Id = ssss.Id,
                            Title = ssss.Name,
                        }).ToList()
                    }).ToList()
                });
            }
            return data;
        }

        public List<Menus> GetRolePermissions(string roleId)
        {
            return DbFunction((db) =>
            {
                var data = db.Queryable<Menus, RoleMenu, Roles>((t1, t2, t3) => new object[] {
                    JoinType.Inner,t1.Id==t2.MenuId,
                    JoinType.Inner,t2.RoleId==t3.Id
                })
                .Where((t1, t2, t3) => t3.Id == roleId)
                .ToList();
                return data;
            });
        }
    }
}