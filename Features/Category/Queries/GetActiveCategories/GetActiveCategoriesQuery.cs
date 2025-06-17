using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Category.Dtos;

namespace Alwalid.Cms.Api.Features.Category.Queries.GetActiveCategories
{
    public class GetActiveCategoriesQuery : IQuery<IEnumerable<CategoryResponseDto>>
    {
        // No parameters needed for getting active categories
    }
} 