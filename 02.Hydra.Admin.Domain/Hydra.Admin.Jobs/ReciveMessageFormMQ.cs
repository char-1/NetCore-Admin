using Hydra.Admin.DataAccess;
using Hydra.Admin.EventBus.Mq;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Utility.Helper;
using Newtonsoft.Json;

namespace Hydra.Admin.Jobs
{
    public class ReciveMessageFormMQ : SugarContext<AnalysisDashboard>
    {
        /// <summary>
        /// 消费注册消息
        /// </summary>
        //public static void RegisterPlayerQueue()
        //{
        //    var IMessageQueue = new MessageQueue();
        //    IMessageQueue.Receive<playerRegister>("queue.player.register", (messageId, player) =>
        //    {
        //        if (player != null)
        //        {
        //            DbAction(db =>
        //            {
        //                db.Insertable(player).ExecuteCommandAsync();
        //            });
        //        }
        //    });
        //}
        ///// <summary>
        ///// 消费绑定消息
        ///// </summary>
        //public static void BindPlayerQueue()
        //{
        //    var IMessageQueue = new MessageQueue();
        //    IMessageQueue.Receive<playerBind>("queue.player.bind", (messageId, player) =>
        //    {
        //        if (player != null)
        //        {
        //            DbAction(db =>
        //            {
        //                db.Insertable(player).ExecuteCommandAsync();

        //            });
        //        }
        //    });
        //}
        ///// <summary>
        ///// 操作日志
        ///// </summary>
        //public static void GameOperateQueue()
        //{
        //    var IMessageQueue = new MessageQueue();
        //    IMessageQueue.Receive<Logs>("queue.game.operate", (messageId, log) =>
        //    {
        //        if (log != null)
        //        {
        //            DbAction(db =>
        //            {
        //                db.Insertable(log).ExecuteCommandAsync();

        //            });
        //        }
        //    });
        //}
    }
}
