using System.Linq.Expressions;

namespace HtmlToPdfConverter.BL.Services;

/// <summary>
/// Conversion jobs queue interface.
/// </summary>
public interface IJobQueue
{
    /// <summary>
    /// Asynchronously queues a conversion to PDF job.
    /// </summary>
    /// <param name="job">Job to call.</param>
    /// <returns>Job's unique identifier.</returns>
    Task<string> QueueJobAsync(Expression<Action> job);
    /// <summary>
    /// Asynchronously dequeues a conversion to PDF job.
    /// </summary>
    /// <param name="jobId">Job unique identifier.</param>
    Task DequeueJobAsync(string jobId);
}