using System.Linq.Expressions;
using Hangfire;

namespace HtmlToPdfConverter.BL.Services
{
    /// <summary>
    /// Hangfire-based implementation of <see cref="IJobQueue"/>.
    /// </summary>
    public class HangfireJobQueue : IJobQueue
    {
        public Task<string> QueueJobAsync(Expression<Action> job)
        {
            var jobId = BackgroundJob.Enqueue(job);
            return Task.FromResult(jobId);
        }

        public Task DequeueJobAsync(string jobId)
        {
            BackgroundJob.Delete(jobId);
            return Task.CompletedTask;
        }
    }
}
