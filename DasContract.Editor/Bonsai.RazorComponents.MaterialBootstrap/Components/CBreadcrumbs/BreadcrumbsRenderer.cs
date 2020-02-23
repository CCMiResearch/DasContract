using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CBreadcrumbs
{
    public class BreadcrumbsRenderer
    {
        /// <summary>
        /// List of current breadcrumbs trail
        /// </summary>
        public List<Breadcrumb> Trail { get; } = new List<Breadcrumb>();

        /// <summary>
        /// Events triggered on new breadcrumbs finished building
        /// </summary>
        public event BreadcrumbsEventHandler OnNewBreadcrumbs;

        /// <summary>
        /// Removes all crumbs
        /// </summary>
        void Clear()
        {
            Trail.Clear();
        }

        /// <summary>
        /// Initializes new crumb trail
        /// </summary>
        /// <returns>This</returns>
        public BreadcrumbsRenderer Initialize()
        {
            Clear();
            return this;
        }

        /// <summary>
        ///  Adds base crumb
        /// </summary>
        /// <param name="name">The name of the home</param>
        /// <returns></returns>
        public BreadcrumbsRenderer AddHome(string name = "Index")
        {
            Initialize();
            Trail.Add(new Breadcrumb(name, "/"));
            return this;
        }

        /// <summary>
        /// Add base crumb
        /// </summary>
        /// <param name="name">The name of the home</param>
        /// <param name="url">The url of home</param>
        /// <returns></returns>
        public BreadcrumbsRenderer AddHome(string name, string url = "/")
        {
            Initialize();
            Trail.Add(new Breadcrumb(name, url));
            return this;
        }

        /// <summary>
        /// Adds new crumb to the trail
        /// </summary>
        /// <param name="name">Display name</param>
        /// <param name="url">Target relative url (f.e. "/User")</param>
        /// <returns>This</returns>
        public BreadcrumbsRenderer AddCrumb(string name, string url)
        {
            Trail.Add(new Breadcrumb(name, url));
            return this;
        }

        /// <summary>
        /// Adds new crumbs to the trail
        /// </summary>
        /// <param name="crumb">The new crumb</param>
        /// <returns>This</returns>
        public BreadcrumbsRenderer AddCrumb(Breadcrumb crumb)
        {
            Trail.Add(crumb);
            return this;
        }

        /// <summary>
        /// Adds the last crumb and finishes rendering
        /// </summary>
        /// <param name="name">Last crumbs name</param>
        public void AddLastCrumb(string name)
        {
            Trail.Add(new Breadcrumb(name, ""));
            Finish();
        }

        /// <summary>
        /// Adds the last crumb and finishes rendering
        /// </summary>
        /// <param name="crumb">The last crumb</param>
        public void AddLastCrumb(Breadcrumb crumb)
        {
            AddLastCrumb(crumb.Name);
        }


        /// <summary>
        /// Finished breadcrumb building
        /// </summary>
        public void Finish()
        {
            OnNewBreadcrumbs?.Invoke(this, new BreadcrumbsArgs(Trail));
        }


    }

    public delegate void BreadcrumbsEventHandler(BreadcrumbsRenderer caller, BreadcrumbsArgs args);
}
