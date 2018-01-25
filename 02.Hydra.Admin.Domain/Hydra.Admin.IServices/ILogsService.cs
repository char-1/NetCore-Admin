using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Utility.iViewControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.IServices
{
    public interface ILogsService : IBaseService<Logs>
    {
        IViewTable<Logs> GetLogsGrid(BaseQuery query);
        bool RemoveLogs(string logIds);
    }
}
