using System.Windows;

namespace OnlineStoreApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
