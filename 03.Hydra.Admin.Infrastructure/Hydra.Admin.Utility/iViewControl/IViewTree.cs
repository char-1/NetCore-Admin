using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Utility.iViewControl
{
    /// <summary>
    /// IView Tree 数据模型
    /// </summary>
    public class IViewTree
    {
        public IViewTree()
        {
            Expand = false;
            Disabled = false;
            Checked = false;
            Selected = false;
            Levele = 1;
            Children = new HashSet<IViewTree>();
        }
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 是否折叠
        /// </summary>
        public bool Expand { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Disabled { get; set; }
        /// <summary>
        /// 是否选中子节点
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Selected { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Levele { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Router { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string RouterName { get; set; }
        /// <summary>
        /// 子元素
        /// </summary>
        public ICollection<IViewTree> Children { get; set; }
    }
}
