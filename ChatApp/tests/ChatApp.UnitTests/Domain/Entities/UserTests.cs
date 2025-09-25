using ChatApp.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ChatApp.UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for the User entity
/// </summary>
public class UserTests
{
    [Fact]
    public void User_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var user = new User
        {
            Email = "test@example.com",
            Name = "Test User",
            GoogleId = "google123"
        };

        // Assert
        user.Id.Should().NotBeEmpty();
        user.Email.Should().Be("test@example.com");
        user.Name.Should().Be("Test User");
        user.GoogleId.Should().Be("google123");
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.LastActiveAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.ProfilePictureUrl.Should().BeNull();
        user.Messages.Should().NotBeNull().And.BeEmpty();
        user.RefreshTokens.Should().NotBeNull().And.BeEmpty();
        user.DeletedAt.Should().BeNull();
        user.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void User_ShouldAllowSettingProfilePictureUrl()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            Name = "Test User",
            GoogleId = "google123",
            ProfilePictureUrl = "https://example.com/avatar.jpg"
        };

        // Act & Assert
        user.ProfilePictureUrl.Should().Be("https://example.com/avatar.jpg");
    }

    [Fact]
    public void User_ShouldSupportCollectionNavigation()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com",
            Name = "Test User",
            GoogleId = "google123"
        };

        var message = new Message
        {
            Content = "Test message",
            UserId = user.Id,
            User = user
        };

        var refreshToken = new RefreshToken
        {
            Token = "refresh_token_123",
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            User = user
        };

        // Act
        user.Messages.Add(message);
        user.RefreshTokens.Add(refreshToken);

        // Assert
        user.Messages.Should().HaveCount(1);
        user.Messages.First().Should().Be(message);
        user.RefreshTokens.Should().HaveCount(1);
        user.RefreshTokens.First().Should().Be(refreshToken);
    }
}