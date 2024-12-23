using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class categories
    {
        [Key]
        public int category_id { get; set; }
        public string name { get; set; }
    }

}
