using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Models.View;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Utility.iViewControl;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hydra.Admin.Services
{
    public class playerOnlineService : BaseService<playerOnline>, IplayerOnlineService
    {
        public async Task<PlayerOnlineView> GetPlayerOnlineChart(DashBoardQuery query)
        {
            return await Task.Run(() =>
            {
                var playerOnline = new PlayerOnlineView();
                var grid = new IViewTable<dynamic>();
                var where = PredicateBuilderUtility.True<playerOnline>();
                if (query.stime.HasValue && query.etime.HasValue)
                    where = where.And(s => s.CreateTime >= query.stime.Value && s.CreateTime <= query.qetime);
                var list = DbFunction((db) =>
                {
                    return db.Queryable<playerOnline>().Where(where).OrderBy(it => it.CreateTime, SqlSugar.OrderByType.Desc).ToList();
                });
                if (list.Any())
                {
                    if (query.tabs == 0)
                    {
                        //Chart
                        playerOnline.xAxisData = list.Select(s => s.HM.ToString().PadLeft(4,'0').Substring(0, 2) + ":" + s.HM.ToString().PadLeft(4, '0').Substring(2, 2));
                        playerOnline.yAxisData = list.Select(s => s.OLPlayer);
                        //Table
                        grid.rows = list.Select(s => new
                        {
                            RowKey = s.HM.ToString().PadLeft(4, '0').Substring(0, 2) + ":" + s.HM.ToString().PadLeft(4, '0').Substring(2, 2),
                            RowValue = s.OLPlayer
                        });
                        grid.total = list.Count;
                    }
                    else
                    {
                        //Table
                        var group = list.GroupBy(s => s.YMD, (key, value) => new
                        {
                            RowKey = key.ToString().Substring(0, 4) + "-" + key.ToString().Substring(4, 2) + "-" + key.ToString().Substring(6, 2),
                            RowValue = value.Sum(s => s.OLPlayer)
                        }).ToList();
                        grid.rows = group;
                        grid.total = group.Count;
                        //Chart
                        playerOnline.xAxisData = group.Select(s => s.RowKey);
                        playerOnline.yAxisData = group.Select(s => s.RowValue);
                    }
                }
                playerOnline.Table = grid;
                return playerOnline;
            });
        }
    }
}
