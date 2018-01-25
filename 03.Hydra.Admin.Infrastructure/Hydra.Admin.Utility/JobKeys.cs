using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Utility
{
    /// <summary>
    /// Job 调度key 集合
    /// </summary>
    public class JobKeys
    {
        /// <summary>
        /// 生成月份数据集合
        /// </summary>
        public static readonly string STBulidMonth = "Job_STBulidMonth";
        /// <summary>
        /// 汇总玩家数据
        /// </summary>
        public static readonly string STBulidPlayer = "Job_STBulidPlayer";
        /// <summary>
        /// 消费玩家注册消息
        /// </summary>
        public static readonly string ReciveRegisterPlayer = "MQ_ReciveRegistPlayerQueue";
        /// <summary>
        /// 消费玩家注册消息
        /// </summary>
        public static readonly string ReciveBindPlayer = "MQ_ReciveBindPlayerQueue";
    }
}
