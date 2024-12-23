using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class ProductSize
    {
        [Key]
        public int id { get; set; }
        public int item_id { get; set; }
        public string size { get; set; }
        public int quantity { get; set; }

        public tovary Product { get; set; }
    }
}
