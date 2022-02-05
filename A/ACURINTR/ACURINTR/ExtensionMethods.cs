using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ACURINTR
{
	static class ExtensionMethods
	{
		public static List<string> GetNumericGroups(this string text)
		{
			List<string> numericGroups = new List<string>();
			StringBuilder numberBuilder = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				if (Regex.IsMatch(text[i].ToString(), @"\d"))
				{
					numberBuilder.Append(text[i]);
				}
				else if (numberBuilder.Length > 0)
				{
					numericGroups.Add(numberBuilder.ToString());
					numberBuilder = new StringBuilder();
				}
			}
			if (numberBuilder.Length > 0)
			{
				numericGroups.Add(numberBuilder.ToString());
			}
			return numericGroups;
		}
	}
}
