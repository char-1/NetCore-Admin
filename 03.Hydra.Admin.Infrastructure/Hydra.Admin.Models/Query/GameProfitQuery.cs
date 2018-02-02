using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Query
{
    public class GameProfitQuery : BaseQuery
    {
        public GameProfitQuery()
        {
            this.type = "all";
        }
        public DateTime? pointDate { get; set; }
        public int? pointType { get; set; }
        public string type { get; set; }
    }
}
