using System;
using System.IO;
using System.Threading;

namespace BookDownloader
{
    class Program
    {
        static void Main(string[] args)
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
                downloader.DownloadBookAsync(bookId, resolution, cts.Token).Wait();
            }
            catch (Exception e)
            {
                cts.Cancel();
                Console.WriteLine(e);
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
