using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work_with_db.Tables
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now.ToUniversalTime();
        public string Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

}
