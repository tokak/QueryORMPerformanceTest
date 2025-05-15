using Dapper;
using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

class Program
{
    // Veritabanı bağlantı 
    private static readonly string _connectionString = "Server=DESKTOP-141UAGI;Initial Catalog=TestOrmDb;Integrated Security=true;TrustServerCertificate=True;";
    static void Main(string[] args)
    {
        // EF Core, Dapper ve ADO.NET ile veritabanından okuma işlemleri yapılır
        var efCoreResult = TestListEntityFrameworkCore();
        var dapperResult = TestListDapper();
        var adoResult = TestListAdoNet();

        // Sonuçlar (kayıt sayısı ve süre) konsola yazdırılır
        Console.WriteLine($"EF Core - {efCoreResult.Item1} ürün {efCoreResult.Item2} ms içinde alındı.");
        Console.WriteLine($"Dapper - {dapperResult.Item1} ürün {dapperResult.Item2} ms içinde alındı.");
        Console.WriteLine($"ADO.NET - {adoResult.Item1} ürün {adoResult.Item2} ms içinde alındı.");
    }

   
    // Entity Framework Core kullanarak Products tablosundaki tüm kayıtları alır.    
    // Toplam kayıt sayısı ve işlemin milisaniye cinsinden süresi döndürülür
    public static (int, long) TestListEntityFrameworkCore()
    {
        using (var context = new AppDbContext())
        {
            var stopwatch = Stopwatch.StartNew(); // Zaman ölçümü başlatılır
            var products = context.Products.ToList(); 
            stopwatch.Stop(); // Zaman ölçümü durdurulur

            return (products.Count, stopwatch.ElapsedMilliseconds); 
        }
    }


    // Dapper kullanarak Products tablosundaki tüm kayıtları alır.   
    // Toplam kayıt sayısı ve işlemin milisaniye cinsinden süresi döndürülür
    public static (int, long) TestListDapper()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open(); 
            var stopwatch = Stopwatch.StartNew(); // Zaman ölçümü başlatılır

            var products = connection.Query<Product>("SELECT * FROM Products").ToList(); 

            stopwatch.Stop(); // Zaman ölçümü durdurulur
            return (products.Count, stopwatch.ElapsedMilliseconds); 
        }
    }


    // ADO.NET kullanarak Products tablosundaki toplam kayıt sayısını alır.
    //Kayıt sayısı ve işlemin milisaniye cinsinden süresi döndürülür
    public static (int count, long elapsedMilliseconds) TestListAdoNet()
    {
        int count = 0;
        var stopwatch = Stopwatch.StartNew(); // Zaman ölçümü başlatılır

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open(); 
            string query = "SELECT COUNT(*) FROM Products"; 

            using (var command = new SqlCommand(query, connection))
            {
                count = (int)command.ExecuteScalar(); 
            }
        }
        stopwatch.Stop(); // Zaman ölçümü durdurulur
        return (count, stopwatch.ElapsedMilliseconds); 
    }
}
