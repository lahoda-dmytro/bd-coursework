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
        public ObservableCollection<admin_log> AdminLogs { get; set; }
        public ObservableCollection<tovary> ProductSizes { get; set; }  // Колекція для розмірів продуктів

        public int CurrentAdminId { get; set; }

        public AllTablesWindow(int adminId)
        {
            InitializeComponent();
            CurrentAdminId = adminId;
            LoadData();
            LoadProductSizes();  // Завантаження розмірів продуктів
            DataContext = this;
        }

        public AllTablesWindow()
        {
            InitializeComponent();
            LoadData();
            LoadProductSizes();  // Завантаження розмірів продуктів
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
                AdminLogs = new ObservableCollection<admin_log>(context.admin_log.ToList());

                UsersDataGrid.ItemsSource = Users;
                ProductsDataGrid.ItemsSource = Products;
                OrdersDataGrid.ItemsSource = Orders;
                CategoriesDataGrid.ItemsSource = Categories;
                AdminLogsDataGrid.ItemsSource = AdminLogs;
            }
        }

        private void LoadProductSizes()
        {
            using (var context = new AppDbContext())
            {
                var productSizes = context.tovary
                    .Select(p => new
                    {
                        p.item_id,
                        p.size_s,
                        p.size_m,
                        p.size_l
                    })
                    .ToList();

                ProductSizesDataGrid.ItemsSource = productSizes;
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
                context.Database.ExecuteSqlRaw(
                    "EXEC AddProduct @name, @price, @photo_url, @description, @category_id, @quantity, @size_s, @size_m, @size_l",
                    new Microsoft.Data.SqlClient.SqlParameter("@name", newProduct.name),
                    new Microsoft.Data.SqlClient.SqlParameter("@price", newProduct.price),
                    new Microsoft.Data.SqlClient.SqlParameter("@photo_url", newProduct.photo_url),
                    new Microsoft.Data.SqlClient.SqlParameter("@description", newProduct.description),
                    new Microsoft.Data.SqlClient.SqlParameter("@category_id", newProduct.category_id),
                    new Microsoft.Data.SqlClient.SqlParameter("@quantity", newProduct.quantity),
                    new Microsoft.Data.SqlClient.SqlParameter("@size_s", newProduct.size_s),
                    new Microsoft.Data.SqlClient.SqlParameter("@size_m", newProduct.size_m),
                    new Microsoft.Data.SqlClient.SqlParameter("@size_l", newProduct.size_l)
                );
            }

            LoadProductSizes();  // Оновити список розмірів після додавання нового товару
        }

        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is tovary selectedProduct)
            {
                Products.Remove(selectedProduct);

                using (var context = new AppDbContext())
                {
                    context.Database.ExecuteSqlRaw(
                        "EXEC RemoveProductSize @item_id",
                        new Microsoft.Data.SqlClient.SqlParameter("@item_id", selectedProduct.item_id)
                    );
                }
            }
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Orders selectedOrder)
            {
                using (var context = new AppDbContext())
                {
                    context.Database.ExecuteSqlRaw(
                        "EXEC ConfirmOrder @order_id",
                        new Microsoft.Data.SqlClient.SqlParameter("@order_id", selectedOrder.order_id)
                    );
                    MessageBox.Show("Order confirmed.");
                }
            }
        }

        private void RemoveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Orders selectedOrder)
            {
                Orders.Remove(selectedOrder);

                using (var context = new AppDbContext())
                {
                    context.Database.ExecuteSqlRaw(
                        "EXEC RemoveOrder @order_id",
                        new Microsoft.Data.SqlClient.SqlParameter("@order_id", selectedOrder.order_id)
                    );
                    MessageBox.Show("Order removed.");
                }
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var newCategory = new categories { name = "New Category" };
            Categories.Add(newCategory);

            using (var context = new AppDbContext())
            {
                context.Database.ExecuteSqlRaw(
                    "EXEC AddCategory @name",
                    new Microsoft.Data.SqlClient.SqlParameter("@name", newCategory.name)
                );
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
                        context.Database.ExecuteSqlRaw(
                            "EXEC UpdateCategory @category_id, @name",
                            new Microsoft.Data.SqlClient.SqlParameter("@category_id", editedCategory.category_id),
                            new Microsoft.Data.SqlClient.SqlParameter("@name", editedCategory.name)
                        );
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
                    context.Database.ExecuteSqlRaw(
                        "EXEC RemoveCategory @category_id",
                        new Microsoft.Data.SqlClient.SqlParameter("@category_id", selectedCategory.category_id)
                    );
                }
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                   

                    foreach (var product in Products)
                    {
                        context.Database.ExecuteSqlRaw(
                            "EXEC SaveProduct @item_id, @name, @price, @photo_url, @description, @category_id, @quantity, @size_s, @size_m, @size_l",
                            new Microsoft.Data.SqlClient.SqlParameter("@item_id", product.item_id),
                            new Microsoft.Data.SqlClient.SqlParameter("@name", product.name),
                            new Microsoft.Data.SqlClient.SqlParameter("@price", product.price),
                            new Microsoft.Data.SqlClient.SqlParameter("@photo_url", product.photo_url),
                            new Microsoft.Data.SqlClient.SqlParameter("@description", product.description),
                            new Microsoft.Data.SqlClient.SqlParameter("@category_id", product.category_id),
                            new Microsoft.Data.SqlClient.SqlParameter("@quantity", product.quantity),
                            new Microsoft.Data.SqlClient.SqlParameter("@size_s", product.size_s),
                            new Microsoft.Data.SqlClient.SqlParameter("@size_m", product.size_m),
                            new Microsoft.Data.SqlClient.SqlParameter("@size_l", product.size_l)
                        );

                        if (!string.IsNullOrEmpty(product.name))
                        {
                            var adminLogEntry = new admin_log
                            {
                                admin_id = CurrentAdminId,
                                action = "Updated product: " + product.name,
                                change_date = DateTime.Now
                            };
                            context.admin_log.Add(adminLogEntry);
                            Console.WriteLine($"Logging action: {adminLogEntry.action} by admin ID: {adminLogEntry.admin_id} on {adminLogEntry.change_date}");
                        }
                    }

                    foreach (var category in Categories)
                    {
                        context.Database.ExecuteSqlRaw(
                            "EXEC SaveCategory @category_id, @name",
                            new Microsoft.Data.SqlClient.SqlParameter("@category_id", category.category_id),
                            new Microsoft.Data.SqlClient.SqlParameter("@name", category.name)
                        );

                        if (!string.IsNullOrEmpty(category.name))
                        {
                            var adminLogEntry = new admin_log
                            {
                                admin_id = CurrentAdminId,
                                action = "Updated category: " + category.name,
                                change_date = DateTime.Now
                            };
                            context.admin_log.Add(adminLogEntry);
                            Console.WriteLine($"Logging action: {adminLogEntry.action} by admin ID: {adminLogEntry.admin_id} on {adminLogEntry.change_date}");
                        }
                    }

                    foreach (var order in Orders)
                    {
                        context.Database.ExecuteSqlRaw(
                            "EXEC SaveOrder @order_id, @user_id, @address, @phone, @email, @delivery_branch, @note, @total_price, @order_date, @confirmed, @status",
                            new Microsoft.Data.SqlClient.SqlParameter("@order_id", order.order_id),
                            new Microsoft.Data.SqlClient.SqlParameter("@user_id", order.user_id),
                            new Microsoft.Data.SqlClient.SqlParameter("@address", order.address),
                            new Microsoft.Data.SqlClient.SqlParameter("@phone", order.phone),
                            new Microsoft.Data.SqlClient.SqlParameter("@email", order.email),
                            new Microsoft.Data.SqlClient.SqlParameter("@delivery_branch", order.delivery_branch),
                            new Microsoft.Data.SqlClient.SqlParameter("@note", order.note),
                            new Microsoft.Data.SqlClient.SqlParameter("@total_price", order.total_price),
                            new Microsoft.Data.SqlClient.SqlParameter("@order_date", order.order_date),
                    new Microsoft.Data.SqlClient.SqlParameter("@confirmed", order.confirmed),
                    new Microsoft.Data.SqlClient.SqlParameter("@status", order.status)
                );

                        foreach (var orderItem in order.OrderItems)
                        {
                            context.Database.ExecuteSqlRaw(
                                "EXEC SaveOrderItem @id, @order_id, @item_id, @quantity, @price, @size_s, @size_m, @size_l",
                                new Microsoft.Data.SqlClient.SqlParameter("@id", orderItem.id),
                                new Microsoft.Data.SqlClient.SqlParameter("@order_id", orderItem.order_id),
                                new Microsoft.Data.SqlClient.SqlParameter("@item_id", orderItem.item_id),
                                new Microsoft.Data.SqlClient.SqlParameter("@quantity", orderItem.quantity),
                                new Microsoft.Data.SqlClient.SqlParameter("@price", orderItem.price),
                                new Microsoft.Data.SqlClient.SqlParameter("@size_s", orderItem.size_s),
                                new Microsoft.Data.SqlClient.SqlParameter("@size_m", orderItem.size_m),
                                new Microsoft.Data.SqlClient.SqlParameter("@size_l", orderItem.size_l)
                            );
                        }

                        var adminLogEntry = new admin_log
                        {
                            admin_id = CurrentAdminId,
                            action = "Updated order: " + order.order_id,
                            change_date = DateTime.Now
                        };
                        context.admin_log.Add(adminLogEntry);
                        Console.WriteLine($"Logging action: {adminLogEntry.action} by admin ID: {adminLogEntry.admin_id} on {adminLogEntry.change_date}");
                    }

                    context.SaveChanges();
                    MessageBox.Show("Changes saved to the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving changes: {ex.Message}\n\n{ex.InnerException?.Message}");
            }
        }

        private void SaveProductSizesChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    foreach (var productSize in ProductSizesDataGrid.ItemsSource.Cast<dynamic>())
                    {
                        context.Database.ExecuteSqlRaw(
                            "EXEC UpdateProductSize @item_id, @size_s, @size_m, @size_l",
                            new Microsoft.Data.SqlClient.SqlParameter("@item_id", productSize.item_id),
                            new Microsoft.Data.SqlClient.SqlParameter("@size_s", productSize.size_s),
                            new Microsoft.Data.SqlClient.SqlParameter("@size_m", productSize.size_m),
                            new Microsoft.Data.SqlClient.SqlParameter("@size_l", productSize.size_l)
                        );
                    }

                    context.SaveChanges();
                    MessageBox.Show("Product sizes changes saved to the database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving product sizes changes: {ex.Message}");
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
