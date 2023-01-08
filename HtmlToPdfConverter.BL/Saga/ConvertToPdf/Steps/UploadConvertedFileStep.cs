using HtmlToPdfConverter.DAL.Managers;
using Infra.EasySaga;

namespace HtmlToPdfConverter.BL.Saga.ConvertToPdf.Steps
{
    public class UploadConvertedFileStep : ISagaTransactionAsync<ConvertToPdfContext>
    {
        private readonly IFileManager _fileManager;

        public UploadConvertedFileStep(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public async Task ExecuteAsync(ConvertToPdfContext context)
        {
            await _fileManager.PutAsync(context.ConvertedFile, context.Session.ConvertedFileUniqueFullName);
        }

        public async Task RollbackAsync(ConvertToPdfContext context)
        {
            await _fileManager.DeleteAsync(context.Session.ConvertedFileUniqueFullName);
        }
    }
}
