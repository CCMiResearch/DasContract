using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ExamplesLoader
{
    public interface IExampleLoader
    {
        Task<IList<ExampleContract>> ReadManifest();
        Task<string> ReadContract(string filename);
    }
}
