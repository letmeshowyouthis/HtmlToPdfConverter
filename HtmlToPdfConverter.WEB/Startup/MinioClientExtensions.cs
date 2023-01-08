using Minio;
using Minio.Exceptions;
using Polly;

namespace HtmlToPdfConverter.WEB.Startup
{
    public static class MinioClientExtensions
    {
        /// <summary>
        /// Sets reconnect retry policy.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="retryCount">Maximum retries.</param>
        /// <param name="retryInterval">Delay between retries.</param>
        /// <returns></returns>
        public static MinioClient WithReconnectRetryPolicy(this MinioClient client, int retryCount, TimeSpan retryInterval)
        {
            var policy = RetryPolicy.WaitAndRetry(retryCount, retryInterval);
            return client.WithRetryPolicy(policy);
        }
    }

    static class RetryPolicy
    {
        public static AsyncPolicy WaitAndRetry(
            int maxRetries,
            TimeSpan retryInterval) =>
            CreatePolicyBuilder()
                .WaitAndRetryAsync(
                    maxRetries,
                    i => retryInterval);
        
        private static PolicyBuilder CreatePolicyBuilder()
        {
            return Policy
                .Handle<ConnectionException>()
                .Or<InternalClientException>(ex => ex.Message.StartsWith("Unsuccessful response from server"));
        }

        public static MinioClient WithRetryPolicy(this MinioClient client, AsyncPolicy policy) =>
            client.WithRetryPolicy(policy.AsRetryDelegate());

        private static RetryPolicyHandlingDelegate AsRetryDelegate(this AsyncPolicy policy) =>
            policy == null
                ? null
                : async executeCallback => await policy.ExecuteAsync(executeCallback);
    }
}
