using Hydra.Admin.IServices;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.Helper;
using Hydra.Admin.Utility.iViewControl;

namespace Hydra.Admin.Services
{
    public class AnalysisRemainService : BaseService<AnalysisRemain>, IAnalysisRemainService
    {
        public IViewTable<AnalysisRemain> GetRemainGrid(BaseQuery query)
        {
            var grid = new IViewTable<AnalysisRemain>();
            var where = PredicateBuilderUtility.True<AnalysisRemain>();
            if (query.stime.HasValue && query.etime.HasValue)
            {
                where = where.And(s => s.statTime > query.stime && s.statTime < query.etime);
            }
            grid.rows = DbFunction((db) =>
            {
                return db.Queryable<AnalysisRemain>().Where(where).OrderBy(it => it.statTime, SqlSugar.OrderByType.Asc).ToPageList(query.p.Value, query.size, ref total);
            });
            grid.total = total;
            return grid;
        }
    }
}
