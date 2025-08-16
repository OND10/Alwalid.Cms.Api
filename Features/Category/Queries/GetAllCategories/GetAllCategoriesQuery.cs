using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Category.Dtos;

namespace Alwalid.Cms.Api.Features.Category.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : ICacheableQuery<IEnumerable<CategoryResponseDto>>
    {
        public string CacheKey => "GetAllCategories";
        public TimeSpan? CacheDuration => TimeSpan.FromMinutes(10);
    }
} 