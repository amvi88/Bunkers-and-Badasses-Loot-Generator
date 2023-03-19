using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;

namespace Application.Pages
{
    public class BaseItemCrafter : BaseItemViewer 
    {
        public async Task<string> EncodeImageFile(InputFileChangeEventArgs eventArgs)
        {
            var files = eventArgs.GetMultipleFiles(1);
            var file = files.FirstOrDefault();
            if (file == null)
            {
                return null;
            }

            if (file.ContentType == "image/png")
            {
                var resizedFile = await file.RequestImageFileAsync(file.ContentType, 267, 150);
                var buf = new byte[resizedFile.Size];
                using (var stream = resizedFile.OpenReadStream())
                {
                    await stream.ReadAsync(buf);
                }
                return $"data:image/png;base64, {Convert.ToBase64String(buf)}";
            }

            return null;
        }
    }
}