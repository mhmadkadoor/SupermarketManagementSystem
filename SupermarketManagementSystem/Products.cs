using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketManagementSystem
{
    internal class Products
    {
        public string name { get; }
        public decimal price { get; }

        public int amount = 0;
        public string barcode {  get; }
        public  int stock = 0;
        public int criticalAmount = 0;
        public Products(string barcode, int amount)
        {
            this.barcode = barcode;
            if (DatabaseManager.ProductExsit(barcode))
            {
                var product = DatabaseManager.GetProductInfoByBarcode(barcode);
                this.name = product["product_name"].ToString();
                this.price = (decimal)(double)product["price"];
                this.amount = amount;
                this.stock = Convert.ToInt32(product["quantity"]);
            }
        }
        public Products(string barcode) 
        {
            var product = DatabaseManager.GetProductInfoByBarcode(barcode);
            this.name = product["product_name"].ToString();
            this.price = (decimal)(double)product["price"];
            this.stock = Convert.ToInt32(product["quantity"]);
            this.criticalAmount = Convert.ToInt32(product["critical_amount"]);
        }
    }
}
