using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.Exceptions;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Utility.iViewControl;
using System.Linq;

namespace Hydra.Admin.Services
{
    public class LogsService : BaseService<Logs>, ILogsService
    {
        public IViewTable<Logs> GetLogsGrid(BaseQuery query)
        {
            var grid = new IViewTable<Logs>();
            var where = PredicateBuilderUtility.True<Logs>();
            if (!string.IsNullOrEmpty(query.keyword))
                where = where.And(s => s.User.Contains(query.keyword));
            if (query.stime.HasValue && query.etime.HasValue)
                where = where.And(s => s.CreateTime >= query.stime.Value && s.CreateTime < query.qetime.Value);
            grid.rows = DbFunction((db) =>
            {
                return db.Queryable<Logs>().Where(where).OrderBy(it => it.CreateTime,SqlSugar.OrderByType.Desc).ToPageList(query.p.Value, query.size, ref total);
            });
            grid.total = total;
            return grid;
        }

        public bool RemoveLogs(string logIds)
        {
            if (string.IsNullOrEmpty(logIds)) throw new ArgumentEmptyException("参数值不能为空");
            var ids = logIds.Split(',').Select(s => s).ToArray();
            return DbFunction((db) =>
            {
                return db.Deleteable<Logs>(ids).ExecuteCommand()>0;
            }, false);
        }
    }
}
