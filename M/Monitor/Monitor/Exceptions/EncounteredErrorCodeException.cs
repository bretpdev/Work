﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    class EncounteredErrorCodeException : Exception
    {
        public EncounteredErrorCodeException(string message) : base(message) { }
    }
}