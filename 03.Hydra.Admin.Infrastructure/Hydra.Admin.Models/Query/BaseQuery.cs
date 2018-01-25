using System;

namespace Hydra.Admin.Models.Query
{
    public class BaseQuery
    {
        public BaseQuery()
        {
            this.p = 1;
            this.size = 20;
        }
        /// <summary>
        /// 页码
        /// </summary>
        public int? p { get; set; }
        /// <summary>
        /// 分页数
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sidx { get; set; }
        /// <summary>
        /// 是否倒序
        /// </summary>
        public string sord { get; set; }
        public string keyword { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? stime { get; set; }
        public DateTime? etime { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? qetime
        {
            get
            {
                if (etime.HasValue) return etime.Value.AddDays(1);
                else return null;
            }
        }
    }
}
