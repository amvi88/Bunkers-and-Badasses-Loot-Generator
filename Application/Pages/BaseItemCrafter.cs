using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;
using Models.Builder;

namespace Application.Pages
{
    public class BaseItemCrafter : BaseItemViewer
    {
        public bool UseCustomImage { get; set; } = false;
        public bool AccordionOneCollapsed { get; set; } = false;
        public bool AccordionTwoCollapsed { get; set; } = true;
        public bool AccordionThreeCollapsed { get; set; } = true;
        public bool AccordionFourCollapsed { get; set; } = true;
        public bool AccordionFiveCollapsed { get; set; } = true;
        public bool UseBorderlandsManufacturers { get; set; } = false;

        public List<Guild> guilds = new List<Guild>();

        public async Task<string?> EncodeImageFile(InputFileChangeEventArgs eventArgs)
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

        public void ToggleAccordionOne()
        {
            AccordionOneCollapsed = !AccordionOneCollapsed;
            this.StateHasChanged();
        }

        public void ToggleAccordionTwo()
        {
            AccordionTwoCollapsed = !AccordionTwoCollapsed;
            this.StateHasChanged();
        }

        public void ToggleAccordionThree()
        {
            AccordionThreeCollapsed = !AccordionThreeCollapsed;
            this.StateHasChanged();
        }

        public void ToggleAccordionFour()
        {
            AccordionFourCollapsed = !AccordionFourCollapsed;
            this.StateHasChanged();
        }

        public void ToggleAccordionFive()
        {
            AccordionFiveCollapsed = !AccordionFiveCollapsed;
            this.StateHasChanged();
        }

        public string AccordionOneClass => !AccordionOneCollapsed ? "collapse show" : "collapse";
        public string AccordionTwoClass => !AccordionTwoCollapsed ? "collapse show" : "collapse";
        public string AccordionThreeClass => !AccordionThreeCollapsed ? "collapse show" : "collapse";
        public string AccordionFourClass => !AccordionFourCollapsed ? "collapse show" : "collapse";
        public string AccordionFiveClass => !AccordionFiveCollapsed ? "collapse show" : "collapse";
    }
}