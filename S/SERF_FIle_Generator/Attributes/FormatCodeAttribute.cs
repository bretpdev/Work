using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace SERF_File_Generator
{
    public class InvalidFormatDateException : Exception
    {
        public InvalidFormatDateException() : base("Date should not be 1/1/1900") { }
    }
    public class InvalidFormatCodeLengthException : Exception
    {
        public string Value { get; set; }
        public int Length { get; set; }
        public InvalidFormatCodeLengthException(string value, int length)
            : base(string.Format("Given value {0} was longer than expected length of {1}", value, length))
        {
            Value = value;
            Length = length;
        }
    }
    public class FormatCodeAttribute : Attribute
    {
        public string FormatCode { get; private set; }
        public string FormatValue(string value, int length)
        {

            value = value.Trim();
            switch (FormatCode)
            {
                case "X":
                case "I":
                case "M":
                case "C":
                case "F":
                    if (value.Length > length)
                        throw new InvalidFormatCodeLengthException(value, length);
                    return value.Trim().PadRight(length, ' ');
                case "A":
                case "N":
                case "R":
                case "S9":
                    if (!value.TrimStart('0').IsNullOrEmpty())
                        value = value.TrimStart('0');
                    if (value.Length > length)
                        throw new InvalidFormatCodeLengthException(value, length);
                    if (value.Trim().IsNullOrEmpty())
                        return value.PadLeft(length, ' ');

                    return value.PadLeft(length, '0');
                case "D":
                    if (string.IsNullOrEmpty(value))//Added this for dates that do not have values like email address validity date when no email address is populated.
                        return value.PadRight(length, ' ');
                    else if (value == "A")
                        return value.PadRight(length, ' ');
                    var date = DateTime.Parse(value).ToString("MM/dd/yyyy"); 
                    if (date == "01/01/1900")
                        throw new InvalidFormatDateException();
                    return date;
                case "T":
                    return DateTime.Parse(value).ToString("hh:mm:ss");
                default:
                    return value;
            }
        }
        public FormatCodeAttribute(string formatCode)
        {
            this.FormatCode = formatCode;
        }
    }
}
