using ChatApp.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ChatApp.UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for the Message entity
/// </summary>
public class MessageTests
{
    [Fact]
    public void Message_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var message = new Message
        {
            Content = "Test message",
            UserId = Guid.NewGuid()
        };

        // Assert
        message.Id.Should().NotBeEmpty();
        message.Content.Should().Be("Test message");
        message.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        message.EditedAt.Should().BeNull();
        message.IsEdited.Should().BeFalse();
        message.DeletedAt.Should().BeNull();
        message.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void MarkAsEdited_ShouldSetEditedAtTimestamp()
    {
        // Arrange
        var message = new Message
        {
            Content = "Test message",
            UserId = Guid.NewGuid()
        };

        // Act
        message.MarkAsEdited();

        // Assert
        message.EditedAt.Should().NotBeNull();
        message.EditedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        message.IsEdited.Should().BeTrue();
    }

    [Fact]
    public void MarkAsDeleted_ShouldSetDeletedAtTimestamp()
    {
        // Arrange
        var message = new Message
        {
            Content = "Test message",
            UserId = Guid.NewGuid()
        };

        // Act
        message.MarkAsDeleted();

        // Assert
        message.DeletedAt.Should().NotBeNull();
        message.DeletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        message.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Restore_ShouldClearDeletedAtTimestamp()
    {
        // Arrange
        var message = new Message
        {
            Content = "Test message",
            UserId = Guid.NewGuid()
        };
        message.MarkAsDeleted();

        // Act
        message.Restore();

        // Assert
        message.DeletedAt.Should().BeNull();
        message.IsDeleted.Should().BeFalse();
    }
}