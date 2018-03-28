using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Autofac;
using Hydra.Admin.EventBus.Task;
using Hydra.Admin.EventBus.Configuration;
using Hydra.Admin.EventBus.Event;
using Hydra.Admin.EventBus.Mq.Rabbit;
using Hydra.Admin.Utility.Helper;

namespace Hydra.Admin.EventBus
{
    public static class TranContainer
    {
        private static ContainerBuilder _builder;
        public static List<TaskReceiveConfiguration> TaskReceiveConfigs { get; private set; }

        public static void Build()
        {
            _builder = new ContainerBuilder();
            LoadReceiveConfig();
            foreach (var receiveConfig in TaskReceiveConfigs)
            {
                var task = new EventReceiveTask
                {
                    TaskConfiguration = receiveConfig,
                    MessageQueue = new RabbitMq()
                    {
                        ReceiveConfig = receiveConfig
                    }
                };
                _builder.RegisterInstance(task).Keyed<EventReceiveTask>(receiveConfig.QueueName);
            }
        }

        public static void Start()
        {
            Log.Info("MQ 队列消息 消费开始");
            var taskFactory = new TaskFactory();
            var container = _builder.Build();
            foreach (var receiveConfig in TaskReceiveConfigs)
            {
                var config = receiveConfig;
                taskFactory.StartNew(() =>
                {
                    var task = container.ResolveKeyed<EventReceiveTask>(config.QueueName);
                    task.Receive();
                });
            }
        }
        public static void LoadReceiveConfig()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory+ @"Configs/receiveConfig.xml";
            if (!File.Exists(filePath)) return;
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            XmlSerializer xmlSearializer = new XmlSerializer(typeof(TaskReceiveRootConfiguration));
            var model = (TaskReceiveRootConfiguration)xmlSearializer.Deserialize(file);
            TaskReceiveConfigs = model.ReceiveConfigurations;
            file.Close();
        }
    }
}
