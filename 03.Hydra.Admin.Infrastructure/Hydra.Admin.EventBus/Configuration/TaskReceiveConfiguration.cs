using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hydra.Admin.EventBus.Configuration
{
    [XmlRoot("Receive")]
    public class TaskReceiveRootConfiguration
    {
        [XmlArray("Events"), XmlArrayItem("Event")]
        public List<TaskReceiveConfiguration> ReceiveConfigurations { get; set; }
    }
    public class TaskReceiveConfiguration:TaskConfiguration
    {
        [XmlElement]
        public string QueueName { get; set; }
    }
}
