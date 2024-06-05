using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities.Order;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IOrderServices orderServices, IMapper mapper , IUnitOfWork unitOfWork)
        {
            _orderServices = orderServices;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var mappedAddress = _mapper.Map<AddressDto, Address>(orderDto.shipToAddress);

            var Order = await _orderServices.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DelivaryMethodId, mappedAddress);
            if (Order is null) return BadRequest(new ApiResponse(400, "There is a proplem With Your Product"));
            return Ok(Order);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IReadOnlyCollection<Order>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderServices.GetOrderForSepcificUserAsync(buyerEmail);
            if (orders is null) return NotFound(new ApiResponse(404, "There is no orders for this user"));
            return Ok(orders);

        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderServices.GetOrderByIdForSepcificUserAsync(buyerEmail, id);
            if (order is null) return NotFound(new ApiResponse(404, "there is no order for this user "));
            return Ok(order);
        }


        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethod()
        {
            var deliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(deliveryMethod);
        }


    }
}
