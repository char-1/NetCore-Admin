using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Model
{
    /// <summary>
    /// 后台首页面板展示数据集合
    /// </summary>
    public class AnalysisDashboard
    {
        public int Days { get; set; }
        public int AddPlayers { get; set; }
        public int RechargePlayers { get; set; }
        public int RechargeAmount { get; set; }
        public int BindPlayers { get; set; }
    }
}
