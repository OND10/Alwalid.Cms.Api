using ChatApp.Application.Messages.Commands.CreateMessage;
using FluentAssertions;
using Xunit;

namespace ChatApp.UnitTests.Application.Messages.Commands;

/// <summary>
/// Unit tests for CreateMessageCommandValidator
/// </summary>
public class CreateMessageCommandValidatorTests
{
    private readonly CreateMessageCommandValidator _validator;

    public CreateMessageCommandValidatorTests()
    {
        _validator = new CreateMessageCommandValidator();
    }

    [Fact]
    public void Validate_ShouldReturnValid_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateMessageCommand
        {
            Content = "Valid message content",
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_ShouldReturnInvalid_WhenContentIsEmpty(string content)
    {
        // Arrange
        var command = new CreateMessageCommand
        {
            Content = content,
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateMessageCommand.Content))
            .Which.ErrorMessage.Should().Be("Message content is required.");
    }

    [Fact]
    public void Validate_ShouldReturnInvalid_WhenContentExceedsMaxLength()
    {
        // Arrange
        var longContent = new string('a', 2001); // Exceeds 2000 character limit
        var command = new CreateMessageCommand
        {
            Content = longContent,
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateMessageCommand.Content))
            .Which.ErrorMessage.Should().Be("Message content cannot exceed 2000 characters.");
    }

    [Fact]
    public void Validate_ShouldReturnValid_WhenContentIsAtMaxLength()
    {
        // Arrange
        var maxLengthContent = new string('a', 2000); // Exactly 2000 characters
        var command = new CreateMessageCommand
        {
            Content = maxLengthContent,
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnInvalid_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new CreateMessageCommand
        {
            Content = "Valid message content",
            UserId = Guid.Empty
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateMessageCommand.UserId))
            .Which.ErrorMessage.Should().Be("User ID is required.");
    }

    [Fact]
    public void Validate_ShouldReturnMultipleErrors_WhenMultipleFieldsAreInvalid()
    {
        // Arrange
        var command = new CreateMessageCommand
        {
            Content = "", // Invalid
            UserId = Guid.Empty // Invalid
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateMessageCommand.Content));
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateMessageCommand.UserId));
    }
}