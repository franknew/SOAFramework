//-----------------------------------------------------------------------
// <copyright file="IBarGlyph.cs" company="Zen Design Corp">
//     Copyright ?Zen Design Corp 2008 - 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Athena.Unitop.Sure.Lib.Barcode
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// <c>IBarGlyph</c> extends <see cref="T:Zen.Athena.Unitop.Sure.Lib.Barcode.IGlyph"/> by 
    /// specifying a bit encoding for a given character. 
    /// The bits indicate where bars are drawn.
    /// </summary>
    public interface IBarGlyph : IGlyph
    {
        /// <summary>
        /// Gets the bit encoding.
        /// </summary>
        /// <value>The bit encoding.</value>
        short BitEncoding
        {
            get;
        }
    }
}
