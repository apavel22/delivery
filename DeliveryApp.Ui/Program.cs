namespace DeliveryApp.Ui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                //Накатываем миграции на БД, если есть
                //var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //db.Database.Migrate();
            }
            host.Run();
        }
    
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder => webBuilder.UseStartup<Startup>().UseUrls("http://0.0.0.0:81/"));
    }
}