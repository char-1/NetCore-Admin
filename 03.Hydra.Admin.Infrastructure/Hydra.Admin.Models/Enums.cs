using System.ComponentModel;
namespace Hydra.Admin.Models
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum EMenuType
    {
        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        Menu = 0,
        /// <summary>
        /// 按钮
        /// </summary>
        [Description("按钮")]
        Button
    }

    /// <summary>
    /// 是、否
    /// </summary>
    public enum EYesNo
    {
        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        No = 0,
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        Yes,
        Null
    }
    /// <summary>
    /// 状态
    /// </summary>
    public enum EUserState
    {
        /// <summary>
        /// 被锁定
        /// </summary>
        [Description("被锁定")]
        Lock,
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Delete,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal
    }
    public enum EGoldType
    {
        /// <summary>
        /// 系统任务 -
        /// </summary>
        [Description("系统任务")]
        Task,
        /// <summary>
        /// 玩家充值 +
        /// </summary>
        [Description("玩家充值")]
        Rechagre,
        /// <summary>
        /// 玩家提现 -
        /// </summary>
        [Description("玩家提现")]
        Cash,
        /// <summary>
        /// 系统转盘 -
        /// </summary>
        [Description("系统转盘")]
        Turntable,
        /// <summary>
        /// 游戏(下注/结算) +-
        /// </summary>
        [Description("游戏")]
        Game,
        /// <summary>
        /// 系统邮件 -
        /// </summary>
        [Description("系统邮件")]
        EMali
    }
}
