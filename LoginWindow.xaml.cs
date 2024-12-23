using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using WebStore;
using WebStore.Models;

namespace OnlineStoreApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginAsUserButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            using (var context = new AppDbContext())
            {
                var user = context.Users.SingleOrDefault(u => u.username == username && u.password == password);

                if (user != null)
                {
                    var userWindow = new UserWindow(user.id); // Передача ID користувача
                    userWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
        }

        private void LoginAsAdminButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            using (var context = new AppDbContext())
            {
                var adminAccount = context.admin_account.SingleOrDefault(a => a.username == username && a.password == password);

                if (adminAccount != null)
                {
                    var allTablesWindow = new AllTablesWindow(); // Відкриття вікна з усіма таблицями
                    allTablesWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid admin credentials.");
                }
            }
        }
    }
}
