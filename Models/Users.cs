using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class Users
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public bool isAdmin { get; set; } // Поле для вказівки, чи є користувач адміністратором

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
