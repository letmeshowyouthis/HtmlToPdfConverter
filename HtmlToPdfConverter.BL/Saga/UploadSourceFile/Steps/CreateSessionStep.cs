using HtmlToPdfConverter.DAL.Managers;
using Infra.EasySaga;

namespace HtmlToPdfConverter.BL.Saga.UploadSourceFile.Steps
{
    public class CreateSessionStep : ISagaTransactionAsync<UploadSourceFileContext>
    {
        private readonly ISessionManager _sessionManager;

        public CreateSessionStep(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task ExecuteAsync(UploadSourceFileContext context)
        {
            await _sessionManager.AddOrUpdateAsync(context.Session);
        }

        public async Task RollbackAsync(UploadSourceFileContext context)
        {
            await _sessionManager.RemoveAsync(context.Session);
        }
    }
}
