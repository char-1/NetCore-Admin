using System.Threading.Tasks;
using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Utility.iViewControl;
using System.Linq;

namespace Hydra.Admin.Services
{
    public class playerGoldService : BaseService<playerGold>, IplayerGoldService
    {
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
}
