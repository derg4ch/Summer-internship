using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work_with_db.Tables
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ClothingItemId { get; set; }
        public ClothingItem ClothingItem { get; set; }

        public int Quantity { get; set; }
    }
}
