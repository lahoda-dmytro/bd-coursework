using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WebStore.Models;
using WebStore;
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
                var order = new Orders
                {
                    user_id = userId,
                    order_date = DateTime.Now,
                    total_price = cart.Sum(ci => ci.price * ci.quantity),
                    confirmed = false,
                    address = AddressTextBox.Text,
                    phone = PhoneTextBox.Text,
                    email = EmailTextBox.Text,
                    delivery_branch = DeliveryBranchTextBox.Text,
                    note = NoteTextBox.Text,
                    status = "Pending"
                };

                context.Orders.Add(order);
                context.SaveChanges();

                foreach (var cartItem in cart)
                {
                    var orderItem = new OrderItems
                    {
                        order_id = order.order_id,
                        item_id = cartItem.item_id,
                        quantity = cartItem.quantity,
                        price = cartItem.price,
                        size_s = cartItem.size_selected == "S" ? cartItem.quantity : 0,
                        size_m = cartItem.size_selected == "M" ? cartItem.quantity : 0,
                        size_l = cartItem.size_selected == "L" ? cartItem.quantity : 0
                    };

                    var product = context.tovary.FirstOrDefault(p => p.item_id == cartItem.item_id);
                    if (product != null)
                    {
                        if (cartItem.size_selected == "S")
                            product.size_s -= cartItem.quantity;
                        if (cartItem.size_selected == "M")
                            product.size_m -= cartItem.quantity;
                        if (cartItem.size_selected == "L")
                            product.size_l -= cartItem.quantity;

                        product.quantity = product.size_s + product.size_m + product.size_l;
                        context.Entry(product).State = EntityState.Modified;
                    }

                    context.OrderItems.Add(orderItem);
                }

                context.SaveChanges();
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
