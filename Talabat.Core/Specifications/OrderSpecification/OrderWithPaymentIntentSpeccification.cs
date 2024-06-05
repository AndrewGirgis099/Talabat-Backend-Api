using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications.OrderSpecification
{
    public class OrderWithPaymentIntentSpeccification : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpeccification(string PaymentIntentId) : base(o=>o.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
