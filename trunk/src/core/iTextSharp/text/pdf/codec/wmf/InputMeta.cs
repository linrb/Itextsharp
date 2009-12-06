using System;
using System.IO;
using iTextSharp.text;

/*
 * $Id: InputMeta.cs,v 1.4 2008/05/13 11:25:36 psoares33 Exp $
 * 
 *
 * This file is part of the iText project.
 * Copyright (c) 1998-2009 1T3XT BVBA
 * Authors: Bruno Lowagie, Paulo Soares, et al.
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

namespace iTextSharp.text.pdf.codec.wmf {
    /// <summary>
    /// Summary description for InputMeta.
    /// </summary>
    public class InputMeta {
    
        Stream sr;
        int length;
    
        public InputMeta(Stream istr) {
            this.sr = istr;
        }

        public int ReadWord() {
            length += 2;
            int k1 = sr.ReadByte();
            if (k1 < 0)
                return 0;
            return (k1 + (sr.ReadByte() << 8)) & 0xffff;
        }

        public int ReadShort() {
            int k = ReadWord();
            if (k > 0x7fff)
                k -= 0x10000;
            return k;
        }

        public Int32 ReadInt() {
            length += 4;
            int k1 = sr.ReadByte();
            if (k1 < 0)
                return 0;
            int k2 = sr.ReadByte() << 8;
            int k3 = sr.ReadByte() << 16;
            return k1 + k2 + k3 + (sr.ReadByte() << 24);
        }
    
        public int ReadByte() {
            ++length;
            return sr.ReadByte() & 0xff;
        }
    
        public void Skip(int len) {
            length += len;
            Utilities.Skip(sr, len);
        }
    
        public int Length {
            get {
                return length;
            }
        }
    
        public BaseColor ReadColor() {
            int red = ReadByte();
            int green = ReadByte();
            int blue = ReadByte();
            ReadByte();
            return new BaseColor(red, green, blue);
        }
    }
}
