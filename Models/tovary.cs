using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class tovary
    {
        [Key]
        public int item_id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string photo_url { get; set; }
        public string description { get; set; }
        public int category_id { get; set; }
        public int quantity { get; set; }
        public int size_s { get; set; }
        public int size_m { get; set; }
        public int size_l { get; set; }

        [ForeignKey("category_id")]
        public virtual categories Category { get; set; }

        [NotMapped]
        public ObservableCollection<string> AvailableSizes { get; set; } = new ObservableCollection<string> { "S", "M", "L" };

        [NotMapped]
        public string SelectedSize { get; set; }
    }
}
