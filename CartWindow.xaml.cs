using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebStore.Models;
using WebStore;

namespace OnlineStoreApp
{
    public partial class CartWindow : Window
    {
        private int userId;
        private ObservableCollection<koshik> cart;

        public CartWindow(int userId, ObservableCollection<koshik> cart)
        {
            InitializeComponent();
            this.userId = userId;
            this.cart = cart;
            DataContext = this;
            LoadCart();
        }

        private void LoadCart()
        {
            CartItemsControl.ItemsSource = cart;
            TotalPriceText.Text = cart.Sum(ci => ci.price * ci.quantity).ToString("C");
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is koshik selectedCartItem)
            {
                var existingCartItem = cart.FirstOrDefault(ci => ci.cart_id == selectedCartItem.cart_id);
                if (existingCartItem != null)
                {
                    if (existingCartItem.quantity > 1)
                    {
                        existingCartItem.quantity -= 1; // Зменшення кількості товару
                    }
                    else
                    {
                        cart.Remove(existingCartItem);
                    }
                }
                LoadCart();
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var checkoutWindow = new CheckoutWindow(userId, cart);
            checkoutWindow.Show();
            this.Close(); // Закриття поточного вікна після переходу на вікно CheckoutWindow
        }
    }
}
