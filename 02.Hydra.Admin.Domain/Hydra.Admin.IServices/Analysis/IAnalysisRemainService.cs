using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.iViewControl;

namespace Hydra.Admin.IServices
{
    public interface IAnalysisRemainService:IBaseService<AnalysisRemain>
    {
        IViewTable<AnalysisRemain> GetRemainGrid(BaseQuery query);
    }
}
