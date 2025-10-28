using ChatApp.Application.DTOs;

namespace ChatApp.Application.Services;

/// <summary>
/// Interface for authentication services
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user with Google OAuth
    /// </summary>
    /// <param name="googleToken">The Google OAuth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result with tokens and user information</returns>
    Task<AuthTokenDto> AuthenticateWithGoogleAsync(string googleToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes an access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">The refresh token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>New authentication tokens</returns>
    Task<AuthTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes a refresh token
    /// </summary>
    /// <param name="refreshToken">The refresh token to revoke</param>
    /// <param name="reason">The reason for revoking the token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the token was revoked successfully</returns>
    Task<bool> RevokeTokenAsync(string refreshToken, string reason = "User logout", CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a JWT access token for a user
    /// </summary>
    /// <param name="user">The user DTO</param>
    /// <returns>The JWT token</returns>
    string GenerateAccessToken(UserDto user);

    /// <summary>
    /// Generates a refresh token
    /// </summary>
    /// <returns>The refresh token</returns>
    string GenerateRefreshToken();
}