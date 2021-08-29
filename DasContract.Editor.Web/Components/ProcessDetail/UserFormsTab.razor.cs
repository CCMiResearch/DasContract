using DasContract.Abstraction.Processes.Tasks;
using DasContract.Abstraction.UserInterface;
using DasContract.Editor.Web.Components.Common;
using DasContract.Editor.Web.Services.UserForm;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DasContract.Editor.Web.Components.ProcessDetail
{
    public partial class UserFormsTab: ComponentBase, IDisposable
    {
        //protected string UserFormScript { get; set; } = "<Form Label=\"My first form\">\n        <FieldGroup>\n            <SingleLineField Description=\"Very good description mate\" Label=\"Hello world\" />\n        </FieldGroup>\n    </Form>";

        [Inject]
        public UserFormService UserFormService { get; set; }

        protected Refresh Refresh { get; set; }

        private UserTask _userTask;

        [Parameter]
        public UserTask UserTask
        {
            get => _userTask;
            set
            {
                if (_userTask == value)
                    return;

                _userTask = value;
                UserTaskChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<UserTask> UserTaskChanged { get; set; }

        protected void SwitchPreview()
        {
            if (!UserFormService.IsPreviewOpen)
            {
                if (TryRefreshUserForm())
                {
                    UserFormService.IsPreviewOpen = true;
                }
            }
            else
            {
                UserFormService.IsPreviewOpen = false;
            }
        }

        public bool TryRefreshUserForm()
        {
            try
            {
                UserTask.Form = UserForm.DeserializeFormScript(UserTask.FormDefinition);
                UserFormService.CurrentUserForm = UserTask.Form;
                UserFormService.RequestRefresh();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing XML!\n" + $"Error:\n{e.Message}\n{e.StackTrace}");
            }
            return false;
        }

        protected void OnUserFormScriptChange(string script)
        {
            UserTask.FormDefinition = script;
            if(Refresh.AutoRefresh)
            {
                TryRefreshUserForm();
            }
        }

        

        public void Dispose()
        {
            UserFormService.IsPreviewOpen = false;
        }

        protected void OnScriptChanged(string script)
        {
            UserTask.FormDefinition = script;
            if (Refresh.AutoRefresh)
                TryRefreshUserForm();
        }
    }


}
