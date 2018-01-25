using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Query
{
    /// <summary>
    /// 管理员登录
    /// </summary>
    public class AdminLoginQuery
    {
        public AdminLoginQuery()
        {
            this.UpdateLoginToken = true;
        }
        public string LoginName { get; set; }
        public string PassWord { get; set; }
        public bool UpdateLoginToken { get; set; }
    }
    /// <summary>
    /// 修改密码 
    /// </summary>
    public class AdminChangePassQuery
    {
        public string AdminId { get; set; }
        public string OldPass { get; set; }
        public string NewPass { get; set; }
    }
}
