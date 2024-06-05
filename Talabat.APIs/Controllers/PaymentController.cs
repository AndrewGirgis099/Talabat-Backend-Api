using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_fa9c99d681d56303fbd55c2f24a257d8af1a9c1a23ad861e8d62c5fd23db587f";


        public PaymentController(IPaymentServices paymentServices , IMapper mapper)
        {
            _paymentServices = paymentServices;
            _mapper = mapper;
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var customerBasket = await _paymentServices.CreateOrUpdatePaymentIntebt(basketId);
            if (customerBasket is null) return BadRequest(new ApiResponse(400, "There is a problem with your Basket"));
            var mappedBasket = _mapper.Map<CustomerBasket , CustomerBasketDto>(customerBasket);
            return Ok(mappedBasket);
        }

        [HttpPost("webhook")] 
        public async Task<IActionResult> StripeWebHock()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                  await  _paymentServices.UpdatePaymentIntentToSuccedOrFaild(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentServices.UpdatePaymentIntentToSuccedOrFaild(paymentIntent.Id, true);

                }
            

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

        

    }
}
