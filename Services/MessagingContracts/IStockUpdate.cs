using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingContracts
{
    public interface IStockUpdate
    {
        int ProductId { get; }
        int Quantity { get; }
        string Status { get; }
    }
}
