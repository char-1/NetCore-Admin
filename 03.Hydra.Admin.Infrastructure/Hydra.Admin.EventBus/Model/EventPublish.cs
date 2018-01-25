namespace Hydra.Admin.EventBus.Model
{
    public class EventPublish
    {
        public string Payload { get; set; }
        public EventType EventType { get; set; }
    }
}
