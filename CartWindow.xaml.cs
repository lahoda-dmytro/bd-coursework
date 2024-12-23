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
        public ObservableCollection<tovary> Cart { get; set; }

        public CartWindow(ObservableCollection<tovary> cart)
        {
            InitializeComponent();
            Cart = cart;
            LoadCartItems();
            UpdateTotalPrice();
        }

        private void LoadCartItems()
        {
            CartItemsControl.Items.Clear();
            foreach (var item in Cart)
            {
                CartItemsControl.Items.Add(new TextBlock
                {
                    Text = $"{item.name} - {item.price.ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))}",
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }
        }

        private void UpdateTotalPrice()
        {
            TotalPriceText.Text = Cart.Sum(item => item.price).ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is tovary selectedItem)
            {
                Cart.Remove(selectedItem);
                LoadCartItems();
                UpdateTotalPrice();
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var checkoutWindow = new CheckoutWindow(Cart);
            checkoutWindow.Show();
            this.Close();
        }
    }
}
