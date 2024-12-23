using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using WebStore.Models;
using WebStore;
using System.Windows.Controls;

namespace OnlineStoreApp
{
    public partial class CheckoutWindow : Window
    {
        public ObservableCollection<tovary> Cart { get; set; }

        public CheckoutWindow(ObservableCollection<tovary> cart)
        {
            InitializeComponent();
            Cart = cart;
            LoadOrderDetails();
            UpdateTotalPrice();
        }

        private void LoadOrderDetails()
        {
            foreach (var item in Cart)
            {
                OrderItemsControl.Items.Add(new TextBlock
                {
                    Text = $"{item.name} - {item.price.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))} (x{item.quantity})",
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }
        }

        private void UpdateTotalPrice()
        {
            decimal total = Cart.Sum(item => item.price * item.quantity); // Врахування кількості товару
            TotalPriceText.Text = total.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            // Валідація номеру телефону українського формату
            if (!Regex.IsMatch(PhoneTextBox.Text, @"^\+380\d{9}$"))
            {
                MessageBox.Show("Невірний номер телефону. Формат: +380XXXXXXXXX", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Валідація електронної пошти
            if (!Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Невірний формат електронної пошти.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new AppDbContext())
            {
                var newOrder = new Orders
                {
                    user_id = 1, // Заміна на ідентифікатор авторизованого користувача
                    address = AddressTextBox.Text,
                    phone = PhoneTextBox.Text,
                    email = EmailTextBox.Text,
                    delivery_branch = DeliveryBranchTextBox.Text,
                    note = NoteTextBox.Text,
                    order_date = DateTime.Now,
                    total_price = Cart.Sum(item => item.price * item.quantity), // Обчислення загальної суми
                    confirmed = false
                };

                context.Orders.Add(newOrder);
                context.SaveChanges();

                foreach (var item in Cart)
                {
                    var orderItem = new OrderItems
                    {
                        order_id = newOrder.order_id,
                        item_id = item.item_id,
                        quantity = item.quantity, // Використання значення quantity
                        price = item.price
                    };
                    context.OrderItems.Add(orderItem);

                    // Зменшення кількості товарів у базі даних
                    var product = context.tovary.Find(item.item_id);
                    if (product != null)
                    {
                        product.quantity -= item.quantity; // Зменшення кількості
                        if (product.quantity < 0) product.quantity = 0; // Запобігання від'ємній кількості
                    }
                }

                context.SaveChanges();
            }

            MessageBox.Show("Замовлення успішно оформлено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }


    }
}
