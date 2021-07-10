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
                UserTask.Form = DeserializeFormScript(UserTask.FormScript);
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
            UserTask.FormScript = script;
            if(Refresh.AutoRefresh)
            {
                TryRefreshUserForm();
            }
        }

        public UserForm DeserializeFormScript(string formScript)
        {
            using TextReader reader = new StringReader(formScript);
            XmlSerializer serializer = create_throwing_serializer();
            XmlReader xmlReader = new XmlTextReader(reader);

            return (UserForm)serializer.Deserialize(xmlReader);
        }

        private void Serializer_Throw(object sender, XmlElementEventArgs e)
        {
            throw new Exception("XML format exception.");
        }
        private void Serializer_Throw(object sender, XmlAttributeEventArgs e)
        {
            throw new Exception("XML format exception.");
        }
        private void Serializer_Throw(object sender, XmlNodeEventArgs e)
        {
            throw new Exception("XML format exception.");
        }
        private XmlSerializer create_throwing_serializer()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserForm));
            serializer.UnknownAttribute += new XmlAttributeEventHandler(Serializer_Throw);
            serializer.UnknownElement += new XmlElementEventHandler(Serializer_Throw);
            serializer.UnknownNode += new XmlNodeEventHandler(Serializer_Throw);
            return serializer;
        }

        public void Dispose()
        {
            UserFormService.IsPreviewOpen = false;
        }

        protected void OnScriptChanged(string script)
        {
            UserTask.FormScript = script;
            if (Refresh.AutoRefresh)
                TryRefreshUserForm();
        }
    }


}
