using System.Threading.Tasks;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf.Steps;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Moq;
using NUnit.Framework;

namespace HtmlToPdfConverter.BL.Tests.Saga.ConvertToPdf.Steps
{
    internal class FinishSessionStepTests
    {
        [Test]
        public async Task ExecuteAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new ConvertToPdfContext(session, default);
            
            var sessionManagerMock = new Mock<ISessionManager>();

            var step = new FinishSessionStep(sessionManagerMock.Object);

            await step.ExecuteAsync(context);

            Assert.AreNotEqual(null, session.FinishedUtc);
            Assert.AreEqual(SessionStatus.StoredConvertedFile, session.Status);
            sessionManagerMock.Verify(x => x.AddOrUpdateAsync(session), Times.Once);
        }

        [Test]
        public async Task RollbackAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new ConvertToPdfContext(session, default);

            var sessionManagerMock = new Mock<ISessionManager>();

            var step = new FinishSessionStep(sessionManagerMock.Object);
            
            await step.RollbackAsync(context);

            Assert.AreEqual(SessionStatus.New, session.Status);
            Assert.AreEqual(null, session.FinishedUtc);
            sessionManagerMock.Verify(x => x.AddOrUpdateAsync(session));
        }
    }
}
