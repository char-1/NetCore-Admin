using Hydra.Admin.DataAccess;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hydra.Admin.Jobs
{
    /// <summary>
    /// 后台 DashBoard 面板数据统计
    /// </summary>
    public class STDashBoardJob : SugarContext<AnalysisDashboard>
    {
        /// <summary>
        /// 月基础数据生成
        /// </summary>
        public static void STBulidMonthData()
        {
            Console.WriteLine("MLGB");
            var dtNow = DateTime.Now;
            int years = dtNow.Year;
            int month = dtNow.Month;
            int days = DateTime.DaysInMonth(years, month);
            var list = new List<AnalysisDashboard>();
            for (var i = 1; i <= days; i++)
            {
                var datetime = string.Format("{0}{1}{2}", years, month.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0'));
                list.Add(new AnalysisDashboard
                {
                    Days = datetime.ToInt(),
                    AddPlayers = 0,
                    BindPlayers = 0,
                    RechargeAmount = 0,
                    RechargePlayers = 0
                });
            }
            List<AnalysisDashboard> notExistslist;
            DbAction((db) =>
            {
                notExistslist = new List<AnalysisDashboard>();
                foreach (var item in list)
                {
                    if (db.Queryable<AnalysisDashboard>().Any(s => s.Days == item.Days)) continue;
                    else notExistslist.Add(item);
                }
                if (notExistslist.Any())
                    db.Insertable(notExistslist).ExecuteCommand();
            });
        }
        //public static void TestBuild()
        //{
        //    DbAction((db) =>
        //    {

        //        var register = db.Queryable<playerBind>().Where(s => true).ToList();
        //        foreach (var item in register)
        //        {
        //            var time = DateTimeHelper.UnixTimesToDateTime(item.bindTime);
        //            db.Updateable<playerRegister>()
        //                                        .UpdateColumns(r => new playerBind
        //                                        {
        //                                            bindTime1 = time
        //                                        }).Where(s => s.playerId == item.playerId).ExecuteCommand();
        //        }

        //    });
        //}
    }
}
