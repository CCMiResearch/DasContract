using System;
using System.Collections.Generic;
using System.Text;
using Blazor.FileReader;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CFileInput.Uploadable
{
    public class FileToUpload
    {
        /// <summary>
        /// File reference for byte operations, meta info etc.
        /// </summary>
        public IFileReference FileReference { get; set; }

        /// <summary>
        /// Name of the file
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Total size of the file
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// How much bytes has been uploaded
        /// </summary>
        public long SizeUploaded { get; set; } = 0;

        /// <summary>
        /// Tells if the upload of the file has been finished
        /// </summary>
        public bool UploadFinished 
        {
            get
            {
                return UploadSuccess || UploadError;
            }
        }

        /// <summary>
        /// Tells is the upload of the file has been successfull
        /// </summary>
        public bool UploadSuccess { get; set; } = false;

        /// <summary>
        /// Tells if the upload of the file has been affected by an error
        /// </summary>
        public bool UploadError { get; set; } = false;

        /// <summary>
        /// If there has been an error, there might be also some error message
        /// </summary>
        public string ErrorMessage { get; set; } = "";

        public FileToUpload(IFileReference fileReference, string name, long size)
        {
            FileReference = fileReference;
            Name = name;
            Size = size;
        }
    }
}
