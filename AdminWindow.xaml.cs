using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebStore;
using WebStore.Models;

namespace OnlineStoreApp
{
    public partial class AdminWindow : Window
    {
        private AppDbContext context;

        public AdminWindow()
        {
            InitializeComponent();
            context = new AppDbContext();
        }

       
        private void SizesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                context.SaveChanges();
            }
        }

        private void SaveSizesChanges_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
            MessageBox.Show("Sizes changes saved successfully.");
        }
    }
}
