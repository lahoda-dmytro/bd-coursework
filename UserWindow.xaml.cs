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
                Products = new ObservableCollection<tovary>(context.tovary.ToList().Select(p => new tovary
                {
                    item_id = p.item_id,
                    name = p.name ?? "Unknown", // Обробка NULL значення
                    price = p.price, // Обробка NULL значення
                    photo_url = p.photo_url ?? string.Empty, // Обробка NULL значення
                    description = p.description ?? string.Empty, // Обробка NULL значення
                    category_id = p.category_id,
                    quantity = p.quantity // Обробка NULL значення
                }).ToList());
                ProductsItemsControl.ItemsSource = Products;
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
            TotalPrice.Text = Cart.Sum(item => item.price * item.quantity).ToString("C"); // Врахування кількості товару
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var checkoutWindow = new CheckoutWindow(Cart);
            checkoutWindow.Show();
        }
    }
}
