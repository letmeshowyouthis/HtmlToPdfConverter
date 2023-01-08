namespace HtmlToPdfConverter.BL.Services
{
    public interface IConverterService
    {
        /// <summary>
        /// Asynchronously converts the specified file.
        /// </summary>
        /// <param name="file">File to convert.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Created session's identifier.</returns>
        Task<Guid> ConvertAsync(byte[] file, string fileName);
    }
}