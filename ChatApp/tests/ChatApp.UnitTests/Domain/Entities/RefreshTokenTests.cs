using ChatApp.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ChatApp.UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for the RefreshToken entity
/// </summary>
public class RefreshTokenTests
{
    [Fact]
    public void RefreshToken_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var refreshToken = new RefreshToken
        {
            Token = "test_token_123",
            UserId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        // Assert
        refreshToken.Id.Should().NotBeEmpty();
        refreshToken.Token.Should().Be("test_token_123");
        refreshToken.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
        refreshToken.IsRevoked.Should().BeFalse();
        refreshToken.RevokedAt.Should().BeNull();
        refreshToken.RevokedReason.Should().BeNull();
        refreshToken.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        refreshToken.DeletedAt.Should().BeNull();
        refreshToken.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void IsActive_ShouldReturnTrue_WhenTokenIsValidAndNotRevoked()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Token = "test_token_123",
            UserId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        // Act & Assert
        refreshToken.IsActive.Should().BeTrue();
    }

    [Fact]
    public void IsActive_ShouldReturnFalse_WhenTokenIsRevoked()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Token = "test_token_123",
            UserId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        // Act
        refreshToken.Revoke("User logout");

        // Assert
        refreshToken.IsActive.Should().BeFalse();
    }

    [Fact]
    public void IsActive_ShouldReturnFalse_WhenTokenIsExpired()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Token = "test_token_123",
            UserId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(-1) // Expired
        };

        // Act & Assert
        refreshToken.IsActive.Should().BeFalse();
        refreshToken.IsExpired.Should().BeTrue();
    }

    [Fact]
    public void IsActive_ShouldReturnFalse_WhenTokenIsDeleted()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Token = "test_token_123",
            UserId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        // Act
        refreshToken.MarkAsDeleted();

        // Assert
        refreshToken.IsActive.Should().BeFalse();
        refreshToken.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Revoke_ShouldSetRevokedProperties()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Token = "test_token_123",
            UserId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        // Act
        refreshToken.Revoke("User requested logout");

        // Assert
        refreshToken.IsRevoked.Should().BeTrue();
        refreshToken.RevokedAt.Should().NotBeNull();
        refreshToken.RevokedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        refreshToken.RevokedReason.Should().Be("User requested logout");
        refreshToken.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Revoke_ShouldAllowNullReason()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Token = "test_token_123",
            UserId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        // Act
        refreshToken.Revoke();

        // Assert
        refreshToken.IsRevoked.Should().BeTrue();
        refreshToken.RevokedAt.Should().NotBeNull();
        refreshToken.RevokedReason.Should().BeNull();
    }
}