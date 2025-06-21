using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Category.Dtos;

namespace Alwalid.Cms.Api.Features.Category.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IQuery<CategoryResponseDto>
    {
        public int Id { get; set; }
    }
} 