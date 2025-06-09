using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.dto.clothing_item
{
    public class ClothingItemNewDto
    {
        public string Name { get; set; } 
        public int SizeId { get; set; }
        public int BrandId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
