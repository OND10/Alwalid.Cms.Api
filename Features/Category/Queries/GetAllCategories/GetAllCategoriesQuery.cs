using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Category.Dtos;

namespace Alwalid.Cms.Api.Features.Category.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IQuery<IEnumerable<CategoryResponseDto>>
    {
        // No parameters needed for getting all categories
    }
} 