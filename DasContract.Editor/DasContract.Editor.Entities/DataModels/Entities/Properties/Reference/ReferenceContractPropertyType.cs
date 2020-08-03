using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DasContract.Editor.Entities.DataModels.Entities.Properties.Reference
{
    public enum ReferenceContractPropertyType
    {
        [Display(Name = "Single reference")]
        SingleReference,

        [Display(Name = "Collection of references")]
        ReferenceCollection
    }
}
