using Hydra.Admin.DataAccess;
using Hydra.Admin.EventBus.Configuration;
using Hydra.Admin.EventBus.Model;
using Hydra.Admin.EventBus.Mq;
using Hydra.Admin.EventBus.Mq.Rabbit;
using Hydra.Admin.Models.Model;
using Newtonsoft.Json;

namespace Hydra.Admin.EventBus.Task
{
    public class EventReceiveTask : SugarContext<playerRegister>
    {
        public EventReceiveTask()
        {
            var mq = new RabbitMq
            {
                ReceiveConfig = TaskConfiguration
            };
            MessageQueue = mq;
        }
        public TaskReceiveConfiguration TaskConfiguration { get; set; }
        public IMessageQueue MessageQueue { get; set; }
        public void Receive()
        {
            MessageQueue.Receive<EventPublish>((key, eventPublish) =>
            {
                if (eventPublish != null)
                {
                    DbAction(db =>
                    {
                        switch (eventPublish.EventType)
                        {
                            case EventType.PlayerRegister:
                                var register = JsonConvert.DeserializeObject<playerRegister>(eventPublish.Payload);
                                db.Insertable(register).ExecuteCommandAsync();
                                break;
                            case EventType.PlayerLogin:
                                var login = JsonConvert.DeserializeObject<playerLogin>(eventPublish.Payload);
                                db.Insertable(login).ExecuteCommandAsync();
                                break;
                            case EventType.PlayerBinded:
                                var bind = JsonConvert.DeserializeObject<playerBind>(eventPublish.Payload);
                                db.Insertable(bind).ExecuteCommandAsync();
                                break;
                            case EventType.PlayerGold:
                                var golds = JsonConvert.DeserializeObject<playerGold>(eventPublish.Payload);
                                db.Insertable(golds).ExecuteCommandAsync();
                                break;
                            
                        }
                    });
                }
            });
        }
    }
}
#region 
//case EventType.PlayerBets:
//                                var message = JsonConvert.DeserializeObject<List<playerBets>>(eventPublish.Payload);
//                                if (message.Any())
//                                {
//                                    foreach (var item in message)
//                                    {
//                                        var playerBet = db.Queryable<playerBets>().Single(s => s.playerId == item.playerId && s.startTime == item.startTime && s.gameId == item.gameId);
//                                        if (playerBet != null)
//                                        {
//                                            var dbBets = JsonConvert.DeserializeObject<List<betItems>>(playerBet.bets);
//                                            if (dbBets.Any())
//                                            {
//                                                var gameBets = JsonConvert.DeserializeObject<List<betItems>>(item.bets);
//                                                foreach (var items in dbBets)
//                                                {
//                                                    dbBets.FirstOrDefault(s => s._id == items._id).score += gameBets.FirstOrDefault(s => s._id == items._id).score;
//                                                }
//                                                var data = JsonConvert.SerializeObject(dbBets);
//var total = dbBets.Sum(s => s.score);
//db.Updateable<playerBets>()
//                                                .UpdateColumns(r => new playerBets
//                                                {
//                                                    bets = data,
//                                                    allBets = total
//                                                })
//                                                .Where(s => s.playerId == item.playerId && s.startTime == item.startTime && s.gameId == item.gameId)
//                                                .ExecuteCommandAsync();
//                                            }
//                                        }
//                                        else
//                                            db.Insertable(message).ExecuteCommandAsync();
//                                    }
//                                }
//                                break;
#endregion