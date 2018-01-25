using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Models.View;
using Hydra.Admin.Utility.iViewControl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.IServices
{
    public interface IAnalysisDashBoardService : IBaseService<AnalysisDashboard>
    {
        IViewTable<AnalysisDashboard> GetDashBoardGrid(BaseQuery query);

        DashBoardView GetDashBoard(DashBoardQuery query);

        Task<DashBoardView> GetDashBoardAsync(DashBoardQuery query);
    }
}
