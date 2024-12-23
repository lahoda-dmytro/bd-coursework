using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class categories
    {
        [Key]
        public int category_id { get; set; }
        public string name { get; set; }

        public virtual ICollection<tovary> Tovary { get; set; }
    }
}
