using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebStore;
using WebStore.Models;

namespace OnlineStoreApp
{
    public partial class UserWindow : Window
    {
        public ObservableCollection<tovary> Products { get; set; }
        public ObservableCollection<tovary> FilteredProducts { get; set; }
        public ObservableCollection<tovary> Cart { get; set; }

        public UserWindow()
        {
            InitializeComponent();
            LoadProducts();
            DataContext = this;
            Cart = new ObservableCollection<tovary>();
        }

        private void LoadProducts()
        {
            using (var context = new AppDbContext())
            {
                Products = new ObservableCollection<tovary>(context.tovary
                    .Include(t => t.TovarSizes)
                    .ThenInclude(ts => ts.Size)
                    .ToList().Select(p => new tovary
                    {
                        item_id = p.item_id,
                        name = p.name ?? "Unknown",
                        price = p.price,
                        photo_url = p.photo_url ?? string.Empty,
                        description = p.description ?? string.Empty,
                        category_id = p.category_id,
                        quantity = p.quantity,
                        sizes = string.Join(", ", p.TovarSizes.Select(ts => ts.Size.size)) // З'єднання розмірів у строку
                    }).ToList());
                FilteredProducts = new ObservableCollection<tovary>(Products);
                ProductsItemsControl.ItemsSource = FilteredProducts;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchTextBox.Text.ToLower();
            FilteredProducts.Clear();
            foreach (var product in Products)
            {
                if (product.name.ToLower().Contains(filter) || product.description.ToLower().Contains(filter))
                {
                    FilteredProducts.Add(product);
                }
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is tovary selectedProduct)
            {
                var existingProduct = Cart.FirstOrDefault(p => p.item_id == selectedProduct.item_id);
                if (existingProduct != null)
                {
                    existingProduct.quantity += 1; // Збільшення кількості товару
                }
                else
                {
                    selectedProduct.quantity = 1; // Встановлення кількості товару
                    Cart.Add(selectedProduct);
                }

                UpdateCart();
                MessageBox.Show($"Product '{selectedProduct.name}' added to cart.");
            }
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is tovary selectedProduct)
            {
                var existingProduct = Cart.FirstOrDefault(p => p.item_id == selectedProduct.item_id);
                if (existingProduct != null && existingProduct.quantity > 1)
                {
                    existingProduct.quantity -= 1; // Зменшення кількості товару
                }
                else
                {
                    Cart.Remove(selectedProduct);
                }
                UpdateCart();
            }
        }

        private void UpdateCart()
        {
            CartItemsControl.ItemsSource = null;
            CartItemsControl.ItemsSource = Cart;
            TotalPrice.Text = Cart.Sum(item => item.price * item.quantity).ToString("C");
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var checkoutWindow = new CheckoutWindow(Cart);
            checkoutWindow.Show();
            Cart.Clear(); // Очищення кошика після підтвердження замовлення
            UpdateCart();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
