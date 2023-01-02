namespace MarcoWillems.Template.MinimalMicroservice.Contracts.Options;

public class CachingOptions
{
    /// <summary>
    /// Redis connection string
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Redis password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Default timeout value, default to 1 minute. Which means a minute after the value is set, it expires
    /// </summary>
    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(1);
}