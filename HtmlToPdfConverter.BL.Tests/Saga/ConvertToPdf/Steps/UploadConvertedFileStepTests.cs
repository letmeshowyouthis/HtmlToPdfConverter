using System.Threading.Tasks;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf;
using HtmlToPdfConverter.BL.Saga.ConvertToPdf.Steps;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.DAL.Models;
using Moq;
using NUnit.Framework;

namespace HtmlToPdfConverter.BL.Tests.Saga.ConvertToPdf.Steps
{
    internal class UploadConvertedFileStepTests
    {
        [Test]
        public async Task ExecuteAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new ConvertToPdfContext(session, default)
            {
                ConvertedFile = new byte[] { 1, 2, 3 }
            };

            var fileManagerMock = new Mock<IFileManager>();

            var step = new UploadConvertedFileStep(fileManagerMock.Object);

            await step.ExecuteAsync(context);

            fileManagerMock.Verify(x => x.PutAsync(context.ConvertedFile, session.ConvertedFileUniqueFullName), Times.Once);
        }

        [Test]
        public async Task RollbackAsyncIsSuccess()
        {
            var session = new Session("fileName");
            var context = new ConvertToPdfContext(session, default);

            var fileManagerMock = new Mock<IFileManager>();

            var step = new UploadConvertedFileStep(fileManagerMock.Object);

            await step.RollbackAsync(context);

            fileManagerMock.Verify(x => x.DeleteAsync(session.ConvertedFileUniqueFullName), Times.Once);
        }
    }
}
