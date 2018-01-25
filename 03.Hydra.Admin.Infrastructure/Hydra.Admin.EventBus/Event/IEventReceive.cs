using System.Collections.Generic;
using System.Data;
using Hydra.Admin.EventBus.Model;

namespace Hydra.Admin.EventBus.Event
{
    public interface IEventReceive
    {
        void Receive(KeyValuePair<string,EventPublish> message,IDbConnection connection,IDbTransaction transaction);
    }
}
