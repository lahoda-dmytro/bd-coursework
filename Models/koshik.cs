using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class koshik : INotifyPropertyChanged
    {
        private int _quantity;

        public event PropertyChangedEventHandler PropertyChanged;

        [Key]
        public int cart_id { get; set; }
        public int item_id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string size_selected { get; set; }

        public int quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(quantity));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
