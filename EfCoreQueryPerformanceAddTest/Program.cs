using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using System.Diagnostics;
class Program
{
    static void Main(string[] args)
    {
        // Kayıt etmek istediğiniz ürün adedini belirliyoruz
        var addCount = 1000000;

        // Entity Framework Core ile ürünleri veritabanına ekleme işlemi 
        var efCoreInsertResult = InsertWithEntityFrameworkCore(addCount);

        // Ekleme işlemi sonunda toplam süre milisaniye cinsinden ekrana yazdırma
        Console.WriteLine($"EF Core - {efCoreInsertResult} ms sürede {addCount} kayıt eklendi.");
    }

    // Belirtilen sayıda ürünü EF Core kullanarak veritabanına ekler
    public static long InsertWithEntityFrameworkCore(int count)
    {       
        using (var context = new AppDbContext())
        {
            // Zaman ölçümü başlatılıyor
            var stopwatch = Stopwatch.StartNew();
            // Belirtilen sayıda ürün oluşturulup veritabanı bağlamına ekleniyor
            for (int i = 1; i <= count; i++)
            {
                context.Products.Add(new Product
                {
                    ProductName = $"Test Ürün {i}", 
                    ProductPrice = "20",             
                    ProductStock = "20",             
                });
            }            
            context.SaveChanges();
            // Zaman ölçümü durduruluyor
            stopwatch.Stop();

            // Toplam geçen süre milisaniye cinsinden döndürme
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
