using ChatApp.Application.DTOs;
using ChatApp.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Web.Controllers;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ChatApp.Application.Services.IAuthenticationService _authenticationService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the AuthController class
    /// </summary>
    /// <param name="authenticationService">The authentication service</param>
    /// <param name="logger">The logger</param>
    public AuthController(ChatApp.Application.Services.IAuthenticationService authenticationService, ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Initiates Google OAuth login by redirecting to Google
    /// </summary>
    /// <returns>Redirect to Google OAuth</returns>
    [HttpGet("login")]
    public IActionResult Login()
    {
        var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl
        };
        
        return Challenge(properties, "Google");
    }

    /// <summary>
    /// Handles the Google OAuth callback
    /// </summary>
    /// <returns>Redirect to the frontend with authentication result</returns>
    [HttpGet("callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        try
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (!result.Succeeded)
            {
                _logger.LogWarning("Google authentication failed");
                return Redirect($"{GetFrontendUrl()}?error=auth_failed");
            }

            var googleToken = result.Properties?.GetTokenValue("access_token");
            if (string.IsNullOrEmpty(googleToken))
            {
                _logger.LogWarning("No access token received from Google");
                return Redirect($"{GetFrontendUrl()}?error=no_token");
            }

            var authResult = await _authenticationService.AuthenticateWithGoogleAsync(googleToken);
            
            // In a real application, you might want to set secure HTTP-only cookies
            // or redirect to a frontend page that handles the tokens
            return Redirect($"{GetFrontendUrl()}?access_token={authResult.AccessToken}&refresh_token={authResult.RefreshToken}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Google OAuth callback");
            return Redirect($"{GetFrontendUrl()}?error=callback_failed");
        }
    }

    /// <summary>
    /// Authenticates with Google using an access token
    /// </summary>
    /// <param name="request">The authentication request</param>
    /// <returns>Authentication tokens and user information</returns>
    [HttpPost("google")]
    public async Task<ActionResult<AuthTokenDto>> AuthenticateWithGoogle([FromBody] GoogleAuthRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.GoogleToken))
            {
                return BadRequest(new { error = "Google token is required" });
            }

            var result = await _authenticationService.AuthenticateWithGoogleAsync(request.GoogleToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Google authentication failed");
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Google authentication");
            return StatusCode(500, new { error = "Authentication failed" });
        }
    }

    /// <summary>
    /// Refreshes an access token using a refresh token
    /// </summary>
    /// <param name="request">The refresh token request</param>
    /// <returns>New authentication tokens</returns>
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthTokenDto>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { error = "Refresh token is required" });
            }

            var result = await _authenticationService.RefreshTokenAsync(request.RefreshToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Token refresh failed");
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, new { error = "Token refresh failed" });
        }
    }

    /// <summary>
    /// Logs out the user by revoking their refresh token
    /// </summary>
    /// <param name="request">The logout request</param>
    /// <returns>Logout result</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { error = "Refresh token is required" });
            }

            var success = await _authenticationService.RevokeTokenAsync(request.RefreshToken, "User logout");
            
            if (success)
            {
                return Ok(new { message = "Logged out successfully" });
            }
            
            return BadRequest(new { error = "Invalid refresh token" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, new { error = "Logout failed" });
        }
    }

    /// <summary>
    /// Gets the current user information
    /// </summary>
    /// <returns>Current user information</returns>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        return Ok(new
        {
            Id = userId,
            Email = email,
            Name = name
        });
    }

    /// <summary>
    /// Gets the frontend URL from configuration
    /// </summary>
    /// <returns>The frontend URL</returns>
    private string GetFrontendUrl()
    {
        return "https://localhost:7206"; // In production, this should come from configuration
    }
}

/// <summary>
/// Request model for Google authentication
/// </summary>
public class GoogleAuthRequest
{
    /// <summary>
    /// Gets or sets the Google OAuth access token
    /// </summary>
    public string GoogleToken { get; set; } = string.Empty;
}

/// <summary>
/// Request model for token refresh
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Gets or sets the refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Request model for logout
/// </summary>
public class LogoutRequest
{
    /// <summary>
    /// Gets or sets the refresh token to revoke
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}