using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blazor.FileReader;
using Bonsai.Utils.String;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CFileInput.Uploadable
{
    public partial class UploadableFileInput: FileInputBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Target upload URL
        /// </summary>
        [Parameter]
        public string UrlTarget { get; set; } = "";

        /// <summary>
        /// Current state of this input
        /// </summary>
        public UploadableFileInputState State { get; protected set; } = UploadableFileInputState.Selecting;

        /// <summary>
        /// File input used inside this component
        /// </summary>
        public FileInput FileInput { get; protected set; } = null;

        /// <summary>
        /// List of files to be uploaded
        /// </summary>
        public List<FileToUpload> FilesToUpload { get; protected set; } = new List<FileToUpload>();

        /// <summary>
        /// Resets this input
        /// </summary>
        public void Reset()
        {
            FilesToUpload.Clear();
            State = UploadableFileInputState.Selecting;
            if(FileInput != null)
                FileInput.Reset();
            StateHasChanged();
        }

        /// <summary>
        /// Starts uploading process
        /// </summary>
        public async Task StartUploadingAsync()
        {
            State = UploadableFileInputState.Uploading;

            var mediator = new UploadableFileMediator(FilesToUpload, JSRuntime, Id, Name);
            mediator.OnChange += (caller, args) =>
            {
                if (mediator.AllFilesSuccess)
                    State = UploadableFileInputState.Finished;
                StateHasChanged();
            };
            await mediator.UploadAsync(UrlTarget);
        }

        /// <summary>
        /// Callback for when the file input has changed
        /// </summary>
        /// <param name="selectedFile">Collection of selected files</param>
        /// <returns>Task</returns>
        async Task OnFileSelectionChangeAsync(IEnumerable<IFileReference> selectedFile)
        {
            FilesToUpload.Clear();
            foreach(var file in selectedFile)
            {
                var fileConfig = await file.ReadFileInfoAsync();
                var name = fileConfig.Name;
                var size = fileConfig.Size;
                FilesToUpload.Add(new FileToUpload(file, name, size));
            }
        }

        public static string NumberToMB(double number)
        {
            //return Math.Round(number / 1000000, 2) + " MB";
            return Math.Round((number / 1024f) / 1024f, 2) + " MB";
        }
    }
}
