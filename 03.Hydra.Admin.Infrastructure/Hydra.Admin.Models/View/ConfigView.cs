using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.View
{
    public class ConfigView
    {
        /// <summary>
        /// 游戏代码zip包路径
        /// </summary>
        public string Code_url { get; set; }
        /// <summary>
        /// 游戏资源存放路径
        /// </summary>
        public string Update_url { get; set; }
    }
}
