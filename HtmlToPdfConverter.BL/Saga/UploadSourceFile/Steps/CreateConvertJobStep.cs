using HtmlToPdfConverter.BL.Saga.ConvertToPdf;
using HtmlToPdfConverter.BL.Services;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Infra.EasySaga;

namespace HtmlToPdfConverter.BL.Saga.UploadSourceFile.Steps
{
    public class CreateConvertJobStep : ISagaTransactionAsync<UploadSourceFileContext>
    {
        private readonly IJobQueue _jobQueue;
        private readonly ISessionManager _sessionManager;
        private readonly IFileManager _fileManager;
        private readonly ISagaScenario<ConvertToPdfContext> _sagaConvertToPdf;

        private string _jobId;

        public CreateConvertJobStep(IJobQueue jobQueue, ISessionManager sessionManager, IFileManager fileManager, 
            ISagaScenario<ConvertToPdfContext> sagaConvertToPdf)
        {
            _jobQueue = jobQueue;
            _sessionManager = sessionManager;
            _fileManager = fileManager;
            _sagaConvertToPdf = sagaConvertToPdf;
        }

        public async Task ExecuteAsync(UploadSourceFileContext context)
        {
            _jobId = await _jobQueue.QueueJobAsync(() => ConvertToPdfJob(context.Session.Id));
        }

        public async Task RollbackAsync(UploadSourceFileContext context)
        {
            await _jobQueue.DequeueJobAsync(_jobId);
        }

        /// <summary>
        /// Reentrant method that asynchronously converts the source file of the specified session and stores it.
        /// Must be <see langword="public"/> for Hangfire to call.
        /// </summary>
        /// <param name="sessionId">Session identifier.</param>
        public async Task ConvertToPdfJob(Guid sessionId)
        {
            var session = await _sessionManager.FindAsync(sessionId);
            if (session == null 
                || session.Status == SessionStatus.IncorrectSourceFile 
                || session.Status == SessionStatus.StoredConvertedFile) 
                return;

            var sourceFile = await _fileManager.FindAsync(session.SourceFileUniqueFullName);
            if (sourceFile == null) return;

            var context = new ConvertToPdfContext(session, sourceFile); 
            await _sagaConvertToPdf.ExecuteAllAsync(context);
        }
    }
}
