using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class sizes
    {
        [Key]
        public int size_id { get; set; }
        public string size { get; set; }
    }

}
