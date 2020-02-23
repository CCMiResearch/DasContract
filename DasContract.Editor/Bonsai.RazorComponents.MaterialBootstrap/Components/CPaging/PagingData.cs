using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CPaging
{
    public class PagingData
    {
        /// <summary>
        /// Where is the paging now
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// From what item is the paging now set
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// To what item is the paging now set
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// Number of pages
        /// </summary>
        public int PagesCount { get; set; }

        /// <summary>
        /// Total number of items handled by the paging
        /// </summary>
        public int TotalItemsCount { get; set; }
    }
}
