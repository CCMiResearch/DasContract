using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CCard
{
    public partial class Card: LoadableComponent
    {
        /// <summary>
        /// The top image object settings
        /// </summary>
        [Parameter]
        public CardImage TopImage { get; set; } = CardImage.Empty();

        /// <summary>
        /// The bottom image object settings
        /// </summary>
        [Parameter]
        public CardImage BottomImage { get; set; } = CardImage.Empty();

        /// <summary>
        /// Sets the with of the card
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "auto";

        /// <summary>
        /// Tells if the cards should be displayed inline
        /// </summary>
        [Parameter]
        public bool DisplayInline
        {
            get
            {
                if (displayInline == null)
                    return Width != "auto" && Width != null && string.IsNullOrEmpty(Width);
                return displayInline.Value;

            }
            set
            {
                displayInline = value;
            }
        }
        bool? displayInline = null;

        /// <summary>
        /// Card alignment settings
        /// </summary>
        [Parameter]
        public ContentAlign Align { get; set; } = ContentAlign.Left;

        /// <summary>
        /// Card class for aligment
        /// </summary>
        protected string AlignClass => CardContentAlignHelper.ToClass(Align);

        /// <summary>
        /// Card theme color setting
        /// </summary>
        [Parameter]
        public CardScheme Scheme { get; set; } = CardScheme.None;

        /// <summary>
        /// Card class for theme
        /// </summary>
        protected string SchemeClass => CardSchemeHelper.ToClass(Scheme);

        /// <summary>
        /// The header part of the card
        /// </summary>
        [Parameter]
        public RenderFragment Header { get; set; }

        /// <summary>
        /// The body part of the card
        /// </summary>
        [Parameter]
        public RenderFragment Body { get; set; }

        /// <summary>
        /// The footer part of the card
        /// </summary>
        [Parameter]
        public RenderFragment Footer { get; set; }

        /// <summary>
        /// Other content directly inside the card, right after the content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
