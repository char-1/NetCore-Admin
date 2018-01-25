using Newtonsoft.Json;
using Hydra.Admin.EventBus.Exceptions;
using Hydra.Admin.EventBus.Model;
using System;
using System.Data;

namespace Hydra.Admin.EventBus.Event
{
    public class Event
    {
        private readonly EventType _eventType;

        public EventType EventType
        {
            get { return _eventType; }
        }
        public object Payload { get; set; }
        public string Key { get; set; }
        public long UserId { get; set; }

        public Event(EventType eventType)
        {
            _eventType = eventType;
            Payload = new { };
            Key = string.Empty;
            UserId = 0;
        }

        public void Persistence(IDbConnection connection)
        {
            //try
            //{
            //    var sql = @"INSERT INTO EventPublish(Id,`Key`,Payload,EventType,CreateTime,UserId) 
            //            VALUES (?Id,?Key,?Payload,?EventType,?CreateTime,?UserId)";
            //    connection.Execute(sql, new
            //    {
            //        Id = Guid.NewGuid(),
            //        Key = Key,
            //        Payload = JsonConvert.SerializeObject(Payload),
            //        EventType = _eventType,
            //        CreateTime = DateTime.Now,
            //        UserId = UserId
            //    });
            //}
            //catch (Exception e)
            //{
            //    Log.Error("事件持久化错误",e);
            //    throw new TranException("事件持久化错误",e);
            //}
        }
    }
}
