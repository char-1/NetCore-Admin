using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hydra.Admin.Models.Model
{
    public partial class Config
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Version { get; set; }
        /// <summary>
        /// 游戏代码zip包路径
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Code_url { get; set; }
        /// <summary>
        /// 游戏资源存放路径
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Update_url { get; set; }
    }
}
