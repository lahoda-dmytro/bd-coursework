using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WebStore.Models;
using WebStore;

namespace OnlineStoreApp
{
    public partial class AdminWindow : Window
    {
        public ObservableCollection<tovary> Products { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
            LoadProducts();
            DataContext = this;
        }

        private void LoadProducts()
        {
            using (var context = new AppDbContext())
            {
                Products = new ObservableCollection<tovary>(context.tovary.ToList());
                ProductsDataGrid.ItemsSource = Products;
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                foreach (var product in Products)
                {
                    var existingProduct = context.tovary.Find(product.item_id);
                    if (existingProduct != null)
                    {
                        context.Entry(existingProduct).CurrentValues.SetValues(product);
                    }
                    else
                    {
                        context.tovary.Add(product);
                    }
                }
                context.SaveChanges();
                MessageBox.Show("Changes saved to the database.");
            }
        }
    }
}
