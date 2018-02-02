using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Query
{
    public class PlayerQuery : BaseQuery
    {
        public string name { get; set; }
        public int? playerType { get; set; }
        public int? frozen { get; set; }
        public string method { get; set; }
        public string account { get; set; }
    }
    /// <summary>
    /// 玩家订单(充值/提现)
    /// </summary>
    public class PlayerOrderQurey : BaseQuery
    {
        public string name { get; set; }
        public string status { get; set; }
        public int? typeId { get; set; }
    }
    /// <summary>
    /// 玩家金币流水
    /// </summary>
    public class PlayerGoldQuery : BaseQuery
    {
        public int? goleType { get; set; }
        public int? accountId { get; set; }
        public string tabText { get; set; }
    }
}
