namespace MessagingContracts;

public interface IStockCheckRequest
{
  
        Guid ProductId { get; }
        int Quantity { get; }
    
}
