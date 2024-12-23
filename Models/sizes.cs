using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class sizes
    {
        [Key]
        public int size_id { get; set; }
        public string size { get; set; }
    }
}
