using System.Threading.Tasks;
using HtmlToPdfConverter.BL.Saga.UploadSourceFile;
using HtmlToPdfConverter.BL.Saga.UploadSourceFile.Steps;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Moq;
using NUnit.Framework;

namespace HtmlToPdfConverter.BL.Tests.Saga.UploadSoureFile.Steps
{
    internal class FileUploadStepTests
    {
        [Test]
        public async Task ExecuteAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new UploadSourceFileContext(session, default);

            var fileManagerMock = new Mock<IFileManager>();
            var step = new FileUploadStep(fileManagerMock.Object);

            await step.ExecuteAsync(context);

            fileManagerMock.Verify(x => x.PutAsync(context.File, session.SourceFileUniqueFullName), Times.Once);
        }

        [Test]
        public async Task RollbackAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new UploadSourceFileContext(session, default);

            var fileManagerMock = new Mock<IFileManager>();
            var step = new FileUploadStep(fileManagerMock.Object);

            await step.RollbackAsync(context);

            fileManagerMock.Verify(x => x.DeleteAsync(session.SourceFileUniqueFullName), Times.Once);
        }
    }
}
