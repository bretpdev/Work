using System;
using System.Collections.Generic;
/*
 * $Id: CssFileImpl.java 61 2011-05-16 00:09:30Z redlab_b $
 *
 * This file is part of the iText (R) project.
 * Copyright (c) 1998-2015 iText Group NV
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
using iTextSharp.tool.xml.css.parser;

namespace iTextSharp.tool.xml.css {

    /**
     * Implementation of CssFile, the CSS is stored in a map.
     * @author redlab_b
     *
     */
    public class CssFileImpl : ICssFile {

        private IList<CssRule> rules;
        private bool persistent;

        /**
         * Constructs a new CssFileImpl.
         */
        public CssFileImpl() {
            persistent = false;
            rules = new List<CssRule>();
        }

        /*
         * (non-Javadoc)
         *
         * @see com.itextpdf.tool.xml.css.CssFile#add(java.lang.String,
         * java.util.Map)
         */
        virtual public bool Add(String selector, IDictionary<String, String> props) {
            IList<ICssSelectorItem> selectorItems = CssSelectorParser.CreateCssSelector(selector);
            if (selectorItems != null) {
                rules.Add(new CssRule(selectorItems, props));
                return true;
            }
            return false;
        }

        public virtual IList<CssRule> Get(Tag t) {
            IList<CssRule> result = new List<CssRule>();
            foreach (CssRule rule in rules) {
                if (rule.Selector.Matches(t))
                    result.Add(rule);
            }
            return result;
        }

        /* (non-Javadoc)
         * @see com.itextpdf.tool.xml.css.CssFile#isPersistent()
         */
        virtual public bool IsPersistent() {
            return persistent;
        }

        /**
         * @param isPeristent
         */
        virtual public void IsPersistent(bool isPeristent) {
            this.persistent = isPeristent;
        }
    }
}