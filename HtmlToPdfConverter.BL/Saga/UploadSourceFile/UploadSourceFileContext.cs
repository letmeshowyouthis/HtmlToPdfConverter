using HtmlToPdfConverter.DAL.Models;

namespace HtmlToPdfConverter.BL.Saga.UploadSourceFile
{
    public class UploadSourceFileContext
    {
        public Session Session { get; }
        public byte[] File { get; }

        public UploadSourceFileContext(Session session, byte[] file)
        {
            Session = session;
            File = file;
        }
    }
}
