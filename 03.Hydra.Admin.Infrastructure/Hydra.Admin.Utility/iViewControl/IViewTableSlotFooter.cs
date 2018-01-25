using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Utility.iViewControl
{
    public class IViewTableSlotFooter<T, T1>
    {
        public IViewTableSlotFooter()
        {
            this.total = 0;
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> rows { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 统计栏
        /// </summary>
        public T1 footer { get; set; }
    }
}
