using HtmlToPdfConverter.DAL.Models;

namespace HtmlToPdfConverter.BL.Saga.ConvertToPdf
{
    public class ConvertToPdfContext
    {
        public ConvertToPdfContext(Session session, byte[] sourceFile)
        {
            Session = session;
            SourceFile = sourceFile;
        }

        public Session Session { get; }
        public byte[] SourceFile { get; }

        public byte[] ConvertedFile { get; set; }
    }
}
