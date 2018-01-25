using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.Admin.Utility.Exceptions
{
    /// <summary>
    /// 数据不存在
    /// </summary>
    public class DataNotFoundException : DefinedException
    {
        public DataNotFoundException(string message)
            : base(message)
        {
            
        }
    }
    public class PassNotEqualException : DefinedException
    {
        public PassNotEqualException(string message)
            : base(message)
        {

        }
    }
}
