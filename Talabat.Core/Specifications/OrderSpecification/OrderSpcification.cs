using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications.OrderSpecification
{
    public class OrderSpcification : BaseSpecifications<Entities.Order.Order>
    {
        public OrderSpcification(string email):base(o=>o.BuyerEmail == email)
        {
            Inculeds.Add(o => o.DeliveryMethod);
            Inculeds.Add(o => o.Items);

            AddOrderByDesc(o => o.OrderDate);
        }

        public OrderSpcification(string email , int orderId) : base(o=>o.BuyerEmail==email && o.Id ==orderId) 
        {
            Inculeds.Add(o => o.DeliveryMethod);
            Inculeds.Add(o => o.Items);

        }

    }
}
