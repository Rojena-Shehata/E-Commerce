using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.OrderModule
{
    public enum OrderStatus
    {
        Pending,
        PaymentRecieved,
        PaymentFailed
    }
}
