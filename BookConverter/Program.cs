using System;
using System.Linq;
using System.IO;
using ImageMagick;
using System.Text.RegularExpressions;

namespace BookConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"D:\Examples\BookDownloader\BookDownloader\bin\Debug\netcoreapp2.0\book_21556237";
            var outputPath = Path.Combine(path, "book.pdf");

            Console.WriteLine("Started");
            
            var files = new DirectoryInfo(path).GetFiles("page_*").OrderBy(f => {
                var pageIndex = new Regex(@"(page_|\.|gif|jpg)", RegexOptions.Multiline).Replace(f.Name, "");
                return int.Parse(pageIndex);
            });

            Console.WriteLine($"Found pages: {files.Count()}");
            Console.WriteLine($"Converting...");

            using (var collection = new MagickImageCollection())
            {
                foreach (var file in files)
                {
                    collection.Add(new MagickImage(Path.Combine(path, file.Name)));
                }

                collection.Write(outputPath);
            }

            Console.WriteLine($"Done! {outputPath}");
            Console.ReadLine();
        }
    }
}
