using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;

public partial class UserDefinedFunctions
{
    [SqlFunction]
    public static SqlString SplitAndRemoveQuotes(string originalValue, string delimiter, int index, bool trim)
    {
        List<string> fields = new List<string>();
        bool withinQuotes = false;
        int startIndex = 0;

        for (int currentIndex = 0; currentIndex <= originalValue.Length - 1; currentIndex++)
        {
            if (originalValue.Substring(currentIndex, 1) == @"""")
            {
                withinQuotes = !withinQuotes;
                continue;
            }

            if (!withinQuotes && originalValue.Substring(currentIndex, 1) == delimiter)
            {
                if (trim)
                    fields.Add(originalValue.Substring(startIndex, currentIndex - startIndex).Replace(@"""", string.Empty).Trim());
                else
                    fields.Add(originalValue.Substring(startIndex, currentIndex - startIndex).Replace(@"""", string.Empty));

                startIndex = currentIndex + 1;
            }
        }

        if (trim)
            fields.Add(originalValue.Substring(startIndex).Replace(@"""", string.Empty).Trim());
        else
            fields.Add(originalValue.Substring(startIndex).Replace(@"""", string.Empty));
        return fields[index];
    }
}
