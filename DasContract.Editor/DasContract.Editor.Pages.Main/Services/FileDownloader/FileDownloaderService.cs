using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace DasContract.Editor.Pages.Main.Services.FileDownloader
{
    public class FileDownloaderService : IFileDownloaderService
    {
        readonly IJSRuntime jsRuntime;

        public FileDownloaderService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task SaveAsync(string fileName, string fileContent, string contentType = "text/plain", string charset = "utf-8")
        {
            await jsRuntime.InvokeVoidAsync("DasContractPages.SaveFile", fileName, fileContent, contentType, charset);
        }

        public async Task SaveAsync(string fileName, byte[] fileContent, string contentType = "text/plain", string charset = "utf-8")
        {
            var fileContentString = Encoding.GetEncoding(charset).GetString(fileContent);
            await SaveAsync(fileName, fileContentString, contentType, charset);
        }

    }
}
