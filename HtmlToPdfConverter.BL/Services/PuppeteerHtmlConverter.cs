using System.Text;
using PuppeteerSharp;

namespace HtmlToPdfConverter.BL.Services
{
    /// <summary>
    /// Puppeteer-based implementation of <see cref="IHtmlConverter"/>.
    /// </summary>
    public class PuppeteerHtmlConverter : IHtmlConverter
    {
        private readonly Encoding _fileEncoding = Encoding.UTF8;

        /// <summary>
        /// Converts UTF8 HTML file into a PDF file.
        /// </summary>
        /// <param name="htmlFile">HTML file in UTF8.</param>
        /// <returns>PDF file.</returns>
        public async Task<byte[]> ToPdf(byte[] htmlFile)
        {
            var html = _fileEncoding.GetString(htmlFile);

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, 
                Args = new[]
                {
                    "--no-sandbox" // Allows us to use Puppeteer in a Docker container without specifying a user setup.
                } });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            return await page.PdfDataAsync();
        }
    }
}
