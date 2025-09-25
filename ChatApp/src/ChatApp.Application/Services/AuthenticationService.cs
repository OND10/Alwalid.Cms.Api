using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ChatApp.Application.Services;

/// <summary>
/// Service for handling authentication operations
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the AuthenticationService class
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="configuration">The configuration</param>
    /// <param name="httpClient">The HTTP client for Google API calls</param>
    public AuthenticationService(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <inheritdoc />
    public async Task<AuthTokenDto> AuthenticateWithGoogleAsync(string googleToken, CancellationToken cancellationToken = default)
    {
        // Verify Google token and get user info
        var googleUser = await VerifyGoogleTokenAsync(googleToken, cancellationToken);
        
        // Find or create user
        var user = await GetOrCreateUserAsync(googleUser, cancellationToken);
        
        // Update last active time
        await _unitOfWork.Users.UpdateLastActiveAsync(user.Id, cancellationToken);
        
        // Generate tokens
        var accessToken = GenerateAccessToken(_mapper.Map<UserDto>(user));
        var refreshToken = GenerateRefreshToken();
        
        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenExpiryDays", 7))
        };
        
        await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AuthTokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            ExpiresIn = _configuration.GetValue<int>("Jwt:ExpiryMinutes", 60) * 60,
            User = _mapper.Map<UserDto>(user)
        };
    }

    /// <inheritdoc />
    public async Task<AuthTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenEntity = await _unitOfWork.RefreshTokens.GetActiveTokenAsync(refreshToken, cancellationToken);
        if (tokenEntity == null)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        // Update last active time
        await _unitOfWork.Users.UpdateLastActiveAsync(tokenEntity.UserId, cancellationToken);

        // Generate new access token
        var accessToken = GenerateAccessToken(_mapper.Map<UserDto>(tokenEntity.User));
        
        return new AuthTokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken, // Keep the same refresh token
            TokenType = "Bearer",
            ExpiresIn = _configuration.GetValue<int>("Jwt:ExpiryMinutes", 60) * 60,
            User = _mapper.Map<UserDto>(tokenEntity.User)
        };
    }

    /// <inheritdoc />
    public async Task<bool> RevokeTokenAsync(string refreshToken, string reason = "User logout", CancellationToken cancellationToken = default)
    {
        var tokenEntity = await _unitOfWork.RefreshTokens.GetActiveTokenAsync(refreshToken, cancellationToken);
        if (tokenEntity == null)
        {
            return false;
        }

        tokenEntity.Revoke(reason);
        await _unitOfWork.RefreshTokens.UpdateAsync(tokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    /// <inheritdoc />
    public string GenerateAccessToken(UserDto user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("google_id", user.Id.ToString()) // Store Google ID as custom claim
            }),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiryMinutes", 60)),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <inheritdoc />
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// Verifies a Google OAuth token and returns user information
    /// </summary>
    /// <param name="googleToken">The Google OAuth token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Google user information</returns>
    private async Task<GoogleUserInfo> VerifyGoogleTokenAsync(string googleToken, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://www.googleapis.com/oauth2/v2/userinfo?access_token={googleToken}", cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var googleUser = JsonSerializer.Deserialize<GoogleUserInfo>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            if (googleUser == null || string.IsNullOrEmpty(googleUser.Id))
            {
                throw new UnauthorizedAccessException("Invalid Google token.");
            }
            
            return googleUser;
        }
        catch (HttpRequestException ex)
        {
            throw new UnauthorizedAccessException("Failed to verify Google token.", ex);
        }
    }

    /// <summary>
    /// Gets an existing user or creates a new one from Google user information
    /// </summary>
    /// <param name="googleUser">The Google user information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user entity</returns>
    private async Task<User> GetOrCreateUserAsync(GoogleUserInfo googleUser, CancellationToken cancellationToken)
    {
        // Try to find user by Google ID first
        var user = await _unitOfWork.Users.GetByGoogleIdAsync(googleUser.Id, cancellationToken);
        
        if (user == null)
        {
            // Try to find by email
            user = await _unitOfWork.Users.GetByEmailAsync(googleUser.Email, cancellationToken);
            
            if (user != null)
            {
                // Update existing user with Google ID
                user.GoogleId = googleUser.Id;
                user.Name = googleUser.Name ?? user.Name;
                user.ProfilePictureUrl = googleUser.Picture ?? user.ProfilePictureUrl;
                await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            }
            else
            {
                // Create new user
                user = new User
                {
                    Email = googleUser.Email,
                    Name = googleUser.Name ?? googleUser.Email,
                    GoogleId = googleUser.Id,
                    ProfilePictureUrl = googleUser.Picture
                };
                
                await _unitOfWork.Users.AddAsync(user, cancellationToken);
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        
        return user;
    }

    /// <summary>
    /// Represents Google user information from OAuth
    /// </summary>
    private class GoogleUserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Picture { get; set; }
    }
}