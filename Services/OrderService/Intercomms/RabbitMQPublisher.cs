using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace OrderService.Intercomms
{
    public class RabbitMQPublisher
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;
        private readonly string _routingKey;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _host = configuration["RabbitMQ:Host"];
            _username = configuration["RabbitMQ:Username"];
            _password = configuration["RabbitMQ:Password"];
            _exchange = configuration["RabbitMQ:Exchange"];
            _routingKey = configuration["RabbitMQ:RoutingKey"];
        }

        public void Publish<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _host,
                UserName = _username,
                Password = _password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Direct);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: _exchange, routingKey: _routingKey, body: body);
        }
    }
}