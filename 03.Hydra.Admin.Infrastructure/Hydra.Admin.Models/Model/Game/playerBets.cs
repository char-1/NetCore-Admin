using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Admin.Models.Model
{
    public class playerBets
    {
        public playerBets()
        {
            this.isBanker = 0;
            this.isRobot = 0;
            this.win = 0;
            this.issettle = 0;
            this.waterate = 0.05M;
            //this.betItem = new HashSet<betItems>();
        }
        public string Id { get; set; }
        public string playerId { get; set; }
        public int gameId { get; set; }
        public long startTime { get; set; }
        public int isBanker { get; set; }
        //[SqlSugar.SugarColumn(IsIgnore = true)]
        //public ICollection<betItems> betItem { get; set; }
        public string bets
        {
            get; set;
        }
        public decimal allBets { get; set; }
        public int isRobot { get; set; }
        public decimal win { get; set; }
        public string result { get; set; }
        public int issettle { get; set; }
        public decimal waterate { get; set; }

    }

    public class betItems
    {
        public int _id { get; set; }
        public decimal score { get; set; }
    }
}
