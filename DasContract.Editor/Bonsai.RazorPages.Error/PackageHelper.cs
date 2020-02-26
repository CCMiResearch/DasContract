using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorPages.Error
{
    public class LibraryInfo
    {
        public static string Name { get; set; } = typeof(LibraryInfo).Assembly.GetName().Name;
    }
}
