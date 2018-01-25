using System;

namespace Hydra.Admin.Models.Model
{
    public class AnalysisRemain
    {
        public long Id { get; set; }
        public int players { get; set; }
        public decimal secondDay { get; set; }
        public decimal thirdDay { get; set; }
        public decimal seventhDay { get; set; }
        public decimal fifteenDay { get; set; }
        public decimal thirtiethDay { get; set; }
        public DateTime statTime { get; set; }
        public DateTime addTime { get; set; }
    }
}
