using HtmlToPdfConverter.BL.Saga.UploadSourceFile;
using HtmlToPdfConverter.DAL.Models;
using Infra.EasySaga;

namespace HtmlToPdfConverter.BL.Services
{
    /// <summary>
    /// PuppeteerSharp-based implementation of <see cref="IConverterService"/>. 
    /// </summary>
    public class ConverterService : IConverterService
    {
        private readonly ISagaScenario<UploadSourceFileContext> _sagaUploadSourceFile;

        public ConverterService(ISagaScenario<UploadSourceFileContext> sagaUploadSourceFile)
        {
            _sagaUploadSourceFile = sagaUploadSourceFile;
        }

        public async Task<Guid> ConvertAsync(byte[] file, string fileName)
        {
            var session = new Session(fileName);

            var context = new UploadSourceFileContext(session, file);
            await _sagaUploadSourceFile.ExecuteAllAsync(context);

            return session.Id;
        }
    }
}
