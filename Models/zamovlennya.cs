using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class zamovlennya
    {
        [Key]
        public int order_id { get; set; }
        public string user_name { get; set; }
        public string user_surname { get; set; }
        public int delivery_service_id { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string delivery_branch { get; set; }
        public string note { get; set; }
        public DateTime order_date { get; set; }
        public decimal total_price { get; set; }
    }

}
