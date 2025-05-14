using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        var addCount = 100000;
        // Kayıt ekleme methodu
        var dapperInsertResult = InsertWithDapper(addCount);
        Console.WriteLine($"Dapper - {dapperInsertResult} ms sürede {addCount} kayıt eklendi.");
    }
    public static long InsertWithDapper(int count)
    {
        using (var connection = new SqlConnection("Server=DESKTOP-141UAGI;initial catalog=TestOrmDb;integrated security=true;TrustServerCertificate=True;"))
        {
            connection.Open();
            var stopwatch = Stopwatch.StartNew();

            string sql = "INSERT INTO Products (ProductName, ProductPrice, ProdoctStock) VALUES (@ProductName, @ProductPrice, @ProdoctStock)";

            for (int i = 1; i <= count; i++)
            {
                connection.Execute(sql, new
                {
                    ProductName = $"Test Ürün {i}",  
                    ProductPrice = "20",             
                    ProdoctStock = "20"              
                });
            }

            stopwatch.Stop();
            // Toplam süre milisaniye cinsinden döndürme
            return stopwatch.ElapsedMilliseconds;
        }
    }
}