﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingContracts
{
    public interface IStockCheckResponse
    {
        int ProductId { get; }
        bool IsAvailable { get; }
    }
}
