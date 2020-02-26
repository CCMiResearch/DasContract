using System;
using System.Net.Http;
using System.Text;

namespace DasContract.Editor.Utils.String
{
    public static class AsJsonExtension
    {
        public static HttpContent AsJson(this string content)
        {
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}
