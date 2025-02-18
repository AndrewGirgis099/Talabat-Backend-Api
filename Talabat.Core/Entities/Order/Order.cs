﻿using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, OrderStatus status, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal , string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            Status = status;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public Address ShippingAddress { get; set; }
        public OrderStatus Status { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
        public decimal Total()
            => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; }

    }
}
