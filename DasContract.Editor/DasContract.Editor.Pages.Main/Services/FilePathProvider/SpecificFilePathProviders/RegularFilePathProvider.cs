using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Pages.Main.Services.FilePathProvider.SpecificFilePathProviders
{
    public class RegularFilePathProvider : PackedFilePathProvider
    {
        public override string PathTo(string path) => path;
    }
}
