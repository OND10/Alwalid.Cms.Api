namespace Alwalid.Cms.Api.Common.Validation
{
    public static class ImageValidation
    {
        public static bool ValidationFileUpload(IFormFile file)
        {
            var preferedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!preferedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                return false;
            }

            if (file.Length > 10485760)
            {
                return false;
            }
            return true;
        }

    }

}
