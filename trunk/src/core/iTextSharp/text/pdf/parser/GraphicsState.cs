using System;
using iTextSharp.text.pdf;
/*
 * $Id$
 *
 * This file is part of the iText project.
 * Copyright (c) 1998-2012 1T3XT BVBA
 * Authors: Kevin Day, Bruno Lowagie, Paulo Soares, et al.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License version 3
 * as published by the Free Software Foundation with the addition of the
 * following permission added to Section 15 as permitted in Section 7(a):
 * FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY 1T3XT,
 * 1T3XT DISCLAIMS THE WARRANTY OF NON INFRINGEMENT OF THIRD PARTY RIGHTS.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Affero General Public License for more details.
 * You should have received a copy of the GNU Affero General Public License
 * along with this program; if not, see http://www.gnu.org/licenses or write to
 * the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
 * Boston, MA, 02110-1301 USA, or download the license from the following URL:
 * http://itextpdf.com/terms-of-use/
 *
 * The interactive user interfaces in modified source and object code versions
 * of this program must display Appropriate Legal Notices, as required under
 * Section 5 of the GNU Affero General Public License.
 *
 * In accordance with Section 7(b) of the GNU Affero General Public License,
 * you must retain the producer line in every PDF that is created or manipulated
 * using iText.
 *
 * You can be released from the requirements of the license by purchasing
 * a commercial license. Buying such a license is mandatory as soon as you
 * develop commercial activities involving the iText software without
 * disclosing the source code of your own applications.
 * These activities include: offering paid services to customers as an ASP,
 * serving PDFs on the fly in a web application, shipping iText with a closed
 * source product.
 *
 * For more information, please contact iText Software Corp. at this
 * address: sales@itextpdf.com
 */
namespace iTextSharp.text.pdf.parser {

    /**
     * Keeps all the parameters of the graphics state.
     * @since   2.1.4
     */
    public class GraphicsState {
        /** The current transformation matrix. */
        internal Matrix ctm;
        /** The current character spacing. */
        internal float characterSpacing;

        virtual public float CharacterSpacing {
            get { return characterSpacing; }
        }

        /** The current word spacing. */
        internal float wordSpacing;

        virtual public float WordSpacing { 
			get { return wordSpacing; } 
		}

        /** The current horizontal scaling */
        internal float horizontalScaling;

        virtual public float HorizontalScaling {
            get { return horizontalScaling; }
        }

        /** The current leading. */
        internal float leading;
        /** The active font. */
        internal CMapAwareDocumentFont font;

        virtual public CMapAwareDocumentFont Font {
            get { return font; }
        }

        /** The current font size. */
        internal float fontSize;

        virtual public float FontSize {
            get { return fontSize; }
        }

        /** The current render mode. */
        internal int renderMode;
        /** The current text rise */
        internal float rise;
        /** The current knockout value. */
        internal bool knockout;
        /** The current color space for stroke. */
        internal PdfName colorSpaceFill;
        /** The current color space for stroke. */
        internal PdfName colorSpaceStroke;
        /** The current fill color. */
        internal BaseColor fillColor;
        /** The current stroke color. */
        internal BaseColor strokeColor;
        
        /**
         * Constructs a new Graphics State object with the default values.
         */
        public GraphicsState(){
            ctm = new Matrix();
            characterSpacing = 0;
            wordSpacing = 0;
            horizontalScaling = 1.0f;
            leading = 0;
            font = null;
            fontSize = 0;
            renderMode = 0;
            rise = 0;
            knockout = true;
            colorSpaceFill = null;
            colorSpaceStroke = null;
            fillColor = null;
            strokeColor = null;
        }
        
        /**
         * Copy constructor.
         * @param source    another GraphicsState object
         */
        public GraphicsState(GraphicsState source){
            // note: all of the following are immutable, with the possible exception of font
            // so it is safe to copy them as-is
            ctm = source.ctm;
            characterSpacing = source.characterSpacing;
            wordSpacing = source.wordSpacing;
            horizontalScaling = source.horizontalScaling;
            leading = source.leading;
            font = source.font;
            fontSize = source.fontSize;
            renderMode = source.renderMode;
            rise = source.rise;
            knockout = source.knockout;
            colorSpaceFill = source.colorSpaceFill;
            colorSpaceStroke = source.colorSpaceStroke;
            fillColor = source.fillColor;
            strokeColor = source.strokeColor;
        }

        /**
         * Getter for the current transformation matrix
         * @return the ctm
         * @since iText 5.0.1
         */
        virtual public Matrix GetCtm() {
            return ctm;
        }

        /**
         * Getter for the character spacing.
         * @return the character spacing
         * @since iText 5.0.1
         */
        virtual public float GetCharacterSpacing() {
            return characterSpacing;
        }

        /**
         * Getter for the word spacing
         * @return the word spacing
         * @since iText 5.0.1
         */
        virtual public float GetWordSpacing() {
            return wordSpacing;
        }

        /**
         * Getter for the horizontal scaling
         * @return the horizontal scaling
         * @since iText 5.0.1
         */
        virtual public float GetHorizontalScaling() {
            return horizontalScaling;
        }

        /**
         * Getter for the leading
         * @return the leading
         * @since iText 5.0.1
         */
        virtual public float GetLeading() {
            return leading;
        }

        /**
         * Getter for the font
         * @return the font
         * @since iText 5.0.1
         */
        virtual public CMapAwareDocumentFont GetFont() {
            return font;
        }

        /**
         * Getter for the font size
         * @return the font size
         * @since iText 5.0.1
         */
        virtual public float GetFontSize() {
            return fontSize;
        }

        /**
         * Getter for the render mode
         * @return the renderMode
         * @since iText 5.0.1
         */
        virtual public int GetRenderMode() {
            return renderMode;
        }

        /**
         * Getter for text rise
         * @return the text rise
         * @since iText 5.0.1
         */
        virtual public float GetRise() {
            return rise;
        }

        /**
         * Getter for knockout
         * @return the knockout
         * @since iText 5.0.1
         */
        virtual public bool IsKnockout() {
            return knockout;
        }

        /**
         * Gets the current color space for fill operations
         */
        virtual public PdfName ColorSpaceFill
        {
            get { return colorSpaceFill; }
        }

        /**
         * Gets the current color space for stroke operations
         */
        virtual public PdfName ColorSpaceStroke
        {
            get { return colorSpaceStroke; }
        }

        /**
         * Gets the current fill color
         * @return a BaseColor
         */
        virtual public BaseColor FillColor
        {
            get { return fillColor; }
        }

        /**
         * Gets the current stroke color
         * @return a BaseColor
         */
        virtual public BaseColor StrokeColor
        {
            get { return strokeColor; }
        }
    }
}
