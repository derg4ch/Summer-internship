using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.dto.clothing_item
{
    public class ClothingItemFilterDto
    {
        public string? Name { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? BrandCountry { get; set; }
        public int? SizeId { get; set; }
        public string? SizeName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
    }
}
