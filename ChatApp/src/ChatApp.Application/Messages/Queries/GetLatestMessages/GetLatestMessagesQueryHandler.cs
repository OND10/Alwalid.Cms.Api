using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.Messages.Queries.GetLatestMessages;

/// <summary>
/// Handler for the GetLatestMessagesQuery
/// </summary>
public class GetLatestMessagesQueryHandler : IRequestHandler<GetLatestMessagesQuery, IEnumerable<MessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetLatestMessagesQueryHandler class
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetLatestMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the GetLatestMessagesQuery
    /// </summary>
    /// <param name="request">The query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of latest message DTOs</returns>
    public async Task<IEnumerable<MessageDto>> Handle(GetLatestMessagesQuery request, CancellationToken cancellationToken)
    {
        // Validate and normalize count
        var count = Math.Min(100, Math.Max(1, request.Count));

        // Get latest messages from repository
        var messages = await _unitOfWork.Messages.GetLatestMessagesAsync(count, cancellationToken);

        // Map to DTOs and return
        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }
}