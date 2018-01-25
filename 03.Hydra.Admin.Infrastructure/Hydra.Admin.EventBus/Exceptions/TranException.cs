using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.EventBus.Exceptions
{
    public class TranException:Exception
    {
        public TranException(string message)
            : base(message)
        {

        }
        public TranException(string message,Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
