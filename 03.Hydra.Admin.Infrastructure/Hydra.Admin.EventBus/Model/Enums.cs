using System.ComponentModel;

namespace Hydra.Admin.EventBus.Model
{
    public enum EventPublishStatus : sbyte
    {
        /// <summary>
        /// 新事件，刚产生的事件，等待写入消息队列
        /// </summary>
        [Description("新事件")]
        New = 0,
        /// <summary>
        /// 已发布，已写入消息队列
        /// </summary>
        [Description("已发布")]
        Published = 1,
        /// <summary>
        /// 已处理
        /// </summary>
        [Description("已处理")]
        Processed = 2,
    }
    public enum EventProcessStatus : sbyte
    {
        /// <summary>
        /// 新事件，刚从消息队列中获取到事件
        /// </summary>
        [Description("新事件")]
        New = 0,
        /// <summary>
        /// 已处理
        /// </summary>
        [Description("已处理")]
        Processed = 1

    }
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType : short
    {
        /// <summary>
        /// 玩家注册
        /// </summary>
        [Description("玩家注册")]
        PlayerRegister = 1000,
        /// <summary>
        /// 玩家绑定
        /// </summary>
        [Description("玩家绑定")]
        PlayerBinded = 1001,
        /// <summary>
        /// 玩家登录
        /// </summary>
        [Description("玩家登录")]
        PlayerLogin = 1002,
        /// <summary>
        /// 玩家充值
        /// </summary>
        [Description("玩家充值")]
        PlayerRecharge = 1004,
        /// <summary>
        /// 玩家金币
        /// </summary>
        [Description("玩家金币")]
        PlayerGold = 1003,
        /// <summary>
        /// 玩家下注
        /// </summary>
        [Description("玩家下注")]
        PlayerBets = 2000,
        /// <summary>
        /// 游戏盈利
        /// </summary>
        [Description("游戏盈利")]
        GameProfit = 2001,
        /// <summary>
        /// 后台操作日志
        /// </summary>
        [Description("后台操作日志")]
        SystemOperate = 9000
    }
}
