using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Console;
using NSwagProject.Services;

namespace NSwagProject.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var baseUrl = ConfigurationManager.AppSettings["MathApiUrl"];

            WriteLine("Enter first number:");
            var a = int.Parse(ReadLine());

            WriteLine("Enter second number:");
            var b = int.Parse(ReadLine());

            using (var httpClient = new HttpClient())
            {
                var client = new MathApiClient(baseUrl, httpClient);

                var result = await client
                    .Addition_GetSumAsync(a, b);

                WriteLine($"\n{a} + {b} = {result.Sum}");
            }

            ReadKey();
        }
    }
}
