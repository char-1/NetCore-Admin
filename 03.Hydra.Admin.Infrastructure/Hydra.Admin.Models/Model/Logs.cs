using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.Models.Model
{
    public class Logs : BaseModel<string>
    {
        public string User { get; set; }
        public string IP { get; set; }
        public string Types { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Remark { get; set; }
    }
}
