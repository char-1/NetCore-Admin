using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Hydra.Admin.EventBus.Model;

namespace Hydra.Admin.EventBus.Configuration
{
    public class TaskConfiguration
    {
        [XmlElement("EventType")]
        public EventType EventType { get; set; }

        [XmlElement("EventName")]
        public string EventName { get; set; }

        [XmlElement("ConnectionName")]
        public string ConnectionName { get; set; }
    }
}
