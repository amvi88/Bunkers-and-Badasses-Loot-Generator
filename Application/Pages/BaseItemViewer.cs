using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models;

namespace Application.Pages
{
    public class BaseItemViewer : ComponentBase, IAsyncDisposable
    {
        [Inject]
        private IJSRuntime JSRuntime {get; set;}

        private IJSObjectReference? module;
        private string? result;
        protected Guid id = Guid.NewGuid();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/index.js");
            }
        }

        protected async Task Download()
        {
            result = await DownloadImage(GetElementId(), GetFileName());
        }

        protected async Task Copy()
        {
            result = await CopyImageToClipboard(GetElementId());
        }

        public string GetElementId()
        {
            return $"widget-{id}";
        }

        public virtual string GetFileName()
        {
            return string.Empty;
        }

        public string GetMenuId()
        {
            return $"menu-{id}";
        }

        protected async ValueTask<string?> DownloadImage(string id, string fileName) =>
            module is not null ?
                await module.InvokeAsync<string>("downloadCard", id, fileName) : null;

        protected async ValueTask<string?> CopyImageToClipboard(string id) =>
            module is not null ?
                await module.InvokeAsync<string>("copyCard", id) : null;

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }
    }
}