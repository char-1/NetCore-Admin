using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.iViewControl;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Models.View;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Admin.Services
{
    public class AnalysisDashBoardService : BaseService<AnalysisDashboard>, IAnalysisDashBoardService
    {
        public DashBoardView GetDashBoard(DashBoardQuery query)
        {
            var dashBoard = new DashBoardView();
            var grid = new IViewTable<AnalysisDashboard>();
            var where = PredicateBuilderUtility.True<AnalysisDashboard>();
            if (query.stime.HasValue && query.etime.HasValue)
            {
                var stime = query.stime.Value.ToString("yyyyMMdd").ToInt();
                var etime = query.etime.Value.ToString("yyyyMMdd").ToInt();
                where = where.And(s => s.Days >= stime && s.Days <= etime);
            }
            var list = DbFunction((db) =>
            {
                return db.Queryable<AnalysisDashboard>().Where(where).OrderBy(it => it.Days, SqlSugar.OrderByType.Desc).ToList();
            });
            if (list.Any())
            {
                //BoardItem
                dashBoard.DashBoardItem = new List<decimal> {
                    list.Sum(s=>s.AddPlayers).ToDecimal(),
                    list.Sum(s=>s.RechargePlayers).ToDecimal(),
                    list.Sum(s=>s.RechargeAmount).ToDecimal(),
                    list.Sum(s=>s.BindPlayers).ToDecimal()
                };
                //Chart
                dashBoard.xAxisData = list.Select(s => s.Days.ToString());
                var dic = new Dictionary<string, IEnumerable<decimal>>
                {
                    { "addPlayers", list.Select(s => s.AddPlayers.ToDecimal()) },
                    { "rechargePlayers", list.Select(s => s.RechargePlayers.ToDecimal()) },
                    { "rechargeAmount", list.Select(s => s.RechargeAmount.ToDecimal()) },
                    { "bindPlayers", list.Select(s => s.BindPlayers.ToDecimal()) }
                };
                dashBoard.yAxisData = dic;
                //Table
                grid.rows = list;
                grid.total = list.Count;
            }
            dashBoard.Table = grid;
            return dashBoard;
        }

        public async Task<DashBoardView> GetDashBoardAsync(DashBoardQuery query)
        {
            return await Task.Run(() =>
            {
                var dashBoard = new DashBoardView();
                var grid = new IViewTable<AnalysisDashboard>();
                var where = PredicateBuilderUtility.True<AnalysisDashboard>();
                if (query.stime.HasValue && query.etime.HasValue)
                {
                    var stime = query.stime.Value.ToString("yyyyMMdd").ToInt();
                    var etime = query.etime.Value.ToString("yyyyMMdd").ToInt();
                    where = where.And(s => s.Days >= stime && s.Days <= etime);
                }
                var list = DbFunction((db) =>
                {
                    return db.Queryable<AnalysisDashboard>().Where(where).OrderBy(it => it.Days, SqlSugar.OrderByType.Desc).ToList();
                });
                if (list.Any())
                {
                    //BoardItem
                    dashBoard.DashBoardItem = new List<decimal> {
                    list.Sum(s=>s.AddPlayers).ToDecimal(),
                    list.Sum(s=>s.RechargePlayers).ToDecimal(),
                    list.Sum(s=>s.RechargeAmount).ToDecimal(),
                    list.Sum(s=>s.BindPlayers).ToDecimal()
                };
                    //Chart
                    dashBoard.xAxisData = list.Select(s => s.Days.ToString());
                    var dic = new Dictionary<string, IEnumerable<decimal>>
                {
                    { "addPlayers", list.Select(s => s.AddPlayers.ToDecimal()) },
                    { "rechargePlayers", list.Select(s => s.RechargePlayers.ToDecimal()) },
                    { "rechargeAmount", list.Select(s => s.RechargeAmount.ToDecimal()) },
                    { "bindPlayers", list.Select(s => s.BindPlayers.ToDecimal()) }
                };
                    dashBoard.yAxisData = dic;
                    //Table
                    grid.rows = list;
                    grid.total = list.Count;
                }
                dashBoard.Table = grid;
                return dashBoard;
            });
        }


        public IViewTable<AnalysisDashboard> GetDashBoardGrid(BaseQuery query)
        {
            var grid = new IViewTable<AnalysisDashboard>();
            var where = PredicateBuilderUtility.True<AnalysisDashboard>();
            if (query.stime.HasValue && query.etime.HasValue)
            {
                var stime = query.stime.Value.ToString("yyyyMMdd").ToInt();
                var etime = query.etime.Value.ToString("yyyyMMdd").ToInt();
                where = where.And(s => s.Days >= stime && s.Days <= etime);
            }
            grid.rows = DbFunction((db) =>
            {
                return db.Queryable<AnalysisDashboard>().Where(where).OrderBy(it => it.Days, SqlSugar.OrderByType.Desc).ToPageList(query.p.Value, query.size, ref total);
            });
            grid.total = total;
            return grid;
        }
    }
}
