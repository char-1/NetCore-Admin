using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Models.View;
using Hydra.Admin.Utility.eChartControl;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Utility.iViewControl;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Admin.Services
{
    public class AnalysisGameProfitService : BaseService<gameProfit>, IAnalysisGameProfitService
    {
        public GameProfitView GetGameProfitView(BaseQuery query)
        {
            var gameProfit = new GameProfitView();
            var grid = new IViewTable<dynamic>();
            var echart = new EChartItem();
            var where = PredicateBuilderUtility.True<gameProfit>();
            if (query.stime.HasValue && query.etime.HasValue)
                where = where.And(s => s.CreateTime >= query.stime.Value && s.CreateTime <= query.qetime);
            var list = DbFunction((db) =>
            {
                return db.Queryable<gameProfit>().Where(where).OrderBy(it => it.CreateTime, SqlSugar.OrderByType.Desc).ToList();
            });
            if (list.Any())
            {
                //Table
                var group = list.GroupBy(s => s.Today, (key, value) => new
                {
                    PrimaryKey = key.ToString().Substring(0, 4) + "-" + key.ToString().Substring(4, 2) + "-" + key.ToString().Substring(6, 2),
                    Positive = value.Where(s => s.PaymentType == 1).Sum(s => s.Gold),
                    Negative = value.Where(s => s.PaymentType == -1).Sum(s => s.Gold)
                }).ToList();
                grid.rows = group;
                grid.total = group.Count;
                //EChart
                echart.xAxis = group.Select(s => s.PrimaryKey);
                echart.legend = new List<string>
                {
                    "收入","支出"
                };
                var series = new List<SeriesItem>();
                series.Add(new SeriesItem
                {
                    name = "收入",
                    itemStyle = new ItemStyle
                    {
                        normal = new Normal
                        {
                            color = "#2b83f9",
                            areaStyle = new AreaStyle
                            {
                                color = "#E8F5FD"
                            }
                        }
                    },
                    data = group.Select(s => s.Positive)
                });
                series.Add(new SeriesItem
                {
                    name = "支出",
                    itemStyle = new ItemStyle
                    {
                        normal = new Normal
                        {
                            color = "#00e09e",
                            areaStyle = new AreaStyle
                            {
                                color = "#3eede7"
                            }
                        }
                    },
                    data = group.Select(s => s.Negative*-1)
                });
                echart.series = series;
            }
            gameProfit.EChart = echart;
            gameProfit.Table = grid;
            return gameProfit;
        }
        public async Task<GameProfitView> GetGameProfitViewAsync(BaseQuery query)
        {
            return await Task.Run(() =>
            {
                var gameProfit = new GameProfitView();
                var grid = new IViewTable<dynamic>();
                var echart = new EChartItem();
                var where = PredicateBuilderUtility.True<gameProfit>();
                if (query.stime.HasValue && query.etime.HasValue)
                    where = where.And(s => s.CreateTime >= query.stime.Value && s.CreateTime <= query.qetime);
                var list = DbFunction((db) =>
                {
                    return db.Queryable<gameProfit>().Where(where).OrderBy(it => it.CreateTime, SqlSugar.OrderByType.Desc).ToList();
                });
                if (list.Any())
                {
                    //EChart
                    grid.total = list.Count;
                    //Table
                }
                gameProfit.Table = grid;
                return gameProfit;
            });
        }
    }
}
