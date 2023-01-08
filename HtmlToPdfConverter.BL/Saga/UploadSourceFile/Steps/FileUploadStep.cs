using HtmlToPdfConverter.DAL.Managers;
using Infra.EasySaga;

namespace HtmlToPdfConverter.BL.Saga.UploadSourceFile.Steps
{
    public class FileUploadStep : ISagaTransactionAsync<UploadSourceFileContext>
    {
        private readonly IFileManager _fileManager;

        public FileUploadStep(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public async Task ExecuteAsync(UploadSourceFileContext context)
        {
            await _fileManager.PutAsync(context.File, context.Session.SourceFileUniqueFullName);
        }

        public async Task RollbackAsync(UploadSourceFileContext context)
        {
            await _fileManager.DeleteAsync(context.Session.SourceFileUniqueFullName);
        }
    }
}
