using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace NSLDSCONSO
{
    public class NameParser
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string MiddleInitial { get; private set; }

        public NameParser(string name)
        {
            try
            {
                name = name.Trim(' ', ',');
                if (name.Contains(","))
                {
                    //parse from format LASTNAME, FIRSTNAME M
                    var split = name.Split(',').Select(o => o.Trim()).Where(o => !string.IsNullOrEmpty(o)).ToArray();
                    LastName = split[0].Trim();
                    var firstAndInitial = split[1].Trim().Split(' ');
                    FirstName = firstAndInitial[0];
                    if (firstAndInitial.Length > 1)
                        MiddleInitial = firstAndInitial.Last().ToString();
                }
                else
                {
                    var parts = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 3 && parts[1].Length == 1)
                    {   //parse from format FIRSTNAME M LASTNAME
                        FirstName = parts[0];
                        MiddleInitial = parts[1];
                        LastName = parts[2];
                    }
                    else if (parts.Length > 2)
                    {
                        FirstName = parts[0];
                        LastName = string.Join(" ", parts.Skip(1));
                    }
                    else if (parts.Length == 2)
                    {
                        FirstName = parts[0];
                        LastName = parts[1];
                    }
                    else
                    {
                        FirstName = name;
                    }
                }
            }
            catch (Exception)
            {
                FirstName = name.Trim();
            }
        }
    }

}
