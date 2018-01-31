using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Models.View;
using System.Threading.Tasks;

namespace Hydra.Admin.IServices
{
    public interface IAnalysisGameProfitService : IBaseService<gameProfit>
    {
        Task<GameProfitView> GetGameProfitViewAsync(BaseQuery query);
        GameProfitView GetGameProfitView(BaseQuery query);
    }
}
