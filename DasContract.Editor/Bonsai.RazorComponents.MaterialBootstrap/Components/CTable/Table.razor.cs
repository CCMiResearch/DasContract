using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CTable
{
    public partial class Table : LoadableComponent
    {
        /// <summary>
        /// Tables head content
        /// </summary>
        [Parameter]
        public RenderFragment TableHead { get; set; }

        /// <summary>
        /// Tables body content
        /// </summary>
        [Parameter]
        public RenderFragment TableBody { get; set; }

        /// <summary>
        /// Tables footer content
        /// </summary>
        [Parameter]
        public RenderFragment TableFoot { get; set; }

        /// <summary>
        /// Tables caption
        /// </summary>
        [Parameter]
        public RenderFragment TableCaption { get; set; }

        /// <summary>
        /// Table scheme
        /// </summary>
        [Parameter]
        public TableScheme Scheme { get; set; } = TableScheme.Normal;

        /// <summary>
        /// Class for table defined by the scheme
        /// </summary>
        protected string TableSchemeClass => TableSchemeHelper.ToClass(Scheme);

        /// <summary>
        /// Table head scheme
        /// </summary>
        [Parameter]
        public TableHeadScheme HeadScheme { get; set; } = TableHeadScheme.Normal;

        /// <summary>
        /// Class for tables head defined by the scheme
        /// </summary>
        protected string TableHeadSchemeClass => TableHeadSchemeHelper.ToClass(HeadScheme);

        /// <summary>
        /// Table stripes
        /// </summary>
        [Parameter]
        public TableStripe Stripes { get; set; } = TableStripe.None;

        /// <summary>
        /// Class for table stripes defined by the stripes
        /// </summary>
        protected string TableStripesClass => TableStripesHelper.ToClass(Stripes);

        /// <summary>
        /// Table borders
        /// </summary>
        [Parameter]
        public TableBorder Borders { get; set; } = TableBorder.Rows;

        /// <summary>
        /// Class for table boders defined by the borders
        /// </summary>
        protected string TableBordersClass => TableBordersHelper.ToClass(Borders);

        /// <summary>
        /// Tells if the table rows are hoverable (hover effect)
        /// </summary>
        [Parameter]
        public bool HoverableRows { get; set; } = false;

        /// <summary>
        /// Returns class that sets if the rows are hoverable
        /// </summary>
        protected string HoverableRowsClass
        {
            get
            {
                if (HoverableRows)
                    return "table-hover";
                return "";
            }
        }

        /// <summary>
        /// Table size
        /// </summary>
        [Parameter]
        public TableSize Size { get; set; } = TableSize.Normal;

        /// <summary>
        /// Class for table size defined by the size
        /// </summary>
        protected string TableSizeClass => TableSizeHelper.ToClass(Size);
    }
}
