using MassTransit;
using MessagingContracts;
using ProductService.Repositories;

namespace ProductService
{
    public class StockCheckConsumer : IConsumer<IStockCheckRequest>
    {
        private readonly IProductRepository _repository;

        public StockCheckConsumer(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IStockCheckRequest> context)
        {
            var product = await _repository.GetProductById(context.Message.ProductId);
            bool isAvailable = product != null && product.Stock >= context.Message.Quantity;

            await context.RespondAsync<IStockCheckResponse>(new
            {
                IsAvailable = isAvailable
            });
        }
    }
}