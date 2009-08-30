using System;
using iTextSharp.text.error_messages;

/*
 * Copyright 2005 by Michael Niedermair.
 *
 * The contents of this file are subject to the Mozilla Public License Version 1.1
 * (the "License"); you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the License.
 *
 * The Original Code is 'iText, a free JAVA-PDF library'.
 *
 * The Initial Developer of the Original Code is Bruno Lowagie. Portions created by
 * the Initial Developer are Copyright (C) 1999, 2000, 2001, 2002 by Bruno Lowagie.
 * All Rights Reserved.
 * Co-Developer of the code is Paulo Soares. Portions created by the Co-Developer
 * are Copyright (C) 2000, 2001, 2002 by Paulo Soares. All Rights Reserved.
 *
 * Contributor(s): all the names of the contributors are added in the source code
 * where applicable.
 *
 * Alternatively, the contents of this file may be used under the terms of the
 * LGPL license (the "GNU LIBRARY GENERAL PUBLIC LICENSE"), in which case the
 * provisions of LGPL are applicable instead of those above.  If you wish to
 * allow use of your version of this file only under the terms of the LGPL
 * License and not to allow others to use your version of this file under
 * the MPL, indicate your decision by deleting the provisions above and
 * replace them with the notice and other provisions required by the LGPL.
 * If you do not delete the provisions above, a recipient may use your version
 * of this file under either the MPL or the GNU LIBRARY GENERAL PUBLIC LICENSE.
 *
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the MPL as stated above or under the terms of the GNU
 * Library General Public License as published by the Free Software Foundation;
 * either version 2 of the License, or any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU Library general Public License for more
 * details.
 *
 * If you didn't download this code from the following link, you should check if
 * you aren't using an obsolete version:
 * http://www.lowagie.com/iText/
 */

namespace iTextSharp.text {

    /**
    * Chapter with auto numbering.
    *
    * @author Michael Niedermair
    */
    public class ChapterAutoNumber : Chapter {

        /**
        * Is the chapter number already set?
        * @since	2.1.4
        */
        protected bool numberSet = false;

        /**
        * Create a new object.
        *
        * @param para     the Chapter title (as a <CODE>Paragraph</CODE>)
        */
        public ChapterAutoNumber(Paragraph para) : base(para, 0) {
        }

        /**
        * Create a new objet.
        * 
        * @param title     the Chapter title (as a <CODE>String</CODE>)
        */
        public ChapterAutoNumber(String title) : base(title, 0) {
        }

        /**
        * Create a new section for this chapter and ad it.
        *
        * @param title  the Section title (as a <CODE>String</CODE>)
        * @return Returns the new section.
        */
        public override Section AddSection(String title) {
    	    if (AddedCompletely) {
    		    throw new InvalidOperationException(MessageLocalization.GetComposedMessage("this.largeelement.has.already.been.added.to.the.document"));
    	    }
            return AddSection(title, 2);
        }

        /**
        * Create a new section for this chapter and add it.
        *
        * @param title  the Section title (as a <CODE>Paragraph</CODE>)
        * @return Returns the new section.
        */
        public override Section AddSection(Paragraph title) {
    	    if (AddedCompletely) {
    		    throw new InvalidOperationException(MessageLocalization.GetComposedMessage("this.largeelement.has.already.been.added.to.the.document"));
    	    }
            return AddSection(title, 2);
        }

        /**
        * Changes the Chapter number.
        * @param	number	the new chapter number
        * @since 2.1.4
        */
        public int SetAutomaticNumber(int number) {
    	    if (!numberSet) {
        	    number++;
        	    base.SetChapterNumber(number);
        	    numberSet = true;
    	    }
		    return number;
        }
    }
}
