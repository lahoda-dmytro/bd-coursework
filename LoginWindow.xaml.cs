using System.Linq;
using System.Windows;
using WebStore;

namespace OnlineStoreApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginAsUser_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordTextBox.Password;

            using (var context = new AppDbContext())
            {
                var user = context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    var userWindow = new UserWindow();
                    userWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
        }

        private void LoginAsAdmin_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordTextBox.Password;

            using (var context = new AppDbContext())
            {
                var admin = context.admin_account.SingleOrDefault(a => a.username == username && a.password == password);
                if (admin != null)
                {
                    var allTablesWindow = new AllTablesWindow();
                    allTablesWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid admin username or password.");
                }
            }
        }
    }
}
