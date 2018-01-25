using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Hydra.Admin.EventBus.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Hydra.Admin.DataAccess;

namespace Hydra.Admin.EventBus.Mq.Rabbit
{
    public class RabbitMq : IMessageQueue
    {
        public TaskReceiveConfiguration ReceiveConfig { get; set; }

        private static Config RabbitConfig
        {
            get
            {
                var configPath = GetMapPath("/Configs/rabbitMQ.config");
                using (var fs = new FileStream(configPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var xs = new XmlSerializer(typeof(Config));
                    return (Config)xs.Deserialize(fs);
                }
            }
        }

        public void Receive<T>(Action<string, T> action)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(RabbitConfig.Uri)
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(ReceiveConfig.QueueName, true, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    var consumerTag = channel.BasicConsume(ReceiveConfig.QueueName, false, consumer);
                    consumer.Received += (ch, ea) =>
                    {
                        var body = Encoding.UTF8.GetString(ea.Body);
                        var o = JsonConvert.DeserializeObject<T>(body);
                        try
                        {
                            action(ea.BasicProperties.MessageId, o);
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        catch (Exception)
                        {
                            //当处理出现错误时，需断开连接，此消息才会被重新接收到
                            channel.BasicCancel(consumerTag);
                            Receive(action);
                        }
                    };
                    Thread.Sleep(Timeout.Infinite);
                }
            }
        }

        public void Send(string messageId, object message)
        {
            throw new NotImplementedException();
        }

        public static string GetMapPath(string path)
        {
            var applicationBase = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(path)) return (applicationBase + path);
            path = path.Replace("/", @"\");
            if (!path.StartsWith(@"\"))
                path = @"\" + path;
            path = path.Substring(path.IndexOf('\\') + (applicationBase.EndsWith(@"\") ? 1 : 0));
            return (applicationBase + path);
        }
    }
}
