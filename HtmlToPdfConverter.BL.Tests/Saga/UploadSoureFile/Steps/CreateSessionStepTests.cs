using HtmlToPdfConverter.BL.Saga.UploadSourceFile;
using HtmlToPdfConverter.BL.Saga.UploadSourceFile.Steps;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace HtmlToPdfConverter.BL.Tests.Saga.UploadSoureFile.Steps
{
    public class CreateSessionStepTests
    {
        [Test]
        public async Task ExecuteAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new UploadSourceFileContext(session, default);

            var sessionManagerMock = new Mock<ISessionManager>();
            var step = new CreateSessionStep(sessionManagerMock.Object);

            await step.ExecuteAsync(context);

            sessionManagerMock.Verify(x => x.AddOrUpdateAsync(session), Times.Once);
        }

        [Test]
        public async Task RollbackAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new UploadSourceFileContext(session, default);

            var sessionManagerMock = new Mock<ISessionManager>();
            var step = new CreateSessionStep(sessionManagerMock.Object);

            await step.RollbackAsync(context);

            sessionManagerMock.Verify(x => x.RemoveAsync(session), Times.Once);
        }
    }
}
