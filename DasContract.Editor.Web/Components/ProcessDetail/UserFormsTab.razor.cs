using DasContract.Abstraction.UserInterface;
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
        protected string UserFormScript { get; set; } = "<Form Label=\"My first form\">\n        <FieldGroup>\n            <SingleLineField Description=\"Very good description mate\" Label=\"Hello world\" />\n        </FieldGroup>\n    </Form>";

        [Inject]
        public UserFormService UserFormService { get; set; }

        private UserForm _userForm;

        [Parameter]
        public UserForm UserForm
        {
            get => _userForm;
            set
            {
                if (_userForm == value)
                    return;

                _userForm = value;
                UserFormChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<UserForm> UserFormChanged { get; set; }

        protected void SwitchPreview()
        {
            if (!UserFormService.IsPreviewOpen)
            {
                if (TryRefreshUserForm())
                {
                    UserFormService.CurrentUserForm = UserForm;
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
                UserForm = DeserializeFormScript(UserFormScript);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing XML!\n" + $"Error:\n{e.Message}\n{e.StackTrace}");
            }
            return false;
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
    }


}
