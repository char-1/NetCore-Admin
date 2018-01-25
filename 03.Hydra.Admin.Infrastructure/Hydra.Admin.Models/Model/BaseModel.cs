using System;

namespace Hydra.Admin.Models.Model
{
    public abstract class BaseModel<T>
    {
        public BaseModel()
        {
            CreateTime = DateTime.Now;
            SortNumber = 0;
            IsEnable = true;
        }
        /// <summary>
        /// 主键编号
        /// </summary>
        public T Id { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNumber { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
