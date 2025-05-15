using Dapper;
using DataAccessLayer.Entity;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        var addCount = 1000000;
        // Kayıt ekleme methodu
        var dapperInsertResult = InsertWithDapper(addCount);
        Console.WriteLine($"Dapper - {dapperInsertResult} ms sürede {addCount} kayıt eklendi.");
    }
    public static long InsertWithDapper(int count)
    {
        using (var connection = new SqlConnection("Server=DESKTOP-141UAGI;initial catalog=TestOrmDb;integrated security=true;TrustServerCertificate=True;"))
        {
            connection.Open();

            var products = new List<Product>();
            for (int i = 1; i <= count; i++)
            {
                products.Add(new Product
                {
                    ProductName = $"Test Ürün {i}",
                    ProductPrice = "20",
                    ProductStock = "20"
                });
            }
            string sql = "INSERT INTO Products (ProductName, ProductPrice, ProductStock) VALUES (@ProductName, @ProductPrice, @ProductStock)";
            var stopwatch = Stopwatch.StartNew();
            connection.Execute(sql, products);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}