using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.Scripts;

namespace THRDPAURES
{
    public class DemographicsBase
    {
        private ReflectionInterface RI;

        public DemographicsBase() { }

        public DemographicsBase(ReflectionInterface ri)
        {
            RI = ri;
        }

        /// <summary>
        /// GetText but removes underscores.
        /// </summary>
        /// <param name="row">Session row.</param>
        /// <param name="column">Session Column.</param>
        /// <param name="length">Length of Text.</param>
        /// <returns></returns>
        protected string GetTextRemoveUnderscore(int row, int column, int length)
        {
            //To keep this method from break if the default constructor is used.
            if (RI == null)
                return "";

            return RI.GetText(row, column, length).Replace("_", "");
        }
    }
}
