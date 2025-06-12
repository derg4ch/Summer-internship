using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.dto.order
{
    public class OrderNewDto
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ClothingItemId { get; set; }
        public int Quantity { get; set; }
    }
}
