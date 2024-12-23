using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class koshik
    {
        [Key]
        public int cart_id { get; set; }
        public int item_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public int size_id { get; set; }
        public decimal total_price { get; set; }
    }

}
