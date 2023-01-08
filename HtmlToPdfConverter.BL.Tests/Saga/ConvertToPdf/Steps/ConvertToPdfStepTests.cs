using System.Threading.Tasks;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf.Steps;
using HtmlToPdfConverter.BL.Services;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Moq;
using NUnit.Framework;

namespace HtmlToPdfConverter.BL.Tests.Saga.ConvertToPdf.Steps
{
    internal class ConvertToPdfStepTests
    {
        [Test]
        public async Task ExecuteAsyncIsSuccess_CorrectFile()
        {
            var sourceFile = new byte[] { 1, 2, 3 };
            var session = new Session("fileName");
            var context = new ConvertToPdfContext(session, sourceFile);

            var htmlConverterMock = new Mock<IHtmlConverter>(MockBehavior.Loose);
            var sessionManagerMock = new Mock<ISessionManager>();

            var convertedFile = new byte[] { 2, 3, 4, 5 };
            htmlConverterMock
                .Setup(x => x.ToPdf(sourceFile))
                .ReturnsAsync(convertedFile);

            var step = new ConvertToPdfStep(htmlConverterMock.Object, sessionManagerMock.Object);

            await step.ExecuteAsync(context);

            Assert.AreEqual(convertedFile, context.ConvertedFile);
        }
        
        [Test]
        public async Task ExecuteAsyncIsSuccess_IncorrectFile()
        {
            var sourceFile = new byte[] { 1, 2, 3 };
            var session = new Session("fileName");
            var context = new ConvertToPdfContext(session, sourceFile);

            var htmlConverterMock = new Mock<IHtmlConverter>(MockBehavior.Strict);
            var sessionManagerMock = new Mock<ISessionManager>();

            var step = new ConvertToPdfStep(htmlConverterMock.Object, sessionManagerMock.Object);

            try
            {
                await step.ExecuteAsync(context);
            }
            catch
            {
                // ignored
            }

            Assert.AreEqual(null, context.ConvertedFile);
            Assert.AreEqual(SessionStatus.IncorrectSourceFile, session.Status);
            sessionManagerMock.Verify(x => x.AddOrUpdateAsync(session));
        }

        [Test]
        public async Task RollbackIsSuccess()
        {
            var sourceFile = new byte[] { 1, 2, 3 };
            var session = new Session("fileName");
            var context = new ConvertToPdfContext(session, sourceFile);

            var htmlConverterMock = new Mock<IHtmlConverter>();
            var sessionManagerMock = new Mock<ISessionManager>();

            var step = new ConvertToPdfStep(htmlConverterMock.Object, sessionManagerMock.Object);
            
            Assert.DoesNotThrowAsync(async () => await step.RollbackAsync(context));
        }
    }
}
