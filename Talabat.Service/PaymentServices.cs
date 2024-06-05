using Microsoft.Extensions.Configuration;
using Stripe;
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
    public class PaymentServices : IPaymentServices
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentServices(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntebt(string basketId)
        {
            //secretKey 
            StripeConfiguration.ApiKey = _configuration["StripeKey:Secretkey"];
            //GetBasket 
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            if(Basket is null) return null;

            var shippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethodId = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(Basket.DeliveryMethodId.Value);
                shippingPrice = DeliveryMethodId.Cost;
            }
            //Total =  SubTotal + DM.Metheod
            if(Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Core.Entities.Product>().GetAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quentity);



            // create Payment Inetent
            var Services = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var Option = new PaymentIntentCreateOptions()
                {
                    Amount = (long) SubTotal * 100 + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await Services.CreateAsync(Option);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)SubTotal * 100 + (long)shippingPrice * 100,

                };
                paymentIntent = await  Services.UpdateAsync(Basket.PaymentIntentId, option);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
             await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }

        public async Task<Order> UpdatePaymentIntentToSuccedOrFaild(string PaymentIntetId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpeccification(PaymentIntetId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if(flag)
            {
                order.Status = OrderStatus.PaymentSucceeded;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
