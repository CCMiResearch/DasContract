using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Utils.String;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CFileInput
{
    public class FileInputBase: LoadableComponent
    {
        /// <summary>
        /// Id of the input
        /// </summary>
        [Parameter]
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                    return Label.ToIdFriendly();
                return id;
            }
            set
            {
                id = value;
            }
        }
        string id = null;

        /// <summary>
        /// Name of the property that the file will be uploaded in
        /// </summary>
        [Parameter]
        public string Name { get; set; } = "files";

        /// <summary>
        /// Inputs label
        /// </summary>
        [Parameter]
        public string Label { get; set; } = "";

        /// <summary>
        /// Inputs description
        /// </summary>
        [Parameter]
        public string Description { get; set; } = "";

        //--------------------------------------------------
        //                 INPUT ATTRIBUTES
        //--------------------------------------------------
        /// <summary>
        /// File input attribute that specifies what type of files can be inputted
        /// </summary>
        [Parameter]
        public string Accept { get; set; } = "";

        /// <summary>
        /// What source to user for capturing image or video data
        /// </summary>
        [Parameter]
        public string Capture { get; set; } = "";

        /// <summary>
        /// If multiple files should be accepted
        /// </summary>
        [Parameter]
        public bool Multiple { get; set; } = false;


        //--------------------------------------------------
        //                    READONLY
        //--------------------------------------------------
        /// <summary>
        /// Cascading parameter for ReadOnly
        /// </summary>
        [CascadingParameter(Name = "ReadOnly")]
        public bool ReadOnlyCascade { get; set; } = false;

        /// <summary>
        /// Parameter that tells if this input is read only
        /// </summary>
        [Parameter]
        public bool ReadOnly
        {
            get
            {
                return ReadOnlyCascade || readOnly;
            }
            set
            {
                readOnly = value;
            }
        }
        private bool readOnly = false;
    }
}
