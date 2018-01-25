using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.Utility.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class NullException : Exception
    {
        public NullException(string message)
            : base(message)
        {
        }
    }
}
