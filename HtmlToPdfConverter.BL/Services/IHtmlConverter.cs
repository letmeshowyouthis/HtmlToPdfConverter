namespace HtmlToPdfConverter.BL.Services
{
    /// <summary>
    /// Converts HTML files to PDF.
    /// </summary>
    public interface IHtmlConverter
    {
        /// <summary>
        /// Converts the specified HTML to PDF.
        /// </summary>
        /// <param name="htmlFile">HTML to convert.</param>
        /// <returns>HTML converted to PDF.</returns>
        public Task<byte[]> ToPdf(byte[] htmlFile);
    }
}
