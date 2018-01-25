using Hydra.Admin.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Hydra.Admin.Utility.iViewControl;

namespace Hydra.Admin.IServices
{
    public interface IMenuService : IBaseService<Menus>
    {
        /// <summary>
        /// 获取管理员权限
        /// </summary>
        /// <param name="adminId">管理员编号</param>
        /// <returns></returns>
        List<Menus> GetAdminPermissions(string adminId);
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        List<Menus> GetRolePermissions(string roleId);
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        List<Menus> GetAllPermissions(Expression<Func<Menus, bool>> where);
        /// <summary>
        /// 获取 IViewTree格式权限数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        List<IViewTree> GetIViewTreePermissions(Expression<Func<Menus, bool>> where);
        /// <summary>
        /// 获取级联菜单
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        List<IViewCascader> GetIViewCascaderPermissions(Expression<Func<Menus, bool>> where);
    }
}
