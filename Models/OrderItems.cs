using System.ComponentModel.DataAnnotations;

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
        public string size { get; set; }

        public Orders Order { get; set; }
    }
}
