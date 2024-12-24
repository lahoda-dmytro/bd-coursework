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
            TotalPriceText.Text = cart.Sum(ci => ci.price * ci.quantity).ToString("C");
        }


        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
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

                        product.quantity = product.size_s + product.size_m + product.size_l; // Оновлення загальної кількості
                        context.Entry(product).State = EntityState.Modified;
                    }

                    context.OrderItems.Add(orderItem);
                }

                context.SaveChanges();
                MessageBox.Show("Order confirmed successfully.");
                Close(); // Закриття вікна після підтвердження замовлення
            }
        }


    }
}
