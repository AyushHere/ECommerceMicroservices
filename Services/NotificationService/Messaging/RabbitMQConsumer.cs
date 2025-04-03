using RabbitMQ.Client;
using System;
using System.Text;
using NotificationService.Services;
using Microsoft.EntityFrameworkCore.Metadata;
namespace NotificationService.Messaging
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly INotificationService _notificationService;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
            var factory = new ConnectionFactory { HostName = "localhost" }; // RabbitMQ Server
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "OrderNotifications", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received message: {message}");

                // Send notification
                await _notificationService.SendNotification(message);
            };

            _channel.BasicConsume(queue: "OrderNotifications", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}