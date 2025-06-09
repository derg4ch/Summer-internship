using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.dto.order
{
    public class OrderInfoDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public int UserId { get; set; }
        public string UserEmail { get; set; }

        public int TotalItems { get; set; }         
        public decimal TotalPrice { get; set; }    
    }
}
