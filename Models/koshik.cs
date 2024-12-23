using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class koshik
    {
        [Key]
        public int cart_id { get; set; }
        public int item_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string size_selected { get; set; }

        [ForeignKey("item_id")]
        public virtual tovary Tovar { get; set; }
    }
}
