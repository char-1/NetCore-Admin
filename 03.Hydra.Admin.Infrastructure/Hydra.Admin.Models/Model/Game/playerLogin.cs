using Hydra.Admin.Utility.Helper;
using System;

namespace Hydra.Admin.Models.Model
{
    public class playerLogin
    {
        public long Id { get; set; }
        public string playerId { get; set; }
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public long loginTime { get; set; }
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public long signTime { get; set; }

        public DateTime lastLoginTime
        {
            get
            {
                return DateTimeHelper.UnixTimesToDateTime(loginTime);
            }
        }
        public DateTime registerTime
        {
            get
            {
                return DateTimeHelper.UnixTimesToDateTime(signTime);
            }
        }
    }
}
