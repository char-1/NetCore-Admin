using Hydra.Admin.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Model
{
    /// <summary>
    /// 玩家金币
    /// </summary>
    public class playerGold
    {
        public string Id { get; set; }
        public string playerId { get; set; }
        public int playerType { get; set; }
        public int accountId { get; set; }
        public decimal gold { get; set; }
        public string remark { get; set; }
        /// <summary>
        /// 金币来源
        /// </summary>
        public int? goldType { get; set; }
        //[SqlSugar.SugarColumn(IsIgnore = true)]
        public long ignoreTime { get; set; }
        public DateTime createTime
        {
            get
            {
                return DateTimeHelper.UnixTimesToDateTime(ignoreTime);
            }
        }

    }
}
