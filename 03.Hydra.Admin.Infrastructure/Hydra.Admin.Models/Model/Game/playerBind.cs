using Hydra.Admin.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Model
{
    /// <summary>
    /// 玩家绑定
    /// </summary>
    public class playerBind
    {
        public string playerId { get; set; }
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public long bindTicks { get; set; }
        public DateTime bindTime
        {
            get
            {
                return DateTimeHelper.UnixTimesToDateTime(bindTicks);
            }
        }
        public string nickName { get; set; }
        public string bindPhone { get; set; }
    }
}
