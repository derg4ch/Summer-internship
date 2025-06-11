using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.dto.order_item
{
    public class OrderItemInfoDto
    {
        public int Id { get; set; }
        public int OrderUserId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }

        public int ClothingItemId { get; set; }
        public string ClothingItemName { get; set; }
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
