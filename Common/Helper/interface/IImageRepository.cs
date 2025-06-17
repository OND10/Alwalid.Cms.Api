using Microsoft.AspNetCore.Http;

namespace Alwalid.Cms.Api.Common.Helper.Interface
{
    public interface IImageRepository
    {
        Task<string> Upload(object model, IFormFile file);
    }
} 