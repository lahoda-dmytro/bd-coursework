using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class tovary
    {
        [Key]
        public int item_id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string photo_url { get; set; }
        public string description { get; set; }
        public int category_id { get; set; }
        public int quantity { get; set; } // Додання властивості quantity
    }
}
