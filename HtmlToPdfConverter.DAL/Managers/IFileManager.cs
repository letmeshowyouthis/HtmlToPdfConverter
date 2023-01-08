namespace HtmlToPdfConverter.DAL.Managers
{
    /// <summary>
    /// File manager interface.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Asynchronously stores the specified file.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="fileId">File identifier.</param>
        Task PutAsync(byte[] file, string fileId);

        /// <summary>
        /// Asynchronously deletes file with the specified identifier.
        /// </summary>
        /// <param name="fileId">File identifier</param>
        Task DeleteAsync(string fileId);

        /// <summary>
        /// Asynchronously finds file with the specified name.
        /// </summary>
        /// <param name="fileId">File identifier.</param>
        /// <returns>File.</returns>
        Task<byte[]?> FindAsync(string fileId);
    }
}
