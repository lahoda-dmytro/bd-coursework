using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class admin_account
    {
        [Key]
        public int admin_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
    }

}
