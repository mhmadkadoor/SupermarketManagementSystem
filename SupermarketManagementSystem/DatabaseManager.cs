using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace SupermarketManagementSystem
{
    internal class DatabaseManager
    {
        public static readonly string DbPath = "local.db";
        public static void InitializeDatabase()
        {
            try
            {
                // Check if database file exists, if not, create it
                if (!File.Exists(DbPath))
                {
                    SQLiteConnection.CreateFile(DbPath);
                    Console.WriteLine("Database created successfully!");
                }

                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS users (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            username TEXT NOT NULL UNIQUE,
                            firstname TEXT NOT NULL,
                            lastname TEXT NOT NULL,
                            password TEXT NOT NULL
                        );";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Users table verified/created successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        public static void AddUser(string username, string firstname, string lastname, string password, string roll)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();

                    // Hash the password
                    string hashedPassword = HashPassword(password, username);

                    string insertUserQuery = @"
                        INSERT INTO users (username, firstname, lastname, password, roll)
                        VALUES (@username, @firstname, @lastname, @password, @roll);";

                    using (var command = new SQLiteCommand(insertUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@firstname", firstname);
                        command.Parameters.AddWithValue("@lastname", lastname);
                        command.Parameters.AddWithValue("@password", hashedPassword);
                        command.Parameters.AddWithValue("@roll", roll);

                        command.ExecuteNonQuery();
                        Console.WriteLine("User added successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
            }
        }

        private static string HashPassword(string password, string username)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + username + "Ol3ama"));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + "Ol3ama"));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static bool ValidateUser(string username, string password)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectUserQuery = @"
                        SELECT password FROM users WHERE username = @username;";
                    using (var command = new SQLiteCommand(selectUserQuery, connection))
                    {
                        // Use parameterized queries to prevent SQL injection
                        command.Parameters.Add(new SQLiteParameter("@username", System.Data.DbType.String) { Value = username });
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            string storedHashedPassword = result.ToString();
                            string hashedPassword = HashPassword(password, username);
                            return storedHashedPassword == hashedPassword;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating user: {ex.Message}");
            }
            return false;
        }

        public static Dictionary<string, string> GetUserInfoById(int Id)
        {
            string userId = Id.ToString().Trim();
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectUserQuery = @"  
                      SELECT * FROM users WHERE id = @id;";
                    using (var command = new SQLiteCommand(selectUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", userId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Dictionary<string, string>
                              {
                                  { "username", reader["username"].ToString() },
                                  { "firstname", reader["firstname"].ToString() },
                                  { "lastname", reader["lastname"].ToString() },
                                  { "password", reader["password"].ToString() },
                                  { "roll", reader["roll"].ToString() }
                              };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user info: {ex.Message}");
            }
            return null;
        }
        public static int GetUserIdByUsername(string username)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectUserQuery = @"
                        SELECT id FROM users WHERE username = @username;";
                    using (var command = new SQLiteCommand(selectUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user ID: {ex.Message}");
            }
            return -1;
        }
        public static void UpdateUserInfo(int id, string username, string firstname, string lastname)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string updateUserQuery = @"
                        UPDATE users 
                        SET username = @username, firstname = @firstname, lastname = @lastname
                        WHERE id = @id;";
                    using (var command = new SQLiteCommand(updateUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@firstname", firstname);
                        command.Parameters.AddWithValue("@lastname", lastname);
                        command.ExecuteNonQuery();
                        Console.WriteLine("User info updated successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user info: {ex.Message}");
            }
        }
        public static void DeleteUser(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string deleteUserQuery = @"
                        DELETE FROM users WHERE id = @id;";
                    using (var command = new SQLiteCommand(deleteUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                        Console.WriteLine("User deleted successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
            }
        }
        public static Boolean IsUsernameAveilable(string username)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectUserQuery = @"
                        SELECT COUNT(*) FROM users WHERE username = @username;";
                    using (var command = new SQLiteCommand(selectUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        var result = command.ExecuteScalar();
                        return Convert.ToInt32(result) == 0; // Returns true if username is not taken
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking username availability: {ex.Message}");
            }
            return false; // Default to false in case of an error
        }
        public static void ChangeUserPassword(string username, string newPassword)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string updatePasswordQuery = @"
                        UPDATE users 
                        SET password = @password
                        WHERE username = @username;";
                    using (var command = new SQLiteCommand(updatePasswordQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", HashPassword(newPassword, username));
                        command.ExecuteNonQuery();
                        Console.WriteLine("User password updated successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user password: {ex.Message}");
            }
        }
        public static void AddProduct(string barcode, string productName, int quantity, double price, string category, int criticalAmount)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string insertProductQuery = @"
                        INSERT INTO inventory (barcode, product_name, quantity, price, category, critical_amount)
                        VALUES (@barcode, @product_name, @quantity, @price, @category, @critical_amount);";
                    using (var command = new SQLiteCommand(insertProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@barcode", barcode);
                        command.Parameters.AddWithValue("@product_name", productName);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@category", category);
                        command.Parameters.AddWithValue("@critical_amount", criticalAmount);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Product added successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
        }

        public static void DeleteProduct(string barcode)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string deleteProductQuery = @"
                        DELETE FROM inventory WHERE barcode = @barcode;";
                    using (var command = new SQLiteCommand(deleteProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@barcode", barcode);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Product deleted successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
            }
        }
        public static Dictionary<string, object> GetProductInfoByBarcode(string barcode)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectProductQuery = @"
                        SELECT * FROM inventory WHERE barcode = @barcode;";
                    using (var command = new SQLiteCommand(selectProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@barcode", barcode);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Dictionary<string, object>
                                {
                                    { "barcode", reader["barcode"] },
                                    { "product_name", reader["product_name"] },
                                    { "quantity", reader["quantity"] },
                                    { "price", reader["price"] },
                                    { "category", reader["category"] },
                                    { "critical_amount", reader["critical_amount"] }
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product info: {ex.Message}");
            }
            return null;
        }
        public static void RecAction(string username, string actionName)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();

                    // Ensure the action exists in the actions table
                    string insertActionQuery = @"
                        INSERT OR IGNORE INTO actions (action_name) VALUES (@action_name);";
                    using (var actionCmd = new SQLiteCommand(insertActionQuery, connection))
                    {
                        actionCmd.Parameters.AddWithValue("@action_name", actionName);
                        actionCmd.ExecuteNonQuery();
                    }

                    // Get user id
                    int userId = GetUserIdByUsername(username);
                    if (userId == -1)
                    {
                        Console.WriteLine("User not found for action logging.");
                        return;
                    }

                    // Insert into actions_history
                    string insertHistoryQuery = @"
                        INSERT INTO actions_history (user_id, action) VALUES (@user_id, @action);";
                    using (var historyCmd = new SQLiteCommand(insertHistoryQuery, connection))
                    {
                        historyCmd.Parameters.AddWithValue("@user_id", userId);
                        historyCmd.Parameters.AddWithValue("@action", actionName);
                        historyCmd.ExecuteNonQuery();
                        Console.WriteLine("Action recorded successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording action: {ex.Message}");
            }
        }
    
        public static void EditProduct(string barcode, string productName, int quantity, double price, string category, int criticalAmount)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    // Check if product exists
                    string selectProductQuery = @"SELECT COUNT(*) FROM inventory WHERE barcode = @barcode;";
                    using (var selectCmd = new SQLiteCommand(selectProductQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@barcode", barcode);
                        var result = selectCmd.ExecuteScalar();
                        if (Convert.ToInt32(result) == 0)
                        {
                            Console.WriteLine("Product not found.");
                            return;
                        }
                    }

                    // Update product details
                    string updateProductQuery = @"
                        UPDATE inventory
                        SET product_name = @product_name,
                            quantity = @quantity,
                            price = @price,
                            category = @category,
                            critical_amount = @critical_amount
                        WHERE barcode = @barcode;";
                    using (var updateCmd = new SQLiteCommand(updateProductQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@product_name", productName);
                        updateCmd.Parameters.AddWithValue("@quantity", quantity);
                        updateCmd.Parameters.AddWithValue("@price", price);
                        updateCmd.Parameters.AddWithValue("@category", category);
                        updateCmd.Parameters.AddWithValue("@critical_amount", criticalAmount);
                        updateCmd.Parameters.AddWithValue("@barcode", barcode);
                        updateCmd.ExecuteNonQuery();
                        Console.WriteLine("Product updated successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing product: {ex.Message}");
            }
        }
    
        public static void SellProduct(string barcode, int quantity)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    // Check current quantity
                    string selectQuery = "SELECT quantity FROM inventory WHERE barcode = @barcode;";
                    using (var selectCmd = new SQLiteCommand(selectQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@barcode", barcode);
                        var result = selectCmd.ExecuteScalar();
                        if (result == null)
                        {
                            Console.WriteLine("Product not found.");
                            return;
                        }
                        int currentQuantity = Convert.ToInt32(result);
                        if (currentQuantity < quantity)
                        {
                            Console.WriteLine("Not enough stock to sell.");
                            return;
                        }
                        int newQuantity = currentQuantity - quantity;
                        string updateQuery = "UPDATE inventory SET quantity = @quantity WHERE barcode = @barcode;";
                        using (var updateCmd = new SQLiteCommand(updateQuery, connection))
                        {
                            updateCmd.Parameters.AddWithValue("@quantity", newQuantity);
                            updateCmd.Parameters.AddWithValue("@barcode", barcode);
                            updateCmd.ExecuteNonQuery();
                            RecordSale(barcode, quantity);
                            Console.WriteLine("Product sold successfully!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error selling product: {ex.Message}");
            }
        }

        public static Boolean ProductExsit(string barcode)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectQuery = "SELECT COUNT(*) FROM inventory WHERE barcode = @barcode;";
                    using (var command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@barcode", barcode);
                        var result = command.ExecuteScalar();
                        return Convert.ToInt32(result) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking product existence: {ex.Message}");
            }
            return false;
        }

        public static Dictionary<string, object>[] GetAllProducts()
        {
            var products = new List<Dictionary<string, object>>();
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectAllQuery = "SELECT * FROM inventory;";
                    using (var command = new SQLiteCommand(selectAllQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new Dictionary<string, object>
                                {
                                    { "barcode", reader["barcode"] },
                                    { "product_name", reader["product_name"] },
                                    { "quantity", reader["quantity"] },
                                    { "price", reader["price"] },
                                    { "category", reader["category"] },
                                    { "critical_amount", reader["critical_amount"] }
                                };
                                products.Add(product);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all products: {ex.Message}");
            }
            return products.ToArray();
        }

        public static Dictionary<string, object>[] SearchInv(string searchTerm)
        {
            var products = new List<Dictionary<string, object>>();
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectQuery = @"
                        SELECT * FROM inventory
                        WHERE barcode LIKE @search
                           OR product_name LIKE @search
                           OR category LIKE @search
                           OR price LIKE @search;";
                    using (var command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new Dictionary<string, object>
                                {
                                    { "barcode", reader["barcode"] },
                                    { "product_name", reader["product_name"] },
                                    { "quantity", reader["quantity"] },
                                    { "price", reader["price"] },
                                    { "category", reader["category"] },
                                    { "critical_amount", reader["critical_amount"] }
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching inventory: {ex.Message}");
            }
            return products.ToArray();
        }
        public static void InitializeSalesTable()
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string createSalesTableQuery = @"
                        CREATE TABLE IF NOT EXISTS sales (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            barcode TEXT NOT NULL,
                            product_name TEXT NOT NULL,
                            quantity_sold INTEGER NOT NULL,
                            unit_price REAL NOT NULL,
                            total_price REAL NOT NULL,
                            sale_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                            user_id INTEGER,
                            FOREIGN KEY (user_id) REFERENCES users(id)
                        );";

                    using (var command = new SQLiteCommand(createSalesTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sales table: {ex.Message}");
            }
        }

        // Record a sale
        public static void RecordSale(string barcode, int quantitySold)
        {   
            var product = GetProductInfoByBarcode(barcode);
            string productName = product["product_name"].ToString();
            double unitPrice = (double)product["price"];
            int userId = User.CurrentUser.id;
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string insertSaleQuery = @"
                        INSERT INTO sales (barcode, product_name, quantity_sold, unit_price, total_price, user_id)
                        VALUES (@barcode, @product_name, @quantity_sold, @unit_price, @total_price, @user_id);";

                    using (var command = new SQLiteCommand(insertSaleQuery, connection))
                    {
                        command.Parameters.AddWithValue("@barcode", barcode);
                        command.Parameters.AddWithValue("@product_name", productName);
                        command.Parameters.AddWithValue("@quantity_sold", quantitySold);
                        command.Parameters.AddWithValue("@unit_price", unitPrice);
                        command.Parameters.AddWithValue("@total_price", quantitySold * unitPrice);
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording sale: {ex.Message}");
            }
        }

        // Get sales data for a specific date range
        public static List<Dictionary<string, object>> GetSalesData(DateTime startDate, DateTime endDate)
        {
            var salesData = new List<Dictionary<string, object>>();
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectSalesQuery = @"
                        SELECT s.*, u.username 
                        FROM sales s 
                        LEFT JOIN users u ON s.user_id = u.id 
                        WHERE DATE(s.sale_date) BETWEEN @start_date AND @end_date
                        ORDER BY s.sale_date DESC;";

                    using (var command = new SQLiteCommand(selectSalesQuery, connection))
                    {
                        command.Parameters.AddWithValue("@start_date", startDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@end_date", endDate.ToString("yyyy-MM-dd"));

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                salesData.Add(new Dictionary<string, object>
                                {
                                    {"id", reader["id"]},
                                    {"barcode", reader["barcode"]},
                                    {"product_name", reader["product_name"]},
                                    {"quantity_sold", reader["quantity_sold"]},
                                    {"unit_price", reader["unit_price"]},
                                    {"total_price", reader["total_price"]},
                                    {"sale_date", reader["sale_date"]},
                                    {"username", reader["username"]}
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting sales data: {ex.Message}");
            }
            return salesData;
        }

        // Get inventory timeline data
        public static List<Dictionary<string, object>> GetInventoryTimeline()
        {
            var timelineData = new List<Dictionary<string, object>>();
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string selectTimelineQuery = @"
                        SELECT 
                            id,
                            product_name,
                            sold,
                            added,
                            stock,
                            price,
                            date
                        FROM inventory_timeline
                        ORDER BY id DESC;";

                    using (var command = new SQLiteCommand(selectTimelineQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                timelineData.Add(new Dictionary<string, object>
                                {
                                    {"id", reader["id"]},
                                    {"product_name", reader["product_name"]},
                                    {"sold", reader["sold"]},
                                    {"added", reader["added"]},
                                    {"stock", reader["stock"]},
                                    {"price", reader["price"]},
                                    {"date", reader["date"]}
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting inventory timeline: {ex.Message}");
            }
            return timelineData;
        }


        public static void UpdateProductQuantity(string barcode, int newQuantity)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string updateQuery = @"
                UPDATE inventory 
                SET quantity = @quantity 
                WHERE barcode = @barcode;";

                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@quantity", newQuantity);
                        command.Parameters.AddWithValue("@barcode", barcode);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Product quantity updated successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product quantity: {ex.Message}");
            }
        }
        

        // Update RecTimeLine to insert date (optional, since default is set)
        public static void RecTimeLine(string product_name, int sold, int added, int stock, decimal price)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string insertTimelineQuery = @"
                        INSERT INTO inventory_timeline (product_name, sold, added, stock, price, date)
                        VALUES (@product_name, @sold, @added, @stock, @price, CURRENT_TIMESTAMP);";
                    using (var command = new SQLiteCommand(insertTimelineQuery, connection))
                    {
                        command.Parameters.AddWithValue("@product_name", product_name);
                        command.Parameters.AddWithValue("@sold", sold);
                        command.Parameters.AddWithValue("@added", added);
                        command.Parameters.AddWithValue("@stock", stock);
                        command.Parameters.AddWithValue("@price", price);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Inventory timeline record added successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding inventory timeline record: {ex.Message}");
            }
        }
        public static Products[] GetAlarms()
        {
            var alarms = new List<Products>();
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string query = @"
                        SELECT barcode, product_name, price, quantity, critical_amount
                        FROM inventory
                        WHERE quantity <= critical_amount;";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var barcode = reader["barcode"].ToString();
                                var product = new Products(barcode);
                                alarms.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving alarm products: {ex.Message}");
            }
            return alarms.ToArray();
        }
    
       


    }

}
