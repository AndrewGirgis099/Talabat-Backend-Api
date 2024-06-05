using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Services.Contract
{
    public interface IOrderServices
    {
        Task<Order?> CreateOrderAsync(string buyerEmail , string basketId , int DelivaeryMethodId , Address shippingAddress);
        Task<IReadOnlyCollection<Order>> GetOrderForSepcificUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdForSepcificUserAsync(string buyerEmail, int OrderId);
    }
}
