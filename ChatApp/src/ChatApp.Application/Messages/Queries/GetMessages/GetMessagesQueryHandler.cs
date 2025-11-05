using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Interfaces;
using MediatR;

namespace ChatApp.Application.Messages.Queries.GetMessages;

/// <summary>
/// Handler for the GetMessagesQuery
/// </summary>
public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, PaginatedListDto<MessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetMessagesQueryHandler class
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Handles the GetMessagesQuery
    /// </summary>
    /// <param name="request">The query request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of message DTOs</returns>
    public async Task<PaginatedListDto<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        // Validate and normalize parameters
        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Min(100, Math.Max(1, request.PageSize));

        // Get messages from repository
        var (messages, totalCount) = await _unitOfWork.Messages.GetMessagesAsync(
            pageNumber, 
            pageSize, 
            request.UserId, 
            cancellationToken);

        // Map to DTOs
        var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);

        return new PaginatedListDto<MessageDto>
        {
            Items = messageDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}