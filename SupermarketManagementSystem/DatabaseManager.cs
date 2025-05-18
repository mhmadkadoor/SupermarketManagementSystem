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
        //private static readonly string DbPath = "local.db";
        private static readonly string DbPath = "C:\\Users\\mhmad\\source\\repos\\SupermarketManagementSystem\\SupermarketManagementSystem\\local.db";
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

        public static void AddUser(string username, string firstname, string lastname, string password)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();

                    // Hash the password
                    string hashedPassword = HashPassword(password);

                    string insertUserQuery = @"
                        INSERT INTO users (username, firstname, lastname, password)
                        VALUES (@username, @firstname, @lastname, @password);";

                    using (var command = new SQLiteCommand(insertUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@firstname", firstname);
                        command.Parameters.AddWithValue("@lastname", lastname);
                        command.Parameters.AddWithValue("@password", hashedPassword);

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
                            string hashedPassword = HashPassword(password);
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
                                  { "password", reader["password"].ToString() }
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
                        command.Parameters.AddWithValue("@password", HashPassword(newPassword));
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
        public static void AddProduct(string barcode, string productName, int quantity, double price, string category)
        {
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={DbPath}; Version=3;"))
                {
                    connection.Open();
                    string insertProductQuery = @"
                        INSERT INTO inventory (barcode, product_name, quantity, price, category)
                        VALUES (@barcode, @product_name, @quantity, @price, @category);";
                    using (var command = new SQLiteCommand(insertProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@barcode", barcode);
                        command.Parameters.AddWithValue("@product_name", productName);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@category", category);
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
                                    { "category", reader["category"] }
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
    
         
    }

}
