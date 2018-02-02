using System.Threading.Tasks;
using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Utility.iViewControl;
using System.Linq;
using Hydra.Admin.Models.View;
using Hydra.Admin.Utility.eChartControl;
using System.Collections.Generic;

namespace Hydra.Admin.Services
{
    public class playerGoldService : BaseService<playerGold>, IplayerGoldService
    {
        public PlatRechargeView GetPlatRecharge(PlayerGoldQuery query)
        {
            var rechargeView = new PlatRechargeView();
            var grid = new IViewTable<dynamic>();
            var echart = new EChartItem();
            var series = new List<SeriesItem>();
            List<EChartRecharge> list = DbFunction((db) =>
             {
                 switch (query.goleType)
                 {
                     case 0:
                         return db.Ado.SqlQuery<EChartRecharge>(@"SELECT DATE_FORMAT(createTime,'%Y-%m-%d') PrimaryKey,SUM(gold) Number FROM  playergold where createTime BETWEEN @startTime AND @endTime GROUP BY DATE_FORMAT(createTime,'%Y-%m-%d')", new
                         {
                             startTime = query.stime,
                             endTime = query.qetime
                         });
                     case 1:
                         return db.Ado.SqlQuery<EChartRecharge>(@"SELECT DATE_FORMAT(createTime,'%Y-%m-%d') PrimaryKey,COUNT(1) Number FROM  playergold where createTime BETWEEN @startTime AND @endTime GROUP BY DATE_FORMAT(createTime,'%Y-%m-%d')", new
                         {
                             startTime = query.stime,
                             endTime = query.qetime
                         });
                     default:
                         return db.Ado.SqlQuery<EChartRecharge>(@"SELECT DATE_FORMAT(createTime,'%Y-%m-%d') PrimaryKey,COUNT(playerId) Number FROM  playergold where createTime BETWEEN @startTime AND @endTime GROUP BY DATE_FORMAT(createTime,'%Y-%m-%d'),playerId", new
                         {
                             startTime = query.stime,
                             endTime = query.qetime
                         });
                 }
             });
            grid.rows = list;
            grid.total = list.Count;
            //EChart
            echart.xAxis = list.Select(s => s.PrimaryKey);
            series.Add(new SeriesItem
            {
                name = query.tabText,
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
                data = list.Select(s => s.Number)
            });
            echart.series = series;
            rechargeView.Table = grid;
            rechargeView.EChart = echart;
            rechargeView.TabExt = list.Sum(s => s.Number);
            return rechargeView;
        }
        public IViewTable<playerGold> GetPlayerGoldGrid(PlayerGoldQuery query)
        {
            var grid = new IViewTable<playerGold>();
            var where = PredicateBuilderUtility.True<playerGold>();
            if (query.goleType.HasValue && query.goleType.Value != -1)
                where = where.And(s => s.goldType == query.goleType.Value);
            if (query.accountId.HasValue && query.accountId.Value != -1)
                where = where.And(s => s.accountId == query.accountId);
            if (query.stime.HasValue && query.etime.HasValue)
                where = where.And(s => s.createTime >= query.stime.Value && s.createTime < query.qetime.Value);
            grid.rows = DbFunction((db) =>
            {
                return db.Queryable<playerGold>().Where(where).OrderBy(it => it.createTime, SqlSugar.OrderByType.Desc).ToPageList(query.p.Value, query.size, ref total);
            });
            grid.total = total;
            return grid;
        }

        public async Task<IViewTable<playerGold>> GetPlayerGoldGridAsync(PlayerGoldQuery query)
        {
            return await Task.Run(() =>
            {
                var grid = new IViewTable<playerGold>();
                var where = PredicateBuilderUtility.True<playerGold>();
                if (query.goleType.HasValue && query.goleType.Value != -1)
                    where = where.And(s => s.goldType == query.goleType);
                if (query.accountId.HasValue && query.accountId.Value != -1)
                    where = where.And(s => s.accountId == query.accountId);
                if (query.stime.HasValue && query.etime.HasValue)
                    where = where.And(s => s.createTime >= query.stime.Value && s.createTime < query.qetime.Value);
                grid.rows = DbFunction((db) =>
                {
                    return db.Queryable<playerGold>().Where(where).OrderBy(it => it.createTime, SqlSugar.OrderByType.Desc).ToPageList(query.p.Value, query.size, ref total);
                });
                grid.total = total;
                return grid;
            });
        }

        public async Task<IViewTableSlotFooter<playerGold, dynamic>> GetPlayerGoldGridFootAsync(PlayerGoldQuery query)
        {
            return await Task.Run(() =>
            {
                var grid = new IViewTableSlotFooter<playerGold, dynamic>();
                var where = PredicateBuilderUtility.True<playerGold>();
                if (query.goleType.HasValue && query.goleType.Value != -1)
                    where = where.And(s => s.goldType == query.goleType);
                if (query.accountId.HasValue && query.accountId.Value != -1)
                    where = where.And(s => s.accountId == query.accountId);
                if (query.stime.HasValue && query.etime.HasValue)
                    where = where.And(s => s.createTime >= query.stime.Value && s.createTime < query.qetime.Value);
                grid.rows = DbFunction((db) =>
                {
                    return db.Queryable<playerGold>().Where(where).OrderBy(it => it.createTime, SqlSugar.OrderByType.Desc).ToPageList(query.p.Value, query.size, ref total);
                });
                grid.total = total;
                grid.footer = new
                {
                    gold = grid.rows.Sum(s => s.gold)
                };
                return grid;
            });
        }
    }
    public class EChartRecharge
    {
        public string PrimaryKey { get; set; }
        public decimal Number { get; set; }
    }
}
