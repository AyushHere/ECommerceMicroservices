using MassTransit;
using NotificationService.Consumers;



namespace NotificationService.Messaging
{
    public static class MassTransitConfig
    {
        public static void AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderPlacedConsumer>();
                x.AddConsumer<OrderStatusChangedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitCfg = configuration.GetSection("RabbitMq");
                    cfg.Host(rabbitCfg["Host"], h =>
                    {
                        h.Username(rabbitCfg["Username"]);
                        h.Password(rabbitCfg["Password"]);
                    });
                
                    cfg.ReceiveEndpoint("order-placed-queue", e =>
                    {
                        e.ConfigureConsumer<OrderPlacedConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("order-status-queue", e =>
                    {
                        e.ConfigureConsumer<OrderStatusChangedConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
