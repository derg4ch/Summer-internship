using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.dto.clothing_item
{
    public class ClothingItemInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; } 
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}
