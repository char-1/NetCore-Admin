using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models
{
    public class CacheKeys
    {
        /// <summary>
        /// 系统菜单缓存键值
        /// </summary>
        public const string Cache_System_Menu = "Cache_System_Menu";
        /// <summary>
        /// 角色权限缓存
        /// </summary>
        public const string Cache_Role_Permission_HasBtn = "Cache_Role_Permission_HasBtn_{0}_{1}";
        /// <summary>
        /// 系统配置
        /// </summary>
        public const string Cache_System_Config = "Cache_System_Config";
        /// <summary>
        /// 管理员Token
        /// </summary>
        public const string Cache_Admin_Token = "Cache_Admin_Token_{0}";
    }
}
