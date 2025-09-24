using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.Messages.Commands.CreateMessage;

/// <summary>
/// Handler for the CreateMessageCommand
/// </summary>
public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, MessageDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the CreateMessageCommandHandler class
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the CreateMessageCommand
    /// </summary>
    /// <param name="request">The command request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created message DTO</returns>
    public async Task<MessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        // Verify user exists
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {request.UserId} not found.", nameof(request.UserId));
        }

        // Create the message
        var message = new Message
        {
            Content = request.Content.Trim(),
            UserId = request.UserId
        };

        // Add the message to the repository
        var createdMessage = await _unitOfWork.Messages.AddAsync(message, cancellationToken);
        
        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Get the message with user information
        var messageWithUser = await _unitOfWork.Messages.GetByIdAsync(createdMessage.Id, cancellationToken);
        
        // Map to DTO and return
        return _mapper.Map<MessageDto>(messageWithUser);
    }
}