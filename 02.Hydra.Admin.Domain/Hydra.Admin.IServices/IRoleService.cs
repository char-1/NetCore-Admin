using Hydra.Admin.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.iViewControl;
using Hydra.Admin.Models.View;
using System.Linq.Expressions;
namespace Hydra.Admin.IServices
{
    public interface IRoleService : IBaseService<Roles>
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IViewTable<Roles> GetRoleGrid(BaseQuery query);
        /// <summary>
        /// 角色权限分配
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="menuIds">权限集合</param>
        /// <returns></returns>
        bool SetRolePermission(string roleId, IEnumerable<string> menuIds);
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="isMenu">是否为Menu菜单</param>
        /// <returns></returns>
        List<IViewTree> GetRolePermission(string roleId, bool isMenu = false);
        /// <summary>
        /// 获取IView Select角色
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        List<IViewSelect> GetSelectRole(Expression<Func<Roles, bool>> where);
    }
}
