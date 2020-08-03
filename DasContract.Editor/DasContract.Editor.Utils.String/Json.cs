using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace DasContract.Editor.Utils.String
{
    public static class JsonExtension
    {
        public static HttpContent AsJson(this string content)
        {
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        public static string ToJsonString(this object content)
        {
            return JsonConvert.SerializeObject(content);
        }

        public static HttpContent ToJsonContent(this object content)
        {
            return content.ToJsonString().AsJson();
        }
    }
}
