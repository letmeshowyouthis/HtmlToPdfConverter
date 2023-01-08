using Newtonsoft.Json;

namespace HtmlToPdfConverter.DAL.Models;

/// <summary>
/// File conversion session.
/// </summary>
public class Session
{
    /// <summary>
    /// Creates new instance of <see cref="Session"/>.
    /// </summary>
    /// <param name="sourceFileHumanName">Source file name.</param>
    public Session(string sourceFileHumanName)
    {
        Id = Guid.NewGuid();
        FileDisplayName = Path.GetFileNameWithoutExtension(sourceFileHumanName);
        StartedUtc = DateTime.UtcNow;
        FinishedUtc = null;

        Status = SessionStatus.New;
    }

    /// <summary>
    /// Expected source file's unique name with extension (e.g. "test.html").
    /// </summary>
    public string SourceFileUniqueFullName => $"{Id}.html";

    /// <summary>
    /// Expected converted file's unique name with extension (e.g. "test.pdf").
    /// </summary>
    public string ConvertedFileUniqueFullName => $"{Id}.pdf";

    /// <summary>
    /// Unique identifier.
    /// </summary>
    [JsonProperty(nameof(Id))]
    public Guid Id { get; set; }

    /// <summary>
    /// Human-readable file name without extension (e.g. "name" instead of "name.pdf").
    /// </summary>
    [JsonProperty(nameof(FileDisplayName))]
    public string? FileDisplayName { get; set; } // TODO rename

    /// <summary>
    /// Gets session start time in UTC.
    /// </summary>
    [JsonProperty(nameof(StartedUtc))]
    public DateTime StartedUtc { get; set; }
    /// <summary>
    /// Gets session finish time in UTC.
    /// </summary>
    [JsonProperty(nameof(FinishedUtc))]
    public DateTime? FinishedUtc { get; set; }

    /// <summary>
    /// Current status.
    /// </summary>
    public SessionStatus Status { get; set; }
}

public enum SessionStatus
{
    /// <summary>
    /// New session created.
    /// </summary>
    New = 0,
    
    /// <summary>
    /// Source file is bad and can not be converted.
    /// </summary>
    IncorrectSourceFile = 1,
    /// <summary>
    /// Converted file is successfully stored.
    /// </summary>
    StoredConvertedFile = 2
}