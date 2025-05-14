using Microsoft.Data.SqlClient;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        var addCount = 100000;

        // ADO.NET ile kayıt ekleme
        var adoInsertResult = InsertWithAdoNet(addCount);
        Console.WriteLine($"ADO.NET - {adoInsertResult} ms sürede {addCount} kayıt eklendi.");
    }
    public static long InsertWithAdoNet(int count)
    {
        // SQL Server bağlantısı oluşturuluyor
        using (var connection = new SqlConnection("Server=DESKTOP-141UAGI;Initial Catalog=TestOrmDb;Integrated Security=true;TrustServerCertificate=True;"))
        {
            connection.Open();
            var stopwatch = Stopwatch.StartNew();

            // SQL sorgusu: parametreli ekleme
            string sql = "INSERT INTO Products (ProductName, ProductPrice, ProdoctStock) VALUES (@ProductName, @ProductPrice, @ProdoctStock)";

            // Her kayıt için ayrı SqlCommand kullanılıyor
            for (int i = 1; i <= count; i++)
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    // Parametreler ekleniyor
                    command.Parameters.AddWithValue("@ProductName", $"Test Ürün {i}");
                    command.Parameters.AddWithValue("@ProductPrice", "20");
                    command.Parameters.AddWithValue("@ProdoctStock", "20");
                    // Komut çalıştırılıyor
                    command.ExecuteNonQuery();
                }
            }
            stopwatch.Stop();
            // Süre milisaniye olarak döndürülüyor
            return stopwatch.ElapsedMilliseconds;
        }
    }
}