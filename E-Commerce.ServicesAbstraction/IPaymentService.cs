using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.ServicesAbstraction
{
    public interface IPaymentService
    {
        Task<Result<BasketDTO>> CreateORUpdatePaymentIntentAsync(string BasketId);
        Task UpdateOrderPaymentStatus(string requestBody, string requestSignatureHeader);
    }
}
