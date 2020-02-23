using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CCard
{
    public struct CardImage
    {
        public CardImage(string sourcePath, string alternativeText)
        {
            SourcePath = sourcePath;
            AlternativeText = alternativeText;
        }

        /// <summary>
        /// Source link for the image
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// Alternative text for the image
        /// </summary>
        public string AlternativeText { get; set; }

        /// <summary>
        /// Tells if this image is an empty image
        /// </summary>
        /// <returns>True if this is an empty image</returns>
        public bool IsEmpty() => SourcePath == null;

        /// <summary>
        /// Returns empty image
        /// </summary>
        /// <returns>Empty image</returns>
        public static CardImage Empty()
        {
            return new CardImage(null, null);
        }
    }
}
