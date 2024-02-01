using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AngleFire.Server.Services
{
    public class RabbitMQService
    {
        private readonly string _rabbitMQConnectionString;
        private readonly string _queueName;

        public RabbitMQService(string rabbitMQConnectionString, string queueName)
        {
            _rabbitMQConnectionString = rabbitMQConnectionString;
            _queueName = queueName;
        }

        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_rabbitMQConnectionString) };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void ReceiveMessage(Action<string> messageHandler)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_rabbitMQConnectionString) };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    messageHandler(message);
                };

                channel.BasicConsume(queue: _queueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }
    }
}
