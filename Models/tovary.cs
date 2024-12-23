using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int quantity { get; set; }

        [ForeignKey("category_id")]
        public virtual categories Category { get; set; }

        public virtual ICollection<tovary_sizes> TovarSizes { get; set; } // Навігаційна властивість для розмірів

        [NotMapped]
        public string sizes { get; set; } // Додаткове поле для відображення розмірів
    }
}
