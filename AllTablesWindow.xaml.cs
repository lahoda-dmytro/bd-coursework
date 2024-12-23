using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using WebStore.Models;
using WebStore;

namespace OnlineStoreApp
{
    public partial class AllTablesWindow : Window
    {
        public ObservableCollection<Users> Users { get; set; }
        public ObservableCollection<tovary> Products { get; set; }
        public ObservableCollection<admin_log> AdminLogs { get; set; }
        public ObservableCollection<Orders> Orders { get; set; }
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
                AdminLogs = new ObservableCollection<admin_log>(context.admin_log.ToList());
                Orders = new ObservableCollection<Orders>(context.Orders
                    .Include(o => o.OrderItems)
                    .Select(o => new Orders
                    {
                        order_id = o.order_id,
                        user_id = o.user_id,
                        delivery_service_id = o.delivery_service_id,
                        address = o.address,
                        phone = o.phone,
                        email = o.email,
                        delivery_branch = o.delivery_branch,
                        note = o.note,
                        order_date = o.order_date,
                        total_price = o.total_price,
                        confirmed = o.confirmed,
                        OrderItems = o.OrderItems.Select(oi => new OrderItems
                        {
                            id = oi.id,
                            order_id = oi.order_id,
                            item_id = oi.item_id,
                            quantity = oi.quantity,
                            price = oi.price
                        }).ToList()
                    }).ToList());
                Categories = new ObservableCollection<categories>(context.categories.ToList());

                UsersDataGrid.ItemsSource = Users;
                ProductsDataGrid.ItemsSource = Products;
                AdminLogsDataGrid.ItemsSource = AdminLogs;
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
                quantity = 0
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

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                foreach (var user in Users)
                {
                    if (context.Users.Any(u => u.Id == user.Id))
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

                context.SaveChanges();
                MessageBox.Show("Changes saved to the database.");
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
