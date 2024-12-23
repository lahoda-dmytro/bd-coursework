using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class OrderItems
    {
        [Key]
        public int id { get; set; }
        public int order_id { get; set; }
        public int item_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public int size_s { get; set; } 
        public int size_m { get; set; } 
        public int size_l { get; set; } 

        [ForeignKey("order_id")]
        public virtual Orders Order { get; set; }

        [ForeignKey("item_id")]
        public virtual tovary Tovar { get; set; }
    }
}
