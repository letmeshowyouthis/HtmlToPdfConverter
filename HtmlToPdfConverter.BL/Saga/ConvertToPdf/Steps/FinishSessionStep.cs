using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Infra.EasySaga;

namespace HtmlToPdfConverter.BL.Saga.ConvertToPdf.Steps
{
    public class FinishSessionStep : ISagaTransactionAsync<ConvertToPdfContext>
    {
        private readonly ISessionManager _sessionManager;

        public FinishSessionStep(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        private SessionStatus _originalStatus;
        public async Task ExecuteAsync(ConvertToPdfContext context)
        {
            _originalStatus = context.Session.Status;

            Finish(context.Session);
            await _sessionManager.AddOrUpdateAsync(context.Session);
        }

        public async Task RollbackAsync(ConvertToPdfContext context)
        {
            UnFinish(context.Session);
            await _sessionManager.AddOrUpdateAsync(context.Session);
        }
        
        private void Finish(Session session)
        {
            session.FinishedUtc = DateTime.UtcNow;
            session.Status = SessionStatus.StoredConvertedFile;
        }

        private void UnFinish(Session session)
        {
            session.FinishedUtc = null;
            session.Status = _originalStatus;
        }
    }
}
