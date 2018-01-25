using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.View
{
    /// <summary>
    /// 管理员登录
    /// </summary>
    public class AdminLoginView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 登录帐号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录 Token值
        /// </summary>
        public string LastLoginToken { get; set; }
    }
    /// <summary>
    /// 菜单 或 按钮
    /// </summary>
    public class MenuOrButtonView
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Router { get; set; }
        public string Icon { get; set; }
    }
    /// <summary>
    /// 管理员权限
    /// </summary>
    public class PermissionView
    {
        public MenuOrButtonView parent { get; set; }
        public List<MenuOrButtonView> children { get; set; }
    }
}
