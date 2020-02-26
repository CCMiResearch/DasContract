using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DasContract.Editor.Pages.Main.Services.FilePathProvider.SpecificFilePathProviders
{
    public class VersionedFilePathProvider : PackedFilePathProvider
    {
        protected string Version
        {
            get
            {
                if (version == null)
                    version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return version;
            }
        }
        private string version = null;

        public override string PathTo(string path)
        {
            /*var uriBuilder = new UriBuilder(path);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["version"] = Version;
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri.ToString();*/
            return path + "?version=" + Version;
        }
    }
}
