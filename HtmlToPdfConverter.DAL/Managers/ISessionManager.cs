using HtmlToPdfConverter.DAL.Models;

namespace HtmlToPdfConverter.DAL.Managers;

/// <summary>
/// Session manager.
/// </summary>
public interface ISessionManager
{
    /// <summary>
    /// Finds session with the specified identifier.
    /// </summary>
    /// <param name="id">Session's identifier.</param>
    /// <returns>Found session.</returns>
    Task<Session?> FindAsync(Guid id);

    /// <summary>
    /// Updates the specified session or adds a new one.
    /// </summary>
    /// <param name="session">Session to update or add.</param>
    /// <returns><see langword="true" /> if session was added or updated; <see langword="false" /> otherwise.</returns>
    Task<bool> AddOrUpdateAsync(Session session);

    /// <summary>
    /// Removes the specified session.
    /// </summary>
    /// <param name="session">Session to remove.</param>
    Task RemoveAsync(Session session);
}