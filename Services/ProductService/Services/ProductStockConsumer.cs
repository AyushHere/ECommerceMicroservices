using MassTransit;
using MessagingContracts;
using Microsoft.EntityFrameworkCore.Metadata;
using ProductService.Data;
using RabbitMQ.Client;

namespace ProductService.Services
{
    public class ProductStockConsumer : IConsumer<IStockUpdate>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductStockConsumer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IStockUpdate> context)
        {
            var message = context.Message;
            var product = await _dbContext.Products.FindAsync(message.ProductId);

            if (product != null && product.Stock >= message.Quantity)
            {
                product.Stock -= message.Quantity;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
