using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Utility.iViewControl
{
    /// <summary>
    /// IVew Cascader 数据模型
    /// </summary>
    public class IViewCascader
    {
        public IViewCascader()
        {
            Children = new HashSet<IViewCascader>();
        }
        /// <summary>
        /// 实际值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 显示文字
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 子元素集合
        /// </summary>
        public ICollection<IViewCascader> Children { get; set; }
    }
}
