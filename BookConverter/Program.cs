using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using BookConverter.Custom;

namespace BookConverter
{
	class Program
    {
		const string BookId = "31991883";

		static void Main(string[] args)
        {
            var path = $@"D:\Examples\BookDownloader\BookDownloader\bin\Debug\netcoreapp2.0\book_{BookId}";
            var outputPath = Path.Combine(path, "book.pdf");

			WriteLog("Started");
            
            var files = new DirectoryInfo(path).GetFiles("page_*").OrderBy(file => GetIndexFromFileName(file.Name));

            WriteLog($"Found pages: {files.Count()}");
            WriteLog($"Converting...");

			ImageSource.ImageSourceImpl = new CoreImageSource();

			using (var doc = new PdfDocument())
			{
				foreach (var file in files.Take(10))
				{
					var imgPath = Path.Combine(path, file.Name);
					var img = XImage.FromStream(() => new MemoryStream(File.ReadAllBytes(imgPath)));

					doc.Pages.Add(new PdfPage
					{
						Width = XUnit.FromPoint(img.PointWidth),
						Height = XUnit.FromPoint(img.PointHeight)
					});

					XGraphics
						.FromPdfPage(doc.Pages[doc.Pages.Count - 1])
						.DrawImage(img, 0, 0);

					WriteLog($"{file.Name} added");
				}
				doc.Save(outputPath);
			}

            WriteLog($"Done! {outputPath}");
            Console.ReadLine();
        }

		static int GetIndexFromFileName(string fileName)
			=> int.Parse(new Regex(@"(page_|\.|gif|jpg)", RegexOptions.Multiline).Replace(fileName, ""));

		static void WriteLog(string message) => Console.WriteLine(message);

	}
}
