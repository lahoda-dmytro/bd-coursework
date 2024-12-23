using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using WebStore.Models;
using WebStore;
using System.Windows.Controls;

namespace OnlineStoreApp
{
    public partial class AllTablesWindow : Window
    {
        public ObservableCollection<Users> Users { get; set; }
        public ObservableCollection<tovary> Products { get; set; }
        public ObservableCollection<Orders> Orders { get; set; }
        public ObservableCollection<OrderItems> OrderItems { get; set; }
        public ObservableCollection<categories> Categories { get; set; }

        public AllTablesWindow()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            using (var context = new AppDbContext())
            {
                Users = new ObservableCollection<Users>(context.Users.ToList());
                Products = new ObservableCollection<tovary>(context.tovary.ToList());
                Orders = new ObservableCollection<Orders>(context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Tovar)
                    .Select(o => new Orders
                    {
                        order_id = o.order_id,
                        user_id = o.user_id,
                        address = o.address ?? string.Empty,
                        phone = o.phone ?? string.Empty,
                        email = o.email ?? string.Empty,
                        delivery_branch = o.delivery_branch ?? string.Empty,
                        note = o.note ?? string.Empty,
                        order_date = o.order_date,
                        total_price = o.total_price,
                        status = o.status ?? string.Empty,
                        confirmed = o.confirmed,
                        OrderItems = o.OrderItems.Select(oi => new OrderItems
                        {
                            id = oi.id,
                            order_id = oi.order_id,
                            item_id = oi.item_id,
                            quantity = oi.quantity,
                            price = oi.price,
                            Tovar = new tovary
                            {
                                item_id = oi.Tovar.item_id,
                                name = oi.Tovar.name,
                                size_s = oi.Tovar.size_s,
                                size_m = oi.Tovar.size_m,
                                size_l = oi.Tovar.size_l
                            },
                            size_s = oi.size_s,
                            size_m = oi.size_m,
                            size_l = oi.size_l
                        }).ToList()
                    }).ToList());
                Categories = new ObservableCollection<categories>(context.categories.ToList());

                UsersDataGrid.ItemsSource = Users;
                ProductsDataGrid.ItemsSource = Products;
                OrdersDataGrid.ItemsSource = Orders;
                CategoriesDataGrid.ItemsSource = Categories;
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new tovary
            {
                name = "New Product",
                price = 0.0m,
                photo_url = "https://example.com/photo.jpg",
                description = "Description",
                category_id = 1,
                quantity = 0,
                size_s = 0,
                size_m = 0,
                size_l = 0
            };

            Products.Add(newProduct);

            using (var context = new AppDbContext())
            {
                context.tovary.Add(newProduct);
                context.SaveChanges();
            }
        }

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is tovary selectedProduct)
            {
                Products.Remove(selectedProduct);

                using (var context = new AppDbContext())
                {
                    context.tovary.Remove(selectedProduct);
                    context.SaveChanges();
                }
            }
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Orders selectedOrder)
            {
                using (var context = new AppDbContext())
                {
                    var order = context.Orders.Find(selectedOrder.order_id);
                    if (order != null)
                    {
                        order.confirmed = true;
                        context.SaveChanges();
                        MessageBox.Show("Order confirmed.");
                    }
                }
            }
        }

        private void RemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Orders selectedOrder)
            {
                using (var context = new AppDbContext())
                {
                    var order = context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.order_id == selectedOrder.order_id);
                    if (order != null)
                    {
                        // Видалення всіх пов'язаних OrderItems
                        context.OrderItems.RemoveRange(order.OrderItems);
                        context.Orders.Remove(order);
                        context.SaveChanges();
                        Orders.Remove(selectedOrder);
                        MessageBox.Show("Order removed.");
                    }
                }
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var newCategory = new categories { name = "New Category" };
            Categories.Add(newCategory);

            using (var context = new AppDbContext())
            {
                context.categories.Add(newCategory);
                context.SaveChanges();
            }
        }

        private void CategoriesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "name")
            {
                var editedCategory = e.Row.Item as categories;
                if (editedCategory != null)
                {
                    using (var context = new AppDbContext())
                    {
                        context.categories.Update(editedCategory);
                        context.SaveChanges();
                    }
                }
            }
        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is categories selectedCategory)
            {
                Categories.Remove(selectedCategory);

                using (var context = new AppDbContext())
                {
                    context.categories.Remove(selectedCategory);
                    context.SaveChanges();
                }
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    foreach (var user in Users)
                    {
                        if (context.Users.Any(u => u.id == user.id))
                        {
                            context.Entry(user).State = EntityState.Modified;
                        }
                        else
                        {
                            context.Users.Add(user);
                        }
                    }

                    foreach (var product in Products)
                    {
                        if (context.tovary.Any(p => p.item_id == product.item_id))
                        {
                            context.Entry(product).State = EntityState.Modified;
                        }
                        else
                        {
                            context.tovary.Add(product);
                        }
                    }

                    foreach (var category in Categories)
                    {
                        if (context.categories.Any(c => c.category_id == category.category_id))
                        {
                            context.Entry(category).State = EntityState.Modified;
                        }
                        else
                        {
                            context.categories.Add(category);
                        }
                    }

                    foreach (var order in Orders)
                    {
                        if (context.Orders.Any(o => o.order_id == order.order_id))
                        {
                            context.Entry(order).State = EntityState.Modified;
                            foreach (var orderItem in order.OrderItems)
                            {
                                if (context.OrderItems.Any(oi => oi.id == orderItem.id))
                                {
                                    context.Entry(orderItem).State = EntityState.Modified;
                                }
                                else
                                {
                                    context.OrderItems.Add(orderItem);
                                }
                            }
                        }
                        else
                        {
                            context.Orders.Add(order);
                        }
                    }

                    context.SaveChanges();
                    MessageBox.Show("Changes saved to the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving changes: {ex.Message}");
            }
        }

        private void SaveProductSizesChanges_Click(object sender, RoutedEventArgs e)
        {
            // Реалізуйте код для збереження змін розмірів продуктів
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
