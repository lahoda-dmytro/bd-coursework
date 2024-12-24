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
        private int userId; 

        public ObservableCollection<tovary> Products { get; set; }
        public ObservableCollection<tovary> FilteredProducts { get; set; }
        public ObservableCollection<koshik> Cart { get; set; } = new ObservableCollection<koshik>();

  


        public UserWindow(int userId)
        {
            InitializeComponent();
            this.userId = userId; 
            LoadCategories();
            LoadProducts();
            DataContext = this;
            Cart = new ObservableCollection<koshik>();
        }

        private void LoadCategories()
        {
            using (var context = new AppDbContext())
            {
                var categories = context.categories.ToList();
                CategoriesComboBox.ItemsSource = categories;
                CategoriesComboBox.DisplayMemberPath = "name";
                CategoriesComboBox.SelectedValuePath = "category_id";
            }
        }

        private void LoadProducts(int? categoryId = null)
        {
            using (var context = new AppDbContext())
            {
                var query = context.tovary.AsQueryable();

                if (categoryId.HasValue)
                {
                    query = query.Where(t => t.category_id == categoryId.Value);
                }

                Products = new ObservableCollection<tovary>(query.ToList().Select(p => new tovary
                {
                    item_id = p.item_id,
                    name = p.name ?? "Unknown",
                    price = p.price,
                    photo_url = p.photo_url ?? string.Empty,
                    description = p.description ?? string.Empty,
                    category_id = p.category_id,
                    quantity = p.quantity,
                    size_s = p.size_s,
                    size_m = p.size_m,
                    size_l = p.size_l,
                    AvailableSizes = new ObservableCollection<string> { "S", "M", "L" }
                }).ToList());
                FilteredProducts = new ObservableCollection<tovary>(Products);
                ProductsItemsControl.ItemsSource = FilteredProducts;
            }
        }

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesComboBox.SelectedItem is categories selectedCategory)
            {
                LoadProducts(selectedCategory.category_id);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is tovary selectedProduct)
            {
                if (string.IsNullOrEmpty(selectedProduct.SelectedSize))
                {
                    MessageBox.Show("Please select a size.");
                    return;
                }

                using (var context = new AppDbContext())
                {
                    var stockItem = context.tovary.FirstOrDefault(p => p.item_id == selectedProduct.item_id);

                    if (stockItem == null)
                    {
                        MessageBox.Show("The selected item is not available.");
                        return;
                    }

                    bool outOfStock = false;
                    switch (selectedProduct.SelectedSize)
                    {
                        case "S":
                            outOfStock = stockItem.size_s < 1;
                            break;
                        case "M":
                            outOfStock = stockItem.size_m < 1;
                            break;
                        case "L":
                            outOfStock = stockItem.size_l < 1;
                            break;
                        default:
                            MessageBox.Show("Invalid size selected.");
                            return;
                    }

                    if (outOfStock)
                    {
                        MessageBox.Show("The selected size is out of stock.");
                        return;
                    }
                }

                var existingCartItem = Cart.FirstOrDefault(ci => ci.item_id == selectedProduct.item_id && ci.size_selected == selectedProduct.SelectedSize);
                if (existingCartItem != null)
                {
                    existingCartItem.quantity += 1;
                }
                else
                {
                    Cart.Add(new koshik
                    {
                        item_id = selectedProduct.item_id,
                        name = selectedProduct.name,
                        quantity = 1,
                        price = selectedProduct.price,
                        size_selected = selectedProduct.SelectedSize
                    });
                }

                UpdateCart();
                MessageBox.Show($"Product '{selectedProduct.name}' ({selectedProduct.SelectedSize}) added to cart.");
            }
        }




        private void UpdateCart()
        {
            CartItemsControl.ItemsSource = null;
            CartItemsControl.ItemsSource = Cart;


            TotalPrice.Text = Cart.Sum(ci => ci.price * ci.quantity).ToString("C");
        }



        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is koshik selectedCartItem)
            {
                var existingCartItem = Cart.FirstOrDefault(ci => ci.cart_id == selectedCartItem.cart_id);
                if (existingCartItem != null)
                {
                    if (existingCartItem.quantity > 1)
                    {
                        existingCartItem.quantity -= 1; 
                    }
                    else
                    {
                        Cart.Remove(existingCartItem);
                    }
                }
                UpdateCart();
            }
        }


        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var checkoutWindow = new CheckoutWindow(userId, Cart);
            checkoutWindow.Show();
            this.Close(); 
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

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
