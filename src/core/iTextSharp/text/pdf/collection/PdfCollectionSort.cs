using System;
using iTextSharp.text.pdf;
using iTextSharp.text.error_messages;

/*
 * $Id:  $
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

namespace iTextSharp.text.pdf.collection {

    public class PdfCollectionSort : PdfDictionary {
        
        /**
        * Constructs a PDF Collection Sort Dictionary.
        * @param key   the key of the field that will be used to sort entries
        */
        public PdfCollectionSort(String key) : base(PdfName.COLLECTIONSORT) {
            Put(PdfName.S, new PdfName(key));
        }
        
        /**
        * Constructs a PDF Collection Sort Dictionary.
        * @param keys  the keys of the fields that will be used to sort entries
        */
        public PdfCollectionSort(String[] keys) : base(PdfName.COLLECTIONSORT) {
            PdfArray array = new PdfArray();
            for (int i = 0; i < keys.Length; i++) {
                array.Add(new PdfName(keys[i]));
            }
            Put(PdfName.S, array);
        }
        
        /**
        * Defines the sort order of the field (ascending or descending).
        * @param ascending true is the default, use false for descending order
        */
        public void SetSortOrder(bool ascending) {
            PdfObject o = (PdfObject)Get(PdfName.S);
            if (o is PdfName) {
                Put(PdfName.A, new PdfBoolean(ascending));
            }
            else {
                throw new InvalidOperationException(MessageLocalization.GetComposedMessage("you.have.to.define.a.bool.array.for.this.collection.sort.dictionary"));
            }
        }
        
        /**
        * Defines the sort order of the field (ascending or descending).
        * @param ascending an array with every element corresponding with a name of a field.
        */
        public void SetSortOrder(bool[] ascending) {
            PdfObject o = (PdfObject)Get(PdfName.S);
            if (o is PdfArray) {
                if (((PdfArray)o).Size != ascending.Length) {
                    throw new InvalidOperationException(MessageLocalization.GetComposedMessage("the.number.of.booleans.in.this.array.doesn.t.correspond.with.the.number.of.fields"));
                }
                PdfArray array = new PdfArray();
                for (int i = 0; i < ascending.Length; i++) {
                    array.Add(new PdfBoolean(ascending[i]));
                }
                Put(PdfName.A, array);
            }
            else {
                throw new InvalidOperationException(MessageLocalization.GetComposedMessage("you.need.a.single.bool.for.this.collection.sort.dictionary"));
            }
        }
    }
}
