using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public abstract class BaseUserSelectedFlow
    {
        public string SSN { get; set; }

        public abstract void Process();
    }
}
