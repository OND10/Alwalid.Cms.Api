namespace ChatApp.Application.DTOs;

/// <summary>
/// Data transfer object for authentication tokens
/// </summary>
public class AuthTokenDto
{
    /// <summary>
    /// Gets or sets the access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token type (usually "Bearer")
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Gets or sets the expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Gets or sets the user information
    /// </summary>
    public UserDto User { get; set; } = null!;
}