using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work_with_db.Tables
{
    public class ClothingItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SizeId { get; set; }
        public Size Size { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
