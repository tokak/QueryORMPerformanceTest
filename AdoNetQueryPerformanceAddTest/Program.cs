using DataAccessLayer.Entity;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        var addCount = 1000000;

        // ADO.NET ile kayıt ekleme
        var adoInsertResult = InsertWithAdoNet(addCount);
        Console.WriteLine($"ADO.NET - {adoInsertResult} ms sürede {addCount} kayıt eklendi.");
    }

    public static long InsertWithAdoNet(int count)
    {
        var time = 0;
        var products = new List<Product>();
        for (int i = 1; i <= count; i++)
        {
            products.Add(new Product
            {
                ProductName = $"Test Ürün {i}",
                ProductStock = "20",
                ProductPrice = "20"
            });
        }
        // DataTable oluştur
        var table = new DataTable();
        table.Columns.Add("ProductName", typeof(string));
        table.Columns.Add("ProductStock", typeof(string));
        table.Columns.Add("ProductPrice", typeof(string));

        foreach (var product in products)
        {
            table.Rows.Add(product.ProductName, product.ProductStock, product.ProductPrice);
        }

        long recordingPeriodMs = 0; //kayıt edilen zamanı hesaplama
        using (var connection = new SqlConnection("Server=DESKTOP-141UAGI;Initial Catalog=TestOrmDb;Integrated Security=true;TrustServerCertificate=True;"))
        {
            connection.Open();
            var stopwatch = Stopwatch.StartNew();
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = "Products";
                bulkCopy.ColumnMappings.Add("ProductName", "ProductName");
                bulkCopy.ColumnMappings.Add("ProductStock", "ProductStock");
                bulkCopy.ColumnMappings.Add("ProductPrice", "ProductPrice");
                bulkCopy.WriteToServer(table);
                stopwatch.Stop();
                recordingPeriodMs = stopwatch.ElapsedMilliseconds;
            }
        }
        return recordingPeriodMs;
    }

}