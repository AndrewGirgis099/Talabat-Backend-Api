using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Services.Contract
{
    public interface IPaymentServices
    {
      Task<CustomerBasket?>  CreateOrUpdatePaymentIntebt(string basketId);
        Task<Order> UpdatePaymentIntentToSuccedOrFaild(string PaymentIntetId, bool flag);
    }
}
