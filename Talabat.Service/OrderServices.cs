using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecification;

namespace Talabat.Service
{
    public class OrderServices : IOrderServices
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentServices _paymentServices;

        public OrderServices(IBasketRepository basketRepo, IUnitOfWork unitOfWork , IPaymentServices paymentServices)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentServices = paymentServices;
        }
        public async Task<Core.Entities.Order.Order?> CreateOrderAsync(string buyerEmail, string basketId, int DelivaeryMethodId, Address shippingAddress)
        {

            var basket = await _basketRepo.GetBasketAsync(basketId);

            var orderItems = new List<OrderItem>();
            if (basket?.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, item.Price, item.Quentity);
                    orderItems.Add(orderItem);
                }
            }

            var subTotal = orderItems.Sum(item => item.Price * item.Quentity);

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(DelivaeryMethodId);
            var spec = new OrderWithPaymentIntentSpeccification(basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Core.Entities.Order.Order>().GetWithSpecAsync(spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Core.Entities.Order.Order>().Delete(ExOrder);
              await  _paymentServices.CreateOrUpdatePaymentIntebt(basketId);
            }
            var orderStatues = new OrderStatus();
            orderStatues = OrderStatus.Pending;

            var order = new Core.Entities.Order.Order(buyerEmail, shippingAddress, orderStatues, deliveryMethod, orderItems, subTotal , basket.PaymentIntentId);
            await _unitOfWork.Repository<Core.Entities.Order.Order>().Add(order);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }

        public async Task<Core.Entities.Order.Order> GetOrderByIdForSepcificUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderSpcification(buyerEmail, OrderId);
            var order = await _unitOfWork.Repository<Core.Entities.Order.Order>().GetWithSpecAsync(spec);
            return order;

        }

        public async Task<IReadOnlyCollection<Core.Entities.Order.Order>> GetOrderForSepcificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpcification(buyerEmail);
            var orders =await _unitOfWork.Repository<Core.Entities.Order.Order>().GetAllWithSpecAsync(spec);
            return orders;
        }


    }
}
