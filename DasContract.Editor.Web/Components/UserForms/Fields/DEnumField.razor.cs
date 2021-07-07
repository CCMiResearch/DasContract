using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.UserInterface.FormFields;
namespace DasContract.Editor.Web.Components.UserForms.Fields
{
    public partial class DEnumField : ComponentBase 
    {
        [Parameter]
        public EnumField Field { get; set; }
    }
}
