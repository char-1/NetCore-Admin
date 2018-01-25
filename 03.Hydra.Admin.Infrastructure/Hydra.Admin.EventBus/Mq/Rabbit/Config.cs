using System.Xml.Serialization;

namespace Hydra.Admin.EventBus.Mq.Rabbit
{
    [XmlRoot("rabbit")]
    public class Config
    {
        [XmlElement("uri")]
        public string Uri { get; set; }
    }
}