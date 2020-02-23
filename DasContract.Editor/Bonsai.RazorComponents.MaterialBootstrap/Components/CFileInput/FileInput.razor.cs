using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blazor.FileReader;
using Bonsai.Utils.String;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CFileInput
{
    public partial class FileInput: FileInputBase
    {

        [Inject]
        IFileReaderService FileReaderService { get; set; }

        /// <summary>
        /// The input element reference
        /// </summary>
        public ElementReference InputElement { get; set; }

        /// <summary>
        /// Callback for file change
        /// </summary>
        /// <param name="args"></param>
        public async Task FileChangedAsync()
        {
            var files = await FileReaderService.CreateReference(InputElement).EnumerateFilesAsync();
            await OnChange.InvokeAsync(files);
        }

        /// <summary>
        /// On file change callback
        /// </summary>
        [Parameter]
        public EventCallback<IEnumerable<IFileReference>> OnChange { get; set; }

        /// <summary>
        /// Resets the input
        /// </summary>
        public void Reset()
        {
            InputValue = "";
            StateHasChanged();
        }

        /// <summary>
        /// Inputs value
        /// </summary>
        protected string InputValue { get; set; } = "";
    }
}
