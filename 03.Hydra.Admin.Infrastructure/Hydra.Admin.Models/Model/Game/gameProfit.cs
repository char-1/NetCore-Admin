using Hydra.Admin.Utility.Helper;
using System;

namespace Hydra.Admin.Models.Model
{
    public class gameProfit
    {
        public gameProfit()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public decimal Gold { get; set; }
        public int PaymentType { get; set; }
        public int GameId { get; set; }
        public long IgnoreTime { get; set; }
        public DateTime CreateTime
        {
            get
            {
                return DateTimeHelper.UnixTimesToDateTime(IgnoreTime);
            }
        }
        public int Today
        {
            get
            {
                return DateTimeHelper.UnixTimesToDateTime(IgnoreTime).ToString("yyyyMMdd").ToInt();
            }
        }
    }
}
