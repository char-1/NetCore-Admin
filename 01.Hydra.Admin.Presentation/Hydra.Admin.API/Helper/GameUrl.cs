namespace Hydra.Admin.API.Helper
{
    public class GameUrl
    {
        //Notice
        public const string GetNotice = "getNotice.nd";//获取通知公告
        public const string SetNotice = "setNotice.nd";//设置通知公告
        public const string RemNotice = "remNotice.nd";//删除通知公告
        public const string EabNotice = "noticeEnable.nd";//启用/停用通知公告
        //Game
        public const string GetGameConfig = "getconfig.nd";//获取通知公告
        public const string SetGameConfig = "setconfig.nd";//设置通知公告
        //Task
        public const string GetTaskConfig = "gettasklist.nd";//获取任务配置
        public const string SetTaskConfig = "settaskinfo.nd";//设置任务配置
        public const string RemTaskConfig = "taskremove.nd";//删除任务
        public const string EabTaskConfig = "enabletask.nd";//启用/停用任务
        public const string SgnTaskConfig = "singletask.nd";//获取单条任务
        //BUG
        public const string GetBugs = "getbuglist.nd";
        public const string SetBug = "setbug.nd";
        public const string RemBug = "rembug.nd";
        //Email
        public const string GetMails = "getemailist.nd";
        public const string SendMail = "sendmail.nd";
        //Single Config
        public const string GetSingleConfig = "getSingle.nd";
        public const string SetSingleConfig = "setSingle.nd";
        public const string CreateRobot = "addRobot.nd";
        //Players
        public const string GetPlayers = "select.nd";//玩家集合
        public const string GetPlayer = "getplayers.nd";//单个玩家
        public const string SetPlayerGold = "addgold.nd";//上/下分
        public const string SetPlayerFreeze = "setFreeze.nd";//解/锁玩家
        public const string GetPlayerRelated = "getPlayerRelated.nd";//获取玩家相关信息(游戏、充值、提现)
        public const string GetPlayerGoldRank = "getPlayerGoldRank.nd";//获取玩家金币排行(游客、正式，排除机器人)
        public const string GetDialRecord = "getDialRecord.nd";//抽奖记录
        //Spark
        public const string GetSpark = "getSpark.nd";
        public const string SetSpark = "setSpark.nd";
        //Product
        public const string GetProduct = "getProduct.nd";
        public const string SetProduct = "addProduct.nd";
        public const string RemProduct = "delProduct.nd";
        //CashRecord 
        public const string GetPlayerOrder = "getPlayerOrder.nd";//获取提现记录
        public const string HandleCashRecord = "handleCashRecord.nd";//处理提现申请

    }
}
