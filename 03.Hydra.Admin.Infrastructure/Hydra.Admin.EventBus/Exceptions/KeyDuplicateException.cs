using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.EventBus.Exceptions
{
    public class KeyDuplicateException:TranException
    {
        public KeyDuplicateException(string message)
            : base(message)
        {

        }
        public KeyDuplicateException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
