using Hydra.Admin.DataAccess;
using Hydra.Admin.Models.Model;
using Flurl;
using Flurl.Http;
using Hydra.Admin.Utility.Helper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hydra.Admin.Jobs
{
    /// <summary>
    /// 统计在线玩家
    /// </summary>
    public class AnalyOnlinePlayerJob : SugarContext<playerOnline>
    {
        public async static Task Execued()
        {
            var api = DbConfig.GameAPI + "analyOnlinePlayer.nd";
            long ticks = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now);
            var data = await api.SetQueryParam("ticks", ticks)
                     .GetAsync()
                     .ReceiveJson<ResponseOnlinePlayer>();
            if (data != null)
            {
                DbAction((db) =>
                {
                    db.Insertable(data.Response).ExecuteCommandAsync();
                });
            }
        }
    }
}
