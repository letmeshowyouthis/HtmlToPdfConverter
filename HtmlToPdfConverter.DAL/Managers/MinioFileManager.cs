using Minio;

namespace HtmlToPdfConverter.DAL.Managers
{
    /// <summary>
    /// MinIO-based implementation of <see cref="IFileManager"/>.
    /// </summary>
    public class MinioFileManager : IFileManager
    {
        private const string BucketName = "html-to-pdf";

        private readonly MinioClient _client;

        public MinioFileManager(MinioClient client)
        {
            _client = client;
        }

        public async Task PutAsync(byte[] file, string fileId)
        {
            // Make a bucket on the server, if not already present.
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(BucketName);
            var found = await _client.BucketExistsAsync(bucketExistsArgs);
            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(BucketName);
                await _client.MakeBucketAsync(makeBucketArgs);
            }

            // Upload a file to bucket.
            var args = new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObjectSize(file.Length)
                .WithObject(fileId)
                .WithStreamData(new MemoryStream(file));

            await _client.PutObjectAsync(args);
        }

        public async Task DeleteAsync(string fileId)
        {
            var args = new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileId);

            await _client.RemoveObjectAsync(args);
        }

        public async Task<byte[]> FindAsync(string fileId)
        {
            await using var ms = new MemoryStream();

            void Callback(Stream stream)
            {
                stream.CopyTo(ms);
                stream.Dispose();
            }
            
            var args = new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileId)
                .WithCallbackStream(Callback);

            await _client.GetObjectAsync(args);

            return ms.ToArray();
        }
    }
}
