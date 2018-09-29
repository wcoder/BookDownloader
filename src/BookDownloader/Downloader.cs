using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BookDownloader
{
    class Downloader
    {
        const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";

        readonly string _userToken;
        readonly string _rootOutput;
        readonly Action<string> _writeToLog;
        readonly HttpClient _httpClient;

        public Downloader(string userToken, string rootOutput, Action<string> writeToLog)
        {
            _userToken = userToken;
            _rootOutput = rootOutput;
            _writeToLog = writeToLog;

            _httpClient = CreateHttpClient();
        }

        public async Task DownloadBookAsync(string bookId, string resolution, CancellationToken ct)
        {
            var outputFolderPath = Directory.CreateDirectory(Path.Combine(_rootOutput, $"book_{bookId}")).FullName;

            var pageIndex = 0;

            while (true)
            {
                var ext = pageIndex == 0 ? "jpg" : "gif";
                var url = $"https://pro.litres.ru/pages/read_book_online/?file={bookId}&page={pageIndex}&rt={resolution}&ft={ext}";

                _writeToLog(url);

                var stream = await DownloadAsync(url, ct);

                var outputFilePath = Path.Combine(outputFolderPath, $"page_{pageIndex}.{ext}");

                await SaveToFileAsync(outputFilePath, stream, ct);

                pageIndex++;
            }
        }

        async Task<Stream> DownloadAsync(string url, CancellationToken ct)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Add("Cookie", $"SID={_userToken}");

                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode == false)
                {
                    throw new Exception("STOP");
                }

                return await response.Content.ReadAsStreamAsync();
            }
        }

        async Task SaveToFileAsync(string path, Stream stream, CancellationToken ct)
        {
            using (var file = File.Create(path))
            {
                await stream.CopyToAsync(file);
            }
        }

        HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            return httpClient;
        }
    }
}
