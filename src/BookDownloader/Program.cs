using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BookDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bookId = "";
            var userToken = ""; // SID from cookies
            var resolution = "w1920"; // w1280


            var outputPath = Directory.GetCurrentDirectory();

            var cts = new CancellationTokenSource();
            var downloader = new Downloader(userToken, outputPath, Console.WriteLine);

            Console.WriteLine("Started");
            Console.WriteLine("Downloading...");

            try
            {
                await downloader.DownloadBookAsync(bookId, resolution, cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
