using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CBreadcrumbs
{
    public class Breadcrumb
    {
        /// <summary>
        /// Contains display name
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Contains target URL (relative to root)
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// Breadcrumb represents one natigation item in breacrumbs
        /// </summary>
        /// <param name="name">Display name</param>
        /// <param name="url">Target relative url (f.e. "/User")</param>
        public Breadcrumb(string name, string url)
        {
            Name = name;
            Url = url;
        }

        /// <summary>
        /// Factory method for breadcrumb
        /// </summary>
        /// <param name="name">Display name</param>
        /// <param name="url">Target relative url (f.e. "/User")</param>
        /// <returns>New breadcrumb</returns>
        public static Breadcrumb Create(string name, string url)
        {
            return new Breadcrumb(name, url);
        }
    }
}
