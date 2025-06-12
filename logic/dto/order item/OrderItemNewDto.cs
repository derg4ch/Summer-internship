using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.dto.order_item
{
    public class OrderItemNewDto
    {
        public int OrderId { get; set; }
        public int ClothingItemId { get; set; }
        public int Quantity { get; set; }
    }
}
