using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.LanguageDictionary.Static.Languages;

namespace Bonsai.RazorComponents.MaterialBootstrap.Services.LanguageDictionary.Languages
{
    class CzechMaterialBootstrapLanguageDictionary: CzechStaticLanguageDictionary, IMaterialBootstrapLanguageDictionary
    {
        public CzechMaterialBootstrapLanguageDictionary()
            : base(dictionary)
        {

        }

        public static readonly Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
            //Uploadable file input
            { MaterialBootstrapLanguageDictionary.UploadableFileInputUpload, "Nahrát vybrané soubory" },
            { MaterialBootstrapLanguageDictionary.UploadableFileInputUploading, "Nahrává se..." },
            { MaterialBootstrapLanguageDictionary.UploadableFileInputDone, "Nahrávání úspěšně dokončeno" },
            { MaterialBootstrapLanguageDictionary.UploadableFileInputUploadMore, "Nahrát více souborů" },

            //Model form
            { MaterialBootstrapLanguageDictionary.ModelFormSave, "Uložit" },
            { MaterialBootstrapLanguageDictionary.ModelFormReset, "Resetovat" },
            { MaterialBootstrapLanguageDictionary.ModelFormResetConfirm, "Opravdu chcete resetovat formulář?" },
            { MaterialBootstrapLanguageDictionary.ModelFormResetConfirmButton, "Resetovat" },
        };
    }
}
