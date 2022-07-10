using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Business.Models;

namespace Application.Pages
{
    public class BaseItemViewer : ComponentBase, IAsyncDisposable
    {
        [Inject]
        private IJSRuntime JSRuntime {get; set;}

        [Parameter]
        public string? FileName {get; set;}

        private IJSObjectReference? module;
        private string? result;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/index.js");
            }
        }

        protected async Task Download()
        {
            result = await DownloadImage("lewt", FileName);
        }

        protected async ValueTask<string?> DownloadImage(string id, string fileName) =>
            module is not null ?
                await module.InvokeAsync<string>("downloadCard", id, fileName) : null;

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }
    }
}