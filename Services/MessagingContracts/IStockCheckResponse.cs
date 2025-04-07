using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingContracts
{
    public interface IStockCheckResponse
    {
        Guid ProductId { get; }
        bool IsAvailable { get; }
    }
}
