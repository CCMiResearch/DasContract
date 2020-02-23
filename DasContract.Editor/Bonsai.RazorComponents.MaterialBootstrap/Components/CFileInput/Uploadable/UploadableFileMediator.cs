using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CFileInput.Uploadable
{
    public class UploadableFileMediator
    {
        /// <summary>
        /// JS mediator
        /// </summary>
        readonly IJSRuntime JSRuntime;

        /// <summary>
        /// Event triggered whenewer there has been any change
        /// </summary>
        public event UploadableFileMediatorHandler OnChange;

        /// <summary>
        /// List of files to be uploaded
        /// </summary>
        readonly List<FileToUpload> FilesToUpload;

        /// <summary>
        /// Tells if all files has been uploaded
        /// </summary>
        public bool AllFilesUploaded
        {
            get
            {
                foreach (var file in FilesToUpload)
                    if (!file.UploadFinished)
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Tells if all files has been uploaded successfully
        /// </summary>
        public bool AllFilesSuccess
        {
            get
            {
                foreach (var file in FilesToUpload)
                    if (!file.UploadSuccess)
                        return false;
                return true;
            }
        }

        /// <summary>
        /// ID if the input
        /// </summary>
        string InputId { get; set; }

        /// <summary>
        /// Name of the input
        /// </summary>
        string Name { get; set; }

        
        public UploadableFileMediator(List<FileToUpload> filesToUpload, IJSRuntime jSRuntime, string inputId, string name)
        {
            FilesToUpload = filesToUpload;
            JSRuntime = jSRuntime;
            InputId = inputId;
            Name = name;
        }

        /// <summary>
        /// Start uploading files one by one
        /// </summary>
        /// <param name="fileToUploads">Files to upload</param>
        public async Task UploadAsync(string targetUrl)
        {
            var thisObjectReference = DotNetObjectReference.Create(this);
            await JSRuntime.InvokeVoidAsync("MaterialBootstrapRazorComponents.UploadableFileInput.Upload", InputId, targetUrl, Name, thisObjectReference);
        }

        /// <summary>
        /// Updates a status of a file
        /// </summary>
        /// <param name="id">File id in the list</param>
        /// <param name="sent">How much data has been already sent</param>
        [JSInvokable]
        public void UpdateFileStatus(int id, long sent)
        {
            if (id >= FilesToUpload.Count)
                return;

            FilesToUpload[id].SizeUploaded = sent;
            OnChange?.Invoke(this, new UploadableFileMediatorArgs());
        }

        /// <summary>
        /// Updates a status of a file to finished
        /// </summary>
        /// <param name="id">File id in the list</param>
        [JSInvokable]
        public void FinishFileUploading(int id)
        {
            if (id >= FilesToUpload.Count)
                return;

            FilesToUpload[id].UploadSuccess = true;
            OnChange?.Invoke(this, new UploadableFileMediatorArgs());
        }

        /// <summary>
        /// Marks an error while the file is uploading
        /// </summary>
        /// <param name="id">File if in the list</param>
        [JSInvokable]
        public void ErrorFileUploading(int id, string error)
        {
            if (id >= FilesToUpload.Count)
                return;

            FilesToUpload[id].UploadError = true;
            FilesToUpload[id].ErrorMessage = error;
            OnChange?.Invoke(this, new UploadableFileMediatorArgs());
        }
    }

    public delegate void UploadableFileMediatorHandler(UploadableFileMediator caller, UploadableFileMediatorArgs args);

    public class UploadableFileMediatorArgs
    {

    }
}
