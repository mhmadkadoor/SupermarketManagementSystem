using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketManagementSystem
{
    internal class InventoryManager
    {
        public static void AddProduct(string barcode, string productName, int quantity, double price, string category, int criticalAmount)
        { 
            string username = User.CurrentUser.username;

            var product = DatabaseManager.GetProductInfoByBarcode(barcode);
            DatabaseManager.AddProduct(barcode, productName, quantity, price, category, criticalAmount);
            DatabaseManager.RecTimeLine(productName,0,quantity, quantity,(decimal)price);
            DatabaseManager.RecAction(username, $"{username} added {quantity}  of {productName} at price {price} to {category} and the critical amount as :{criticalAmount}");
        }

        public static void DeleteProduct(string barcode)
        {
            string username = User.CurrentUser.username;
            var productInfo = DatabaseManager.GetProductInfoByBarcode(barcode);
            string productName = productInfo != null && productInfo.ContainsKey("product_name") ? productInfo["product_name"]?.ToString() : "Unknown Product";
            DatabaseManager.DeleteProduct(barcode);
            DatabaseManager.RecAction(username, $"{username} removed {productName} from the stock");
        }
     
        public static void EditProduct(string barcode, string productName, int quantity, double price, string category, int criticalAmount)
        {
            string username = User.CurrentUser.username;
            var oldProductInfo = DatabaseManager.GetProductInfoByBarcode(barcode);
            string oldName = oldProductInfo["product_name"].ToString();
            double oldPrice = Convert.ToDouble(oldProductInfo["price"]);
            int oldQuantity = Convert.ToInt32(oldProductInfo["quantity"]);
            string oldCategory = oldProductInfo["category"].ToString();
            int oldCriticalAmount = Convert.ToInt32(oldProductInfo["critical_amount"]);

            int added = 0;
            int sold = 0;
            int priceDiff = oldQuantity - quantity;
            if (priceDiff > 0)
            {
                sold = priceDiff;
            }
            else
            {
                added = -priceDiff;
            }

            DatabaseManager.EditProduct(barcode, productName, quantity, price, category, criticalAmount);
            DatabaseManager.RecTimeLine(productName, sold, added, quantity, (decimal)price);

            List<string> changes = new List<string>();
            if (oldName != productName)
                changes.Add($"name: '{oldName}' → '{productName}'");
            if (oldPrice != price)
                changes.Add($"price: {oldPrice} → {price}");
            if (oldQuantity != quantity)
                changes.Add($"quantity: {oldQuantity} → {quantity}");
            if (oldCategory != category)
                changes.Add($"category: '{oldCategory}' → '{category}'");
            if (oldCriticalAmount != criticalAmount)
                changes.Add($"Critical Amount: '{oldCriticalAmount}' → '{criticalAmount}'");

            if (changes.Count > 0)
            {
                string changeSummary = string.Join(", ", changes);
                DatabaseManager.RecAction(username, $"{username} edited product '{barcode}': {changeSummary}");
            }
        }

        public static bool SellProduct(string barcode, int quantityToSell)
        {
            try
            {
                var product = DatabaseManager.GetProductInfoByBarcode(barcode);

                string productName = product["product_name"].ToString();
                double productPrice = Convert.ToDouble(product["price"]);




                if (product == null)
                {
                    Console.WriteLine("Product not found");
                    return false;
                }

                int currentQuantity = Convert.ToInt32(product["quantity"]);
                if (currentQuantity < quantityToSell)
                {
                    Console.WriteLine("Insufficient quantity");
                    return false;
                }

                // Update inventory
                int newQuantity = currentQuantity - quantityToSell;
                DatabaseManager.RecTimeLine(productName, quantityToSell, 0, newQuantity, (decimal)productPrice);
                UpdateProductQuantity(barcode, newQuantity);

                // Record the sale
                DatabaseManager.RecordSale(
                    barcode,
                    quantityToSell
                );

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error selling product: {ex.Message}");
                return false;
            }
        }

        public static void SellProducts(Dictionary<string, Products> shoppingCart)
        {
            foreach (var item in shoppingCart.Values)
            {
                SellProduct(item.barcode, item.amount);
            }
        }
        private static void UpdateProductQuantity(string barcode, int newQuantity)
        {
            DatabaseManager.UpdateProductQuantity(barcode, newQuantity);
        }

        public static List<Dictionary<string, object>> GetSalesInfo(DateTime startDate, DateTime endDate)
        {
            return DatabaseManager.GetSalesData(startDate, endDate);
        }

        public static List<Dictionary<string, object>> GetInventoryTimeline()
        {
            return DatabaseManager.GetInventoryTimeline();
        }
    }
}
