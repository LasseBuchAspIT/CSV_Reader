using System.Globalization;

namespace CSV_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                })
                .Build();
            CultureInfo.CurrentCulture = new("da-DK");
            Thread.CurrentThread.CurrentCulture = new("da-DK");
            host.Run();
        }
    }
}