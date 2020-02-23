using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.Services.Interfaces
{
    public interface IFilePathProvider
    {
        /// <summary>
        /// Calculates path to a resource. 
        /// </summary>
        /// <param name="path">Target absolute path, f.e. (/Fonts/OpenSans.svg)</param>
        /// <returns>Result full path</returns>
        string PathTo(string path);

        /// <summary>
        /// Calculates path to a resource places in a RazorClassLibrary
        /// </summary>
        /// <param name="path">Target absolute path, f.e. (/Fonts/OpenSans.svg)</param>
        /// <param name="razorLibName">The target library name</param>
        /// <returns>Result full path</returns>
        string PathTo(string path, string razorLibName);

        /// <summary>
        /// Calculates clean and unmodified path to a resource
        /// </summary>
        /// <param name="path">Target path</param>
        /// <returns>Result full path</returns>
        string CleanPathTo(string path);

        /// <summary>
        /// Calculates clean and unmodified path to a resource places in a RazorClassLibrary
        /// </summary>
        /// <param name="path">Target absolute path, f.e. (/Fonts/OpenSans.svg)</param>
        /// <param name="razorLibName">The target library name</param>
        /// <returns>Result full path</returns>
        string CleanPathTo(string path, string razorLibName);
    }
}
