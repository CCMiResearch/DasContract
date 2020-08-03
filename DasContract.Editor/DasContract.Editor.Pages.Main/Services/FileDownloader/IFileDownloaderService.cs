using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace DasContract.Editor.Pages.Main.Services.FileDownloader
{
    public interface IFileDownloaderService
    {

        Task SaveAsync(string fileName, string fileContent, string contentType = "text/plain", string charset = "utf-8");

        Task SaveAsync(string fileName, byte[] fileContent, string contentType = "text/plain", string charset = "utf-8");

    }
}
