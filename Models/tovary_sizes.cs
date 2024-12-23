using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class tovary_sizes
    {
        public int item_id { get; set; }
        public int size_id { get; set; }

        [ForeignKey("item_id")]
        public virtual tovary Tovary { get; set; }

        [ForeignKey("size_id")]
        public virtual sizes Size { get; set; }
    }
}
