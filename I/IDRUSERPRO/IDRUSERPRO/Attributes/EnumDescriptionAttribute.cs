using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    class EnumDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public EnumDescriptionAttribute(string description) : base()
        {
            Description = description;
        }
    }
}
