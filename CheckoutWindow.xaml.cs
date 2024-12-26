using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WebStore.Models;
using WebStore;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace OnlineStoreApp
{
    public partial class CheckoutWindow : Window
    {
        private int userId;
        private ObservableCollection<koshik> cart;

        public CheckoutWindow(int userId, ObservableCollection<koshik> cart)
        {
            InitializeComponent();
            this.userId = userId;
            this.cart = cart;
            DataContext = this;
            LoadCart();
        }

        private void LoadCart()
        {
            OrderItemsControl.ItemsSource = cart;
            TotalPriceText.Text = cart.Sum(ci => ci.price * ci.quantity).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }



        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(DeliveryBranchTextBox.Text))
            {
                MessageBox.Show("Будь ласка, заповніть усі обов'язкові поля.", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Будь ласка, введіть коректну електронну пошту.", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Будь ласка, заповніть поле Ім'я та Прізвище.", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPhoneNumber(PhoneTextBox.Text))
            {
                MessageBox.Show("Будь ласка, введіть коректний номер телефону у форматі 09XXXXXXXX або +380XXXXXXXXX.", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new AppDbContext())
            {
                var totalPrice = cart.Sum(ci => ci.price * ci.quantity);
                var itemsJson = Newtonsoft.Json.JsonConvert.SerializeObject(cart.Select(ci => new
                {
                    item_id = ci.item_id,
                    quantity = ci.quantity,
                    price = ci.price,
                    size = ci.size_selected
                }));

                context.Database.ExecuteSqlRaw(
                    "EXEC CreateOrder @user_id, @address, @phone, @email, @delivery_branch, @note, @total_price, @order_date, @confirmed, @status, @items",
                    new Microsoft.Data.SqlClient.SqlParameter("@user_id", userId),
                    new Microsoft.Data.SqlClient.SqlParameter("@address", AddressTextBox.Text),
                    new Microsoft.Data.SqlClient.SqlParameter("@phone", PhoneTextBox.Text),
                    new Microsoft.Data.SqlClient.SqlParameter("@email", EmailTextBox.Text),
                    new Microsoft.Data.SqlClient.SqlParameter("@delivery_branch", DeliveryBranchTextBox.Text),
                    new Microsoft.Data.SqlClient.SqlParameter("@note", NoteTextBox.Text),
                    new Microsoft.Data.SqlClient.SqlParameter("@total_price", totalPrice),
                    new Microsoft.Data.SqlClient.SqlParameter("@order_date", DateTime.Now),
                    new Microsoft.Data.SqlClient.SqlParameter("@confirmed", false),
                    new Microsoft.Data.SqlClient.SqlParameter("@status", "Pending"),
                    new Microsoft.Data.SqlClient.SqlParameter("@items", itemsJson)
                );

                MessageBox.Show("Замовлення успішно підтверджено.");
                Close();
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^(\+380\d{9}|09\d{8})$"))
            {
                return true;
            }
            return false;
        }





    }
}
