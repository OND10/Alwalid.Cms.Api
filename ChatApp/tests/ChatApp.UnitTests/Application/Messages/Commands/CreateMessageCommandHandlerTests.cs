using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Application.Messages.Commands.CreateMessage;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChatApp.UnitTests.Application.Messages.Commands;

/// <summary>
/// Unit tests for CreateMessageCommandHandler
/// </summary>
public class CreateMessageCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMessageRepository> _mockMessageRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateMessageCommandHandler _handler;

    public CreateMessageCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMessageRepository = new Mock<IMessageRepository>();
        _mockMapper = new Mock<IMapper>();

        _mockUnitOfWork.Setup(x => x.Users).Returns(_mockUserRepository.Object);
        _mockUnitOfWork.Setup(x => x.Messages).Returns(_mockMessageRepository.Object);

        _handler = new CreateMessageCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateMessage_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateMessageCommand
        {
            Content = "Test message",
            UserId = userId
        };

        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            Name = "Test User",
            GoogleId = "google123"
        };

        var createdMessage = new Message
        {
            Id = Guid.NewGuid(),
            Content = "Test message",
            UserId = userId,
            User = user
        };

        var messageDto = new MessageDto
        {
            Id = createdMessage.Id,
            Content = "Test message",
            UserId = userId,
            User = new UserDto { Id = userId, Name = "Test User", Email = "test@example.com" }
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockMessageRepository.Setup(x => x.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdMessage);

        _mockMessageRepository.Setup(x => x.GetByIdAsync(createdMessage.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdMessage);

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockMapper.Setup(x => x.Map<MessageDto>(createdMessage))
            .Returns(messageDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Content.Should().Be("Test message");
        result.UserId.Should().Be(userId);

        _mockUserRepository.Verify(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _mockMessageRepository.Verify(x => x.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockMapper.Verify(x => x.Map<MessageDto>(createdMessage), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateMessageCommand
        {
            Content = "Test message",
            UserId = userId
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act & Assert
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"User with ID {userId} not found.*");

        _mockUserRepository.Verify(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        _mockMessageRepository.Verify(x => x.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldTrimContent_WhenCreatingMessage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateMessageCommand
        {
            Content = "  Test message with spaces  ",
            UserId = userId
        };

        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            Name = "Test User",
            GoogleId = "google123"
        };

        var createdMessage = new Message
        {
            Id = Guid.NewGuid(),
            Content = "Test message with spaces", // Trimmed
            UserId = userId,
            User = user
        };

        var messageDto = new MessageDto
        {
            Id = createdMessage.Id,
            Content = "Test message with spaces",
            UserId = userId,
            User = new UserDto { Id = userId, Name = "Test User", Email = "test@example.com" }
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockMessageRepository.Setup(x => x.AddAsync(It.Is<Message>(m => m.Content == "Test message with spaces"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdMessage);

        _mockMessageRepository.Setup(x => x.GetByIdAsync(createdMessage.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdMessage);

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mockMapper.Setup(x => x.Map<MessageDto>(createdMessage))
            .Returns(messageDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Content.Should().Be("Test message with spaces");

        _mockMessageRepository.Verify(x => x.AddAsync(It.Is<Message>(m => m.Content == "Test message with spaces"), It.IsAny<CancellationToken>()), Times.Once);
    }
}