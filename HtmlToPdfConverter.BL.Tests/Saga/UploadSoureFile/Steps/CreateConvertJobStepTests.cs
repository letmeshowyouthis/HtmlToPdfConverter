using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf;
using HtmlToPdfConverter.BL.Saga.UploadSourceFile;
using HtmlToPdfConverter.BL.Saga.UploadSourceFile.Steps;
using HtmlToPdfConverter.BL.Services;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Infra.EasySaga;
using Moq;
using NUnit.Framework;

namespace HtmlToPdfConverter.BL.Tests.Saga.UploadSoureFile.Steps
{
    internal class CreateConvertJobStepTests
    {
        [Test]
        public async Task ExecuteAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new UploadSourceFileContext(session, new byte[] { 1, 2, 3 });
            
            var jobQueueMock = new Mock<IJobQueue>();
            var sessionManagerMock = new Mock<ISessionManager>();
            var fileManagerMock = new Mock<IFileManager>();
            var convertToPdfSagaMock = new Mock<ISagaScenario<ConvertToPdfContext>>();

            var step = new CreateConvertJobStep(
                jobQueueMock.Object, sessionManagerMock.Object, fileManagerMock.Object, convertToPdfSagaMock.Object);
            
            await step.ExecuteAsync(context);

            jobQueueMock.Verify(x => x.QueueJobAsync(It.IsAny<Expression<Action>>()), Times.Once);
        }

        [Test]
        public async Task RollbackAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new UploadSourceFileContext(session, default);

            var jobQueueMock = new Mock<IJobQueue>();
            var sessionManagerMock = new Mock<ISessionManager>();
            var fileManagerMock = new Mock<IFileManager>();
            var convertToPdfSagaMock = new Mock<ISagaScenario<ConvertToPdfContext>>();

            var step = new CreateConvertJobStep(
                jobQueueMock.Object, sessionManagerMock.Object, fileManagerMock.Object, convertToPdfSagaMock.Object);

            await step.RollbackAsync(context);

            jobQueueMock.Verify(x => x.DequeueJobAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ConvertToPdfJobIsSuccess()
        {
            var session = new Session("fileName");

            var jobQueueMock = new Mock<IJobQueue>();
            var sessionManagerMock = new Mock<ISessionManager>();
            var fileManagerMock = new Mock<IFileManager>();
            var convertToPdfSagaMock = new Mock<ISagaScenario<ConvertToPdfContext>>();

            var step = new CreateConvertJobStep(
                jobQueueMock.Object, sessionManagerMock.Object, fileManagerMock.Object, convertToPdfSagaMock.Object);

            sessionManagerMock.Setup(x => x.FindAsync(session.Id))
                .ReturnsAsync(session);
            fileManagerMock.Setup(x => x.FindAsync(session.SourceFileUniqueFullName))
                .ReturnsAsync(new byte[] { 1, 2, 3 });

            await step.ConvertToPdfJob(session.Id);

            sessionManagerMock.Verify(x => x.FindAsync(session.Id), Times.Once);
            fileManagerMock.Verify(x => x.FindAsync(session.SourceFileUniqueFullName), Times.Once);
            convertToPdfSagaMock.Verify(x => x.ExecuteAllAsync(It.IsAny<ConvertToPdfContext>()), Times.Once);
        }
    }
}
