﻿using System;

namespace Uheaa.Common.CommonScreens
{
    /// <summary>
    /// Properties with this attribute will be used during the CSV parse, and other untagged properties will be ignored.
    /// </summary>
    public class CsvRequiredAttribute : Attribute { }
}
