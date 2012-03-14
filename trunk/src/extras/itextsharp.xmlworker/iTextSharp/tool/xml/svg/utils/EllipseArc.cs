/*
 * $Id: $
 *
 * This file is part of the iText (R) project.
 * Copyright (c) 1998-2012 1T3XT BVBA
 * Authors: VVB, Bruno Lowagie, et al.
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
 * a covered work must retain the producer line in every PDF that is created
 * or manipulated using iText.
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

using System;
using iTextSharp.tool.xml.svg.exceptions;

namespace iTextSharp.tool.xml.svg.utils {

    public class EllipseArc
    {
        //TODO write an test with all the possible cases
        //TODO check all the doubles and floats

        //creates an arc defined by two points and the two axes of the ellipse
        private float cx, cy, a, b, startAng, extend;

        public float Cx
        {
            get { return cx; }
        }

        public float Cy
        {
            get { return cy; }
        }

        public float A
        {
            get { return a; }
        }

        public float B
        {
            get { return b; }
        }

        public float StartAng
        {
            get { return startAng; }
        }

        public float Extend
        {
            get { return extend; }
        }

        public static EllipseArc CreateEllipseArc(float x1, float y1, float x2, float y2, float a, float b, float sweep, float largeArc)
        {
            return GetEllipse(x1, y1, x2, y2, a, b, (int)sweep, (int)largeArc);
        }

        private static double CalculateAngle(double x, double y, double cx, double cy, double a, double b)
        {
            double result = Math.Pow(((x - cx) / a), 2.0) + Math.Pow(((y - cy) / b), 2.0);

            double cos = (x - cx) / a;
            double sin = (y - cy) / b;
            if (cos >= 0 && sin >= 0)
            {
                result = Math.Acos(cos) * 180 / Math.PI; //I 
            }
            if (cos >= 0 && sin < 0)
            {
                result = 360 - Math.Acos(cos) * 180 / Math.PI; //IV
            }
            if (cos < 0 && sin >= 0)
            {
                result = Math.Acos(cos) * 180 / Math.PI; // II
            }
            if (cos < 0 && sin < 0)
            {
                result = 360 - Math.Acos(cos) * 180 / Math.PI; //III
            }
            return result;
        }

        private static EllipseArc GetEllipse(float x1, float y1, float x2, float y2, float a, float b, int sweep, int largeArc)
        {
            double r1 = (x1 - x2) / (-2.0 * a);
            double r2 = (y1 - y2) / (2.0 * b);

            if (Math.Sqrt(r1 * r1 + r2 * r2) > 1)
            {
                throw new EllipseArcException("the two given points are not on the ellipse");
            }

            double tussen1 = Math.Asin(Math.Sqrt(r1 * r1 + r2 * r2));
            double tussen2 = Math.Atan(r1 / r2);

            EllipseArc result = CalculatePossibleMiddle(x1, y1, x2, y2, a, b, tussen1, tussen2, sweep, largeArc);
            if (result != null) return result;
            result = CalculatePossibleMiddle(x1, y1, x2, y2, a, b, Math.PI - tussen1, tussen2, sweep, largeArc);
            if (result != null) return result;
            result = CalculatePossibleMiddle(x1, y1, x2, y2, a, b, tussen1, tussen2 + Math.PI, sweep, largeArc);
            if (result != null) return result;
            result = CalculatePossibleMiddle(x1, y1, x2, y2, a, b, Math.PI - tussen1, tussen2 + Math.PI, sweep, largeArc);
            if (result != null) return result;
            return null;
        }

        //TODO check all the boundary cases
        private static EllipseArc CalculatePossibleMiddle(double x1, double y1, double x2, double y2, double a, double b, double t1, double t2, int sweep, int largeArc)
        {
            float[] result = new float[2];

            double x0 = x1 - a * Math.Cos(t1 + t2);
            double y0 = y1 - b * Math.Sin(t1 + t2);

            double check = (Math.Pow(((x2 - x0) / a), 2) + Math.Pow(((y2 - y0) / b), 2));
            //System.out.Println(result);
            if (check - 1 < 0.0000000001)
            {
                result[0] = (float)x0;
                result[1] = (float)y0;

                double theta1 = CalculateAngle(x1, y1, x0, y0, a, b);
                double theta2 = CalculateAngle(x2, y2, x0, y0, a, b);
                double startAngl = 0;
                double extend = 0;

                //both points are on the ellipse, but is this the middle, looked for?
                if (largeArc == 1)
                { //turn more than 180 degrees
                    if ((theta2 > theta1) && (theta2 - theta1 > 180) && sweep == 1)
                    {
                        startAngl = theta1;
                        extend = theta2 - theta1;
                    }
                    if ((theta1 > theta2) && (theta1 - theta2 <= 180) && sweep == 1)
                    {
                        startAngl = theta1;
                        extend = 360 - theta1 + theta2;
                    }
                    if ((theta2 > theta1) && (theta2 - theta1 <= 180) && sweep == 0)
                    {
                        startAngl = theta2;
                        extend = 360 - theta2 + theta1; //or the same extend but negative and start at p1
                    }
                    if ((theta1 > theta2) && (theta1 - theta2 > 180) && sweep == 0)
                    {
                        startAngl = theta2;
                        extend = theta1 - theta2;
                    }
                }
                else
                {
                    if ((theta2 > theta1) && (theta2 - theta1 <= 180) && sweep == 1)
                    {
                        startAngl = theta1;
                        extend = theta2 - theta1;
                    }
                    if ((theta1 > theta2) && (theta1 - theta2 > 180) && sweep == 1)
                    {
                        startAngl = theta1;
                        extend = 360 - theta1 + theta2;
                    }
                    if ((theta2 > theta1) && (theta2 - theta1 > 180) && sweep == 0)
                    {
                        startAngl = theta2;
                        extend = 360 - theta2 + theta1; //or the same extend but negative and start at p1
                    }
                    if ((theta1 > theta2) && (theta1 - theta2 <= 180) && sweep == 0)
                    {
                        startAngl = theta2;
                        extend = theta1 - theta2;
                    }
                }

                if (startAngl >= 0 && extend > 0)
                {
                    return new EllipseArc((float)x0, (float)y0, (float)a, (float)b, (float)startAngl, (float)extend);
                }
            }
            return null;
        }

        protected EllipseArc(float cx, float cy, float a, float b, float startAng, float extend)
        {
            this.cx = cx;
            this.cy = cy;
            this.a = a;
            this.b = b;
            this.startAng = startAng;
            this.extend = extend;
        }
    }
}
