using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupermarketManagementSystem
{
    
    public partial class DashboardForm : DevExpress.XtraEditors.XtraForm
    {
       
        Dictionary<string, object>[] allProducts = DatabaseManager.GetAllProducts();
        //pos 
        private Dictionary<string, Products> shoppingCart = new Dictionary<string, Products>();
        decimal cartTotal = 0;
        


        public DashboardForm()
        {
            InitializeComponent();
            LoadInvTable();
            
            PanelManeger.InitializePanels(this, dashboardPnl, posPnl , inventoryPnl, reportsPnl, profilePnl);
            NavbarManager.InitializeLabels(dashboardNavLbl, posNavLbl, inventoryNavLbl,reportsNavLbl, profileNavLbl,  logoutNavLbl);
            NavbarManager.rollTool();
            UpdateComponents();
            PanelManeger.ShowPanelByName("dashboard");
            LoadDashboardData();
        }
        private void UpdateComponents()
        {
            if (User.CurrentUser != null)
            {
                // Set for Profile panel
                usernameTxtF.Text = User.CurrentUser.username;
                firstnameTxtF.Text = User.CurrentUser.firstname;
                lastnameTxtF.Text = User.CurrentUser.lastname;
            }
        }

        private void logoutNavLbl_Click(object sender, EventArgs e)
        {
            User.CurrentUser = null; // Clear the current user
            PanelManeger.ShowPanelByName("dashboard");
            this.Close();
        }

        private void posNavLbl_Click(object sender, EventArgs e)
        {
            PanelManeger.ShowPanelByName("pos");

        }

        private void inventoryNavLbl_Click(object sender, EventArgs e)
        {
            PanelManeger.ShowPanelByName("inventory");
            LoadInvTable();
        }

        private void profileNavLbl_Click(object sender, EventArgs e)
        {

            PanelManeger.ShowPanelByName("profile");
            UpdateComponents();

        }
        private void reportsNavLbl_Click(object sender, EventArgs e)
        {
            PanelManeger.ShowPanelByName("report");
            loadtimeline();
            LoadSalesTable(startDatePicker.Value, endDatePicker.Value);
        }

        private void dashboardNavLbl_Click(object sender, EventArgs e)
        {
            PanelManeger.ShowPanelByName("dashboard");
            LoadDashboardData();
        }

        private void updatenamesBtn_Click(object sender, EventArgs e)
        {
            string username= usernameTxtF.Text.Trim();
            firstnameTxtF.Text = firstnameTxtF.Text.Trim();
            lastnameTxtF.Text = lastnameTxtF.Text.Trim();

            if (string.IsNullOrEmpty(firstnameTxtF.Text) || string.IsNullOrEmpty(lastnameTxtF.Text) || string.IsNullOrEmpty(usernameTxtF.Text))
            {
                notifacationProfUULbl.Text = "Please enter your first name, last name,\n and username.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
                return;
            }
            else if (firstnameTxtF.Text.Length < 4 || firstnameTxtF.Text.Length > 15 ||
                     lastnameTxtF.Text.Length < 4 || lastnameTxtF.Text.Length > 15 ||
                     username.Length < 4 || username.Length > 15)
            {
                notifacationProfUULbl.Text = "First name, last name,\nand username must be \nbetween 4 and 15 characters.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
                return;
            }
            else if (username.Contains(" "))
            {
                notifacationProfUULbl.Text = "Username cannot contain spaces.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
                return;
            }
            else if (!DatabaseManager.IsUsernameAveilable(username) && User.CurrentUser.username != username  )
            {
                notifacationProfUULbl.Text = "Username is already taken.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
                return;
            }
            else if (User.CurrentUser.username == username || DatabaseManager.IsUsernameAveilable(username))//hbbhhjb
            {
                DatabaseManager.UpdateUserInfo(User.CurrentUser.id, username, firstnameTxtF.Text, lastnameTxtF.Text);
                notifacationProfUULbl.Text = "Profile updated successfully.";
                notifacationProfUULbl.ForeColor = Color.Green;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
            }
            else
            {
                notifacationProfUULbl.Text = "Failed to update profile.";
                notifacationProfUULbl.ForeColor = Color.Red;
                updatenamesProfUUBtn.Location = new Point(updatenamesProfUUBtn.Location.X, notifacationProfUULbl.Bottom + 25);
                notifacationProfUULbl.Visible = true;
            }

        }

        private void updatepasswordProfUPBtn_Click(object sender, EventArgs e)
        {
            string currentpassword = currentpasswordProfUPTxtF.Text.Trim();
            string newpassword = newpasswordProfUPTxtF.Text.Trim();
            string confirmpassword = confirmpasswordProfUPTxtF.Text.Trim();
            if (string.IsNullOrEmpty(currentpassword) || string.IsNullOrEmpty(newpassword) || string.IsNullOrEmpty(confirmpassword))
            {
                notifacationProfUPLbl.Text = "Please enter your \ncurrent password, new password,\n and confirm password.";
                notifacationProfUPLbl.ForeColor = Color.Red;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
                return;
            }
            else if (newpassword.Length < 8 || newpassword.Length > 15)
            {
                notifacationProfUPLbl.Text = "New password must be \nbetween 8 and 15 characters.";
                notifacationProfUPLbl.ForeColor = Color.Red;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
                return;
            }
            else if (newpassword != confirmpassword)
            {
                notifacationProfUPLbl.Text = "New password and confirm \npassword do not match.";
                notifacationProfUPLbl.ForeColor = Color.Red;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
                return;
            }
            else if (DatabaseManager.ValidateUser(User.CurrentUser.username, currentpassword))
            {
                DatabaseManager.ChangeUserPassword(User.CurrentUser.username, newpassword);
                notifacationProfUPLbl.Text = "Password updated successfully.";
                notifacationProfUPLbl.ForeColor = Color.Green;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
            }
            else
            {
                notifacationProfUPLbl.Text = "Current password is incorrect.";
                notifacationProfUPLbl.ForeColor = Color.Red;
                updatepasswordProfUPBtn.Location = new Point(updatepasswordProfUPBtn.Location.X, notifacationProfUPLbl.Bottom + 25);
                notifacationProfUPLbl.Visible = true;
            }
        }



        private void saveUpdateInvAEBtn_Click(object sender, EventArgs e)
        {
            string barcode = barcodeInvAETxtF.Text;
            string productName = productnameInvAETxtF.Text;
            double price = Convert.ToDouble(priceInvAENup.Value);
            int quantity = Convert.ToInt32(quantityInvAENup.Value);
            if (barcode.Length <= 0 || productName.Length <= 0 ) { 
                        notifacationInvLbl.Text = "All fields are required"; 
                        notifacationInvLbl.ForeColor = Color.Red; 
                        notifacationInvLbl.Visible = true; 
            }
            else if (DatabaseManager.ProductExsit(barcode))
            {

                InventoryManager.EditProduct(barcode, productName, quantity, price, "def", Convert.ToInt32(criticalInvNup.Value) );
                notifacationInvLbl.Text = "Product updated seccessfully"; notifacationInvLbl.ForeColor = Color.Green; notifacationInvLbl.Visible = true;

            }
            else {

                InventoryManager.AddProduct(barcode, productName, quantity, price, "def", Convert.ToInt32(criticalInvNup.Value));
                if (DatabaseManager.ProductExsit(barcode))
                {
                    notifacationInvLbl.Text = "Product added seccessfully"; notifacationInvLbl.ForeColor = Color.Green; notifacationInvLbl.Visible = true;
                }
                else { notifacationInvLbl.Text = "Something Went Worng"; notifacationInvLbl.ForeColor = Color.Red; notifacationInvLbl.Visible = true; }
            }
            allProducts = DatabaseManager.SearchInv(searchBarInv.Text.Trim());
            LoadInvTable();

        }

        private void barcodeInvAETxtF_EditValueChanged(object sender, EventArgs e)
        {
            if (DatabaseManager.ProductExsit(barcodeInvAETxtF.Text.Trim()))
            {
                saveUpdateInvAEBtn.Text = "Update Product";
                fetchInvAELbl.Visible = true;
            }
            else 
            {
                saveUpdateInvAEBtn.Text = "Save Product";
                fetchInvAELbl.Visible= false;
            }
        }


        private void LoadInvTable()

        {
            string searchedKeyword = searchBarInv.Text.Trim();

            if (searchedKeyword.Length > 0)
            {
                allProducts = DatabaseManager.SearchInv(searchedKeyword);
                inventoryViewer.Rows.Clear();

                foreach (var product in allProducts)
                {
                    inventoryViewer.Rows.Add(
                        product["barcode"],
                        product["product_name"],
                        product["quantity"],
                        product["price"],
                        product["category"],
                        product["critical_amount"]
                    );
                }
            }
            else
            {
                allProducts = DatabaseManager.GetAllProducts();
                inventoryViewer.Rows.Clear();

                foreach (var product in allProducts)
                {
                    inventoryViewer.Rows.Add(
                        product["barcode"],
                        product["product_name"],
                        product["quantity"],
                        product["price"],
                        product["category"],
                        product["critical_amount"]
                    );
                }
            }

        }


        private void searchRefreshLbl_Click(object sender, EventArgs e)
        {
            LoadInvTable();
        }

        private void fetchInvAELbl_Click(object sender, EventArgs e)
        {
            var productInfo = DatabaseManager.GetProductInfoByBarcode(barcodeInvAETxtF.Text.Trim());
            if (productInfo != null) {
                productnameInvAETxtF.Text = productInfo["product_name"].ToString();
                priceInvAENup.Value = (decimal)(double)productInfo["price"];
                quantityInvAENup.Value = Convert.ToInt32(productInfo["quantity"]);
                criticalInvNup.Value = Convert.ToInt32(productInfo["critical_amount"]);


            }
        }


        private void inventoryViewer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the delete button was clicked
            if (e.ColumnIndex == inventoryViewer.Columns["DeleteButton"].Index && e.RowIndex >= 0)
            {
                // Get the barcode from the selected row
                string barcode = inventoryViewer.Rows[e.RowIndex].Cells["barcodeCol"].Value?.ToString();
                string productName = inventoryViewer.Rows[e.RowIndex].Cells["productnameCol"].Value?.ToString();

                if (!string.IsNullOrEmpty(barcode))
                {
                    // Show confirmation dialog
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete '{productName}'?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            // Call your delete method
                            InventoryManager.DeleteProduct(barcode);

                            // Refresh the table after deletion
                            allProducts = DatabaseManager.SearchInv(searchBarInv.Text.Trim());
                            LoadInvTable();

                            MessageBox.Show($"Product '{productName}' has been deleted successfully.",
                                          "Delete Successful",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting product: {ex.Message}",
                                          "Delete Error",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void loadSalesDataBtn_Click(object sender, EventArgs e)
        {
            LoadSalesTable(startDatePicker.Value, endDatePicker.Value);
        }
        private void LoadSalesTable(DateTime startDate, DateTime endDate )
        {
            var salesData = InventoryManager.GetSalesInfo(startDate, endDate);

            salesDataGridView.Rows.Clear();

            foreach (var sale in salesData)
            {
                // Format unit_price and total_price as decimal with $ sign
                decimal unitPrice = 0, totalPrice = 0;
                if (sale["unit_price"] is decimal)
                    unitPrice = (decimal)sale["unit_price"];
                else if (sale["unit_price"] is double)
                    unitPrice = Convert.ToDecimal((double)sale["unit_price"]);
                else if (sale["unit_price"] != null)
                    unitPrice = Convert.ToDecimal(sale["unit_price"]);

                if (sale["total_price"] is decimal)
                    totalPrice = (decimal)sale["total_price"];
                else if (sale["total_price"] is double)
                    totalPrice = Convert.ToDecimal((double)sale["total_price"]);
                else if (sale["total_price"] != null)
                    totalPrice = Convert.ToDecimal(sale["total_price"]);

                string unitPriceFormatted = $"${unitPrice:F2}";
                string totalPriceFormatted = $"${totalPrice:F2}";

                // Format sale_date as string
                string saleDateFormatted = "";
                if (sale["sale_date"] is DateTime dt)
                    saleDateFormatted = dt.ToString("yyyy-MM-dd HH:mm:ss");
                else if (sale["sale_date"] != null)
                    saleDateFormatted = sale["sale_date"].ToString();

                salesDataGridView.Rows.Add(
                    sale["barcode"],         // barcodeSaleCol
                    sale["product_name"],    // productnameSaleCol
                    sale["quantity_sold"],   // quantitysoldSaleCol
                    unitPriceFormatted,      // unitpriceSaleCol
                    totalPriceFormatted,     // totalpriceSaleCol
                    saleDateFormatted,       // saledateSaleCol
                    sale["username"]         // usernameSaleCol
                );
            }
        }

        private void generateSalesPdfBtn_Click(object sender, EventArgs e)
        {
            
            string pdfPath = ReportManager.CreateSalesReportPdf(startDatePicker.Value, endDatePicker.Value);
            if (!string.IsNullOrEmpty(pdfPath))
            {
                MessageBox.Show($"Sales report generated: {pdfPath}");
                System.Diagnostics.Process.Start(pdfPath);
            }
        }

        private void generateTimelinePdfBtn_Click(object sender, EventArgs e)
        {
            string pdfPath = ReportManager.GenerateInventoryTimelinePDF();
            if (!string.IsNullOrEmpty(pdfPath))
            {
                MessageBox.Show($"Timeline report generated: {pdfPath}");
                System.Diagnostics.Process.Start(pdfPath);
            }
        }

        public void LoadDashboardData()
        {
            try
            {
                var salesData = InventoryManager.GetSalesInfo(DateTime.Now.AddDays(-30), DateTime.Now);

                double totalSales = 0;
                int totalTransactions = salesData.Count;
                Dictionary<string, int> productSales = new Dictionary<string, int>();

                foreach (var sale in salesData)
                {
                    totalSales += Convert.ToDouble(sale["total_price"]);
                    string productName = sale["product_name"].ToString();

                    if (productSales.ContainsKey(productName))
                        productSales[productName] += Convert.ToInt32(sale["quantity_sold"]);
                    else
                        productSales[productName] = Convert.ToInt32(sale["quantity_sold"]);
                }

                totalSalesLbl.Text = $"Total Sales (Last 30 days): ${totalSales:F2}";
                totalTransactionsLbl.Text = $"Total Transactions: {totalTransactions}";

                if (productSales.Count > 0)
                {
                    var topProduct = productSales.OrderByDescending(x => x.Value).First();
                    topProductLbl.Text = $"Top Selling Product: {topProduct.Key} ({topProduct.Value} sold)";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading summary: {ex.Message}");
            }
            LoadAlarmPanel();
        }

        private void loadTimelineBtn_Click(object sender, EventArgs e)
        {
            loadtimeline();
        }

        private void loadtimeline()
        {

            var timelineData = InventoryManager.GetInventoryTimeline();

            timelineDataGridView.Rows.Clear();

            foreach (var entry in timelineData)
            {
                // Format price as decimal with $ sign
                decimal price = 0;
                if (entry["price"] is decimal)
                    price = (decimal)entry["price"];
                else if (entry["price"] is double)
                    price = Convert.ToDecimal((double)entry["price"]);
                else if (entry["price"] != null)
                    price = Convert.ToDecimal(entry["price"]);

                string priceFormatted = $"${price:F2}";

                // Format date as string (if needed)
                string dateFormatted = "";
                if (entry["date"] is DateTime dt)
                    dateFormatted = dt.ToString("yyyy-MM-dd HH:mm:ss");
                else if (entry["date"] != null)
                    dateFormatted = entry["date"].ToString();

                timelineDataGridView.Rows.Add(
                    entry["product_name"],
                    entry["sold"],
                    entry["added"],
                    entry["stock"],
                    priceFormatted,
                    dateFormatted
                );
            }
        }

        private void barcodePosTxtF_TextChanged(object sender, EventArgs e)
        {


                string barcode = barcodePosTxtF.Text.Trim();
                int amount = Convert.ToInt32(amountPosNup.Value);
                ProcessBarcode(barcode, amount);
                cashOutRefresh();

        }
        private void ProcessBarcode(string barcode, int amount)
        {
            if (string.IsNullOrWhiteSpace(barcode) || amount <= 0)
                return;

            if (shoppingCart.ContainsKey(barcode))
            {
                if (shoppingCart[barcode].amount + amount <= Convert.ToInt32(DatabaseManager.GetProductInfoByBarcode(barcode)["quantity"])) 
                {
                    shoppingCart[barcode].amount += amount;
                    barcodePosTxtF.Clear();
                    amountPosNup.Value = 1;
                    updateCart();
                }else
                {
                    barcodePosTxtF.Clear();
                    amountPosNup.Value = 1;
                    updateCart();
                    MessageBox.Show("Not enough stock available for this product.", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                Products product = new Products(barcode, amount);
                if (product.name != null)
                {

                    if (product.amount <= Convert.ToInt32(DatabaseManager.GetProductInfoByBarcode(barcode)["quantity"]))
                    {
                        shoppingCart[barcode] = product;
                        barcodePosTxtF.Clear();
                        amountPosNup.Value = 1;
                        updateCart();
                    }
                    else
                    {
                        barcodePosTxtF.Clear();
                        amountPosNup.Value = 1;
                        updateCart();
                        MessageBox.Show("Not enough stock available for this product.", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }
        }

        private void updateCart()
        {
            cartPosDgv.Rows.Clear();
            int counter = 0;
            decimal total = 0;
            foreach (var item in shoppingCart.Values)
            {
                total += item.price * item.amount;
                cartPosDgv.Rows.Add(
                    counter++,
                    item.barcode,
                    item.name,
                    item.amount,
                    item.price,
                    item.price * item.amount
                );
            }
            cartTotal = total;
            carttotalPosLbl.Text = $"Total: ${total}";
            cashOutRefresh();
        }

        private void clearcartPosLbl_Click(object sender, EventArgs e)
        {
            shoppingCart.Clear();
            updateCart();
        }

        private void cartPosDgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == cartPosDgv.Columns["delete"].Index && e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    // With this corrected line:
                    string barcode = cartPosDgv.Rows[e.RowIndex].Cells["barcode"].Value?.ToString();
                    shoppingCart.Remove(barcode);
                    updateCart();
                }
            }
        }

        private void moneyamount200Lbl_Click(object sender, EventArgs e)
        {
            clientPaidPosNud.Value += 200;
        }

        private void moneyamount100Lbl_Click(object sender, EventArgs e)
        {
            clientPaidPosNud.Value += 100;
        }

        private void moneyamount50Lbl_Click(object sender, EventArgs e)
        {
            clientPaidPosNud.Value += 50;
        }

        private void moneyamount20Lbl_Click(object sender, EventArgs e)
        {
            clientPaidPosNud.Value += 20;
        }

        private void moneyamount10Lbl_Click(object sender, EventArgs e)
        {
            clientPaidPosNud.Value += 10;
        }

        private void moneyamount5Lbl_Click(object sender, EventArgs e)
        {
            clientPaidPosNud.Value += 5;
        }

        private void clientPaidPosNud_ValueChanged(object sender, EventArgs e)
        {
            cashOutRefresh();
        }
        private void cashOutRefresh()
        {
            decimal leftOver = clientPaidPosNud.Value - cartTotal;
            leftoverPosLbl.Text = $"Left Over: ${leftOver}";
        }

        private void cashoutPosBtn_Click(object sender, EventArgs e)
        {
            if (clientPaidPosNud.Value - cartTotal >= 0)
            {
                string pdfPath = ReportManager.CreateReceiptPdf(shoppingCart, Convert.ToDecimal(clientPaidPosNud.Value));
                InventoryManager.SellProducts(shoppingCart);
                cartTotal = 0; clientPaidPosNud.Value = 0; shoppingCart.Clear(); updateCart(); cashOutRefresh();
                if (!string.IsNullOrEmpty(pdfPath))
                {
                    var result = MessageBox.Show("Do you want to open the receipt now?", "Open Receipt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(pdfPath);
                    }

                }
            }
            else
            {
                MessageBox.Show("The client didn't pay enough. Please collect the full amount before proceeding.", "Insufficient Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void LoadAlarmPanel()
        {
            alarmDbPnl.Controls.Clear();
            var alarmProducts = DatabaseManager.GetAlarms();
            int y = 10;
            foreach (var product in alarmProducts)
            {
                var lbl = new Label();
                lbl.AutoSize = true;
                lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lbl.ForeColor = Color.White;
                lbl.BackColor = Color.FromArgb(192, 0, 0); // Red background
                lbl.Padding = new Padding(10, 5, 10, 5);
                lbl.Text = $"Low Stock: {product.name} (Stock: {product.stock}, Critical: {product.criticalAmount})";
                lbl.Location = new Point(10, y);
                lbl.BorderStyle = BorderStyle.FixedSingle;
                alarmDbPnl.Controls.Add(lbl);
                y += lbl.Height + 10;
            }
            if (alarmProducts.Length == 0)
            {
                var lbl = new Label();
                lbl.AutoSize = true;
                lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lbl.ForeColor = Color.White;
                lbl.BackColor = Color.FromArgb(192, 0, 0);
                lbl.Padding = new Padding(10, 5, 10, 5);
                lbl.Text = "No low stock products.";
                lbl.Location = new Point(10, y);
                lbl.BorderStyle = BorderStyle.FixedSingle;
                alarmDbPnl.Controls.Add(lbl);
            }
        }
    }
}