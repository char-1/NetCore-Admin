using System.Collections.Generic;

namespace Hydra.Admin.Utility.iViewControl
{
    /// <summary>
    /// IView Table 数据模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IViewTable<T>
    {
        public IViewTable()
        {
            this.total = 0;
            this.rows = new HashSet<T>();
        }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> rows { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get; set; }
    }
}
