using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Utility
{
    public class AppSettings
    {
        /// <summary>
        /// 数据库链接
        /// </summary>
        public string MasterDB { get; set; }
    }
    /// <summary>
    /// Redis 配置
    /// </summary>
    public class RedisHost
    {
        public RedisHost()
        {
            this.Prot = 6379;
        }
        public string Host { get; set; }
        public int Prot { get; set; }
        public string PassWord { get; set; }
    }
}
