using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SupermarketManagementSystem
{
    internal class ReportManager
    {

        public static string GenerateInventoryTimelinePDF()
        {
            try
            {
                // 1. Prepare file path
                string dateStr = DateTime.Now.ToString("yyyyMMdd");
                string fileName = $"InventoryTimeline_{dateStr}.pdf";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                // 2. Fetch data using InventoryManager
                var data = InventoryManager.GetInventoryTimeline();

                // 3. Sort data so newest (by id) is first (already sorted by id DESC in query)
                // 4. Generate PDF using iTextSharp
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 40, 40, 40, 40))
                using (var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs))
                {
                    doc.Open();

                    // Title
                    var titleFont = iTextSharp.text.FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);
                    var headerFont = iTextSharp.text.FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
                    var rowFont = iTextSharp.text.FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.NORMAL);

                    var title = new iTextSharp.text.Paragraph("INVENTORY TIMELINE REPORT", titleFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER,
                        SpacingAfter = 10f
                    };
                    doc.Add(title);

                    var date = new iTextSharp.text.Paragraph($"Date: {DateTime.Now:yyyy-MM-dd}", rowFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_LEFT,
                        SpacingAfter = 10f
                    };
                    doc.Add(date);

                    // Table (add Date column)
                    var table = new iTextSharp.text.pdf.PdfPTable(6)
                    {
                        WidthPercentage = 100
                    };
                    table.SetWidths(new float[] { 3f, 1f, 1f, 1f, 1f, 2f });

                    // Headers
                    string[] headers = { "Product Name", "Sold", "Added", "Stock", "Price", "Date" };
                    foreach (var h in headers)
                    {
                        var cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(h, headerFont))
                        {
                            BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);
                    }

                    // Rows
                    foreach (var row in data)
                    {
                        table.AddCell(new iTextSharp.text.Phrase(row["product_name"].ToString(), rowFont));
                        table.AddCell(new iTextSharp.text.Phrase(row["sold"].ToString(), rowFont));
                        table.AddCell(new iTextSharp.text.Phrase(row["added"].ToString(), rowFont));
                        table.AddCell(new iTextSharp.text.Phrase(row["stock"].ToString(), rowFont));
                        table.AddCell(new iTextSharp.text.Phrase($"${Convert.ToDouble(row["price"]):F2}", rowFont));
                        table.AddCell(new iTextSharp.text.Phrase(
                            DateTime.TryParse(row["date"]?.ToString(), out var d) ? d.ToString("yyyy-MM-dd HH:mm") : row["date"]?.ToString(),
                            rowFont));
                    }

                    doc.Add(table);
                    doc.Close();
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating PDF report: {ex.Message}");
                return null;
            }
        }


        public static string CreateReceiptPdf(Dictionary<string, Products> shoppingCart, decimal amountPaid)
        {
            // Set file path to Desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"Receipt_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(desktopPath, fileName);

            // Calculate total
            decimal total = 0;
            foreach (var item in shoppingCart.Values)
            {
                total += item.price * item.amount;
            }
            decimal change = amountPaid - total;

            // Create PDF document
            Document doc = new Document();
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // Title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                doc.Add(new Paragraph("Supermarket Receipt", titleFont));
                doc.Add(new Paragraph("Date: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                doc.Add(new Paragraph(" "));

                // Table for products
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 40, 20, 20, 20 });

                table.AddCell("Product");
                table.AddCell("Price");
                table.AddCell("Quantity");
                table.AddCell("Subtotal");

                foreach (var product in shoppingCart.Values)
                {
                    table.AddCell(product.name);
                    table.AddCell('$' + product.price.ToString());
                    table.AddCell(product.amount.ToString());
                    table.AddCell('$' + (product.price * product.amount).ToString());
                }

                doc.Add(table);

                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Total: ${total}"));
                doc.Add(new Paragraph($"Amount Paid: ${amountPaid}"));
                doc.Add(new Paragraph($"Change: ${change}"));
            }
            finally
            {
                doc.Close();
            }

            return filePath;
        }


        public static string CreateSalesReportPdf(DateTime startDate, DateTime endDate)
        {
            // 1. Prepare file path
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"SalesReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf";
            string filePath = Path.Combine(desktopPath, fileName);

            // 2. Fetch data from sales table
            var salesData = new List<Dictionary<string, object>>();
            using (var conn = new System.Data.SQLite.SQLiteConnection($"Data Source={DatabaseManager.DbPath};Version=3;"))
            {
                conn.Open();
                string query = @"SELECT id, barcode, product_name, quantity_sold, unit_price, total_price, sale_date, user_id
                                 FROM sales
                                 WHERE sale_date >= @start AND sale_date <= @end
                                 ORDER BY sale_date ASC";
                using (var cmd = new System.Data.SQLite.SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@start", startDate.ToString("yyyy-MM-dd 00:00:00"));
                    cmd.Parameters.AddWithValue("@end", endDate.ToString("yyyy-MM-dd 23:59:59"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                row[reader.GetName(i)] = reader.GetValue(i);
                            salesData.Add(row);
                        }
                    }
                }
            }

            // 3. Generate PDF using iTextSharp
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var doc = new Document(PageSize.A4, 40, 40, 40, 40))
            using (var writer = PdfWriter.GetInstance(doc, fs))
            {
                doc.Open();

                // Header
                var headerFont = FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.BLUE);
                var subHeaderFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var rowFont = FontFactory.GetFont("Arial", 11, Font.NORMAL);

                var header = new Paragraph("SupermarketManagementSystem", headerFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f
                };
                doc.Add(header);

                var reportTitle = new Paragraph("SALES REPORT", subHeaderFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 5f
                };
                doc.Add(reportTitle);

                var dateRange = new Paragraph($"From: {startDate:yyyy-MM-dd} To: {endDate:yyyy-MM-dd}", rowFont)
                {
                    Alignment = Element.ALIGN_LEFT,
                    SpacingAfter = 5f
                };
                doc.Add(dateRange);

                var generatedOn = new Paragraph($"Report generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", rowFont)
                {
                    Alignment = Element.ALIGN_LEFT,
                    SpacingAfter = 10f
                };
                doc.Add(generatedOn);

                // Table
                PdfPTable table = new PdfPTable(6)
                {
                    WidthPercentage = 100
                };
                table.SetWidths(new float[] { 2f, 3f, 1f, 1f, 1f, 2f });

                string[] headers = { "Barcode", "Product Name", "Qty Sold", "Unit Price", "Total", "Sale Date" };
                foreach (var h in headers)
                {
                    var cell = new PdfPCell(new Phrase(h, subHeaderFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                decimal grandTotal = 0;
                foreach (var row in salesData)
                {
                    table.AddCell(new Phrase(row["barcode"].ToString(), rowFont));
                    table.AddCell(new Phrase(row["product_name"].ToString(), rowFont));
                    table.AddCell(new Phrase(row["quantity_sold"].ToString(), rowFont));
                    table.AddCell(new Phrase($"${Convert.ToDecimal(row["unit_price"]):F2}", rowFont));
                    table.AddCell(new Phrase($"${Convert.ToDecimal(row["total_price"]):F2}", rowFont));
                    table.AddCell(new Phrase(
                        DateTime.TryParse(row["sale_date"]?.ToString(), out var d) ? d.ToString("yyyy-MM-dd HH:mm") : row["sale_date"]?.ToString(),
                        rowFont));
                    grandTotal += Convert.ToDecimal(row["total_price"]);
                }

                doc.Add(table);

                // Grand total
                doc.Add(new Paragraph(" "));
                var totalFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                doc.Add(new Paragraph($"Grand Total: ${grandTotal:F2}", totalFont));

                doc.Close();
            }

            return filePath;
        }


    }
}