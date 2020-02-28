using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.LanguageDictionary.Static.Languages;

namespace Bonsai.RazorComponents.MaterialBootstrap.Services.LanguageDictionary.Languages
{
    class EnglishMaterialBootstrapLanguageDictionary : EnglishStaticLanguageDictionary, IMaterialBootstrapLanguageDictionary
    {
        public EnglishMaterialBootstrapLanguageDictionary()
            : base(dictionary)
        {

        }

        public static readonly Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            //Uploadable file input
            { MaterialBootstrapLanguageDictionary.UploadableFileInputUpload, "Upload selected files" },
            { MaterialBootstrapLanguageDictionary.UploadableFileInputUploading, "Uploading..." },
            { MaterialBootstrapLanguageDictionary.UploadableFileInputDone, "Upload successful" },
            { MaterialBootstrapLanguageDictionary.UploadableFileInputUploadMore, "Upload more files" },

            //Model form
            { MaterialBootstrapLanguageDictionary.ModelFormSave, "Save" },
            { MaterialBootstrapLanguageDictionary.ModelFormReset, "Reset" },
            { MaterialBootstrapLanguageDictionary.ModelFormResetConfirm, "Do you really want to reset the form?" },
            { MaterialBootstrapLanguageDictionary.ModelFormResetConfirmButton, "Reset" },

            //Dialog window
            { MaterialBootstrapLanguageDictionary.DialogWindowCloseButton, "Close" },
        };
    }
}
