using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bonsai.Services.Interfaces;

namespace DasContract.Editor.Pages.Main.Services.FilePathProvider
{
    public abstract class PackedFilePathProvider : IFilePathProvider
    {
        public string CleanPathTo(string path) => path;

        public string CleanPathTo(string path, string razorLibName)
        {
            if (!path.StartsWith("/"))
                path = "/" + path;
            return CleanPathTo("/_content/" + razorLibName + path);
        }

        public abstract string PathTo(string path);

        public string PathTo(string path, string razorLibName)
        {
            if (!path.StartsWith("/"))
                path = "/" + path;
            return PathTo("/_content/" + razorLibName + path);
        }


    }
}
