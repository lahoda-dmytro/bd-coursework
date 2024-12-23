using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class delivery_services
    {
        [Key]
        public int service_id { get; set; }
        public string name { get; set; }
    }
}
