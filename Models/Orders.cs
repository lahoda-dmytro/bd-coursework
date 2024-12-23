using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class Orders
    {
        [Key]
        public int order_id { get; set; }
        public int user_id { get; set; }
        public DateTime order_date { get; set; }
        public decimal total_price { get; set; }
        public bool confirmed { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string delivery_branch { get; set; }
        public string note { get; set; }
        public string status { get; set; } 

        [ForeignKey("user_id")]
        public virtual Users User { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
