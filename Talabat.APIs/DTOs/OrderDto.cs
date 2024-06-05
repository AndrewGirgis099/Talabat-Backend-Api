using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order;

namespace Talabat.APIs.DTOs
{
    public class OrderDto
    {

        [Required]
        public string BasketId { get; set; }
        public int DelivaryMethodId { get; set; }
        public AddressDto shipToAddress { get; set; }
    }
}
