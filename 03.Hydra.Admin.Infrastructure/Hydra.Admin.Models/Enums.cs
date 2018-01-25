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
        /// 任务
        /// </summary>
        [Description("任务")]
        Task,
        /// <summary>
        /// 充值
        /// </summary>
        [Description("充值")]
        Rechagre,
        /// <summary>
        /// 提现
        /// </summary>
        [Description("提现")]
        Cash,
        /// <summary>
        /// 系统
        /// </summary>
        [Description("系统")]
        System,
        /// <summary>
        /// 游戏
        /// </summary>
        [Description("游戏")]
        Game

    }
}
