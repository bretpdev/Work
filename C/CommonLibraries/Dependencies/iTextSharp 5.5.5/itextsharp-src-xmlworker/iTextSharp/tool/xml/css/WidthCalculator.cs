using System;
using System.Collections.Generic;
using System.util;
using iTextSharp.tool.xml.html;

/*
 * $Id: WidthCalculator.java 170 2011-06-08 10:06:43Z emielackermann $
 *
 * This file is part of the iText (R) project.
 * Copyright (c) 1998-2014 iText Group NV
 * Authors: Balder Van Camp, Emiel Ackermann, et al.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License version 3
 * as published by the Free Software Foundation with the addition of the
 * following permission added to Section 15 as permitted in Section 7(a):
 * FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
 * ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
 * OF THIRD PARTY RIGHTS
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
namespace iTextSharp.tool.xml.css {

    /**
     * @author Emiel Ackermann
     *
     */
    public class WidthCalculator {

        private CssUtils utils = CssUtils.GetInstance();

        private const float TOLERANCE = 0.00000000000001f;

        /**
         * Tries to calculate a width from a tag and it's ancestors.
         * @param tag the tag to use
         * @param rootTags the root tags,
         * @param pageWidth the page width
         * @return the width
         */
        public virtual float GetWidth(Tag tag, IList<String> rootTags, float pageWidth) {
            return GetWidth(tag, rootTags, pageWidth, -1);
        }

        public virtual float GetWidth(Tag tag, IList<String> rootTags, float pageWidth, float initialTotalWidth) {
            float width = 0;
            String widthValue;
            tag.CSS.TryGetValue(HTML.Attribute.WIDTH, out widthValue);
            if (widthValue == null) {
                tag.Attributes.TryGetValue(HTML.Attribute.WIDTH, out widthValue);
            }
            if (widthValue != null) {
                if (utils.IsNumericValue(widthValue) || utils.IsMetricValue(widthValue)) {
                    width = utils.ParsePxInCmMmPcToPt(widthValue);
                } else if (utils.IsRelativeValue(widthValue)) {
                    Tag ancestor = tag;
                    float firstAncestorsWidth = 0;
                    while (firstAncestorsWidth == 0 && ancestor.Parent != null) {
                        ancestor = ancestor.Parent;
                        firstAncestorsWidth = GetWidth(ancestor, rootTags, pageWidth, initialTotalWidth);
                    }
                    if (firstAncestorsWidth == 0) {
                        width = utils.ParseRelativeValue(widthValue, pageWidth);
                    } else {
                        width = utils.ParseRelativeValue(widthValue, firstAncestorsWidth);
                    }
                }
            } else if (rootTags.Contains(tag.Name)){
                if (Util.compare(initialTotalWidth, -1) == 0) {
                    width = pageWidth;
                } else {
                    width = initialTotalWidth;
                }
            }

            return width;
        }
    }
}
