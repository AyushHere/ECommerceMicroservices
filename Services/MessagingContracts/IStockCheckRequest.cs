namespace MessagingContracts;

public interface IStockCheckRequest
{
  
        int ProductId { get; }
        int Quantity { get; }
    
}
