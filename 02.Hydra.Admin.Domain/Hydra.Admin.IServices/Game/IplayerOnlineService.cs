using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Models.View;
using System.Threading.Tasks;

namespace Hydra.Admin.IServices
{
    public interface IplayerOnlineService : IBaseService<playerOnline>
    {
        Task<PlayerOnlineView> GetPlayerOnlineChart(DashBoardQuery query);
    }
}
