using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.Utility.Exceptions
{
    /// <summary>
    /// 不满足条件
    /// </summary>
   public class NotContentException : Exception
    {
        public NotContentException(string message)
            : base(message)
        {
        }
    }
}
