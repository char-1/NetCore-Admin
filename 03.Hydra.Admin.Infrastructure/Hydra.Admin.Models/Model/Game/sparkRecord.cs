using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Model
{
    public class sparkRecord
    {
        public string Id { get; set; }
        public int GameId { get; set; }
        public long StartTime { get; set; }
        public long ResultTime { get; set; }
        public decimal GoldPool { get; set; }
        public string Result { get; set; }
        public string Bets { get; set; }
        public string RealBets { get; set; }
        public int IsSpark { get; set; }
        public decimal SparkRate { get; set; }
    }
}
