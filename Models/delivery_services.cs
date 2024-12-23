using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class delivery_services
    {
        [Key]
        public int service_id { get; set; }
        public string name { get; set; }
    }

}
