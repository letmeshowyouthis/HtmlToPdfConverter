using HtmlToPdfConverter.BL.Services;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Infra.EasySaga;

namespace HtmlToPdfConverter.BL.Saga.ConvertToPdf.Steps
{
    public class ConvertToPdfStep : ISagaTransactionAsync<ConvertToPdfContext>
    {
        private readonly IHtmlConverter _htmlConverter;
        private readonly ISessionManager _sessionManager;

        public ConvertToPdfStep(IHtmlConverter htmlConverter, ISessionManager sessionManager)
        {
            _htmlConverter = htmlConverter;
            _sessionManager = sessionManager;
        }

        public async Task ExecuteAsync(ConvertToPdfContext context)
        {
            try
            {
                context.ConvertedFile = await _htmlConverter.ToPdf(context.SourceFile);
            }
            catch
            {
                await SetIncorrectFileStatus(context);
                throw;
            }
        }
        
        private async Task SetIncorrectFileStatus(ConvertToPdfContext context)
        {
            context.Session.Status = SessionStatus.IncorrectSourceFile;
            await _sessionManager.AddOrUpdateAsync(context.Session);
        }

        public Task RollbackAsync(ConvertToPdfContext context)
        {
            return Task.CompletedTask;
        }
    }
}
