using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Alwalid.Cms.Api.Common.Validation;
using Alwalid.Cms.Api.Common.Helper.Interface;

namespace Alwalid.Cms.Api.Common.Helper.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageRepository(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        {
            _webHost = webHost;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Upload(object model, IFormFile file)
        {
            var orgModel = model.GetType();
            string modelName = orgModel.Name;

            ImageValidation.ValidationFileUpload(file);

            if (file != null)
            {
                // Uploading the image to the specified folder
                var folderPath = System.IO.Path.Combine(_webHost.ContentRootPath, "Images", $"{modelName}");
                var localPath = System.IO.Path.Combine(folderPath, file.FileName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using var stream = new FileStream(localPath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Store filename and extension to the DB
                var httpRequest = _httpContextAccessor.HttpContext.Request;
                var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{modelName}/{file.FileName}";

                return await Task.FromResult<string>(urlPath);
            }

            return await Task.FromResult<string>("");
        }
    }
} 