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
            var userToken = ""; // SID
            var outputPath = Directory.GetCurrentDirectory();

            var cts = new CancellationTokenSource();
            var downloader = new Downloader(userToken, outputPath);

            Console.WriteLine("Started");
            Console.WriteLine("Downloading...");

            try
            {
                downloader.DownloadBookAsync(bookId, cts.Token).Wait();
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
