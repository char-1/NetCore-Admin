using Hydra.Admin.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Model
{
    /// <summary>
    /// 玩家注册
    /// </summary>
    public class playerRegister
    {
        public string playerId { get; set; }
        public string nickName { get; set; }
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public long registerTickes { get; set; }
        public int playerType { get; set; }
        public int accountId { get; set; }
        public DateTime registerTime
        {
            get
            {
                return DateTimeHelper.UnixTimesToDateTime(registerTickes);
            }
        }
    }
}
