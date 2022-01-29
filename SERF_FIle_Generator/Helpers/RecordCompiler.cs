using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace SERF_File_Generator
{
    public static class RecordCompiler
    {
        public static string CompileRecord(string header, SerfFileBase data)
        {
            Dictionary<int, string> writeValues = new Dictionary<int, string>();
            foreach (PropertyInfo pi in data.GetType().GetProperties())
            {
                if (pi.HasAttribute<Ignore>())
                    continue;

                if (pi.PropertyType.Namespace == "System.Collections.Generic")
                {
                    dynamic temp = pi.GetValue(data, null);
                    if (temp == null)
                        continue;

                    int offset = 0;
                    for (int index = 0; index < temp.Count; index++)
                    {
                        foreach (PropertyInfo lpi in temp[index].GetType().GetProperties())
                        {
                            var pos1 = pi.GetCustomAttribute<LinePosAttribute>();
                            int length1 = lpi.GetCustomAttribute<LengthAttribute>().IsNull(o => o.Length);
                            var formatAttribute1 = lpi.GetCustomAttribute<FormatCodeAttribute>();

                            string val = (lpi.GetValue(temp[index]) ?? "").ToString();
                            try
                            {
                                val = formatAttribute1.FormatValue(val, length1);
                            }
                            catch (InvalidFormatCodeLengthException ex)
                            {
                                Console.WriteLine("Retrieved value {0} of length {1} for field {2} of class {3}.  Expected length of {4}", val, val.Length, pi.Name, data.GetType().Name, length1);
                                Console.WriteLine("Press any key to exit.");
                                Console.ReadKey();
                                Environment.Exit(0);
                            }
                            catch (InvalidFormatDateException ex)
                            {
                                Console.WriteLine("Retrieved invalid date of 01/01/1900 for field {1} of class {2}", pi.Name, data.GetType().Name);
                                Console.WriteLine("Press any key to exit.");
                                Console.ReadKey();
                                Environment.Exit(0);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Retrieved value {0} of length {1} for field {2} of class {3}.  Expected length of {4}", val, val.Length, pi.Name, data.GetType().Name, length1);
                                Console.WriteLine("Press any key to exit.");
                                Console.ReadKey();
                                Environment.Exit(0);
                            }

                            if (pos1 != null)
                            {
                                foreach (int position in pos1.Positions)
                                {
                                    writeValues[((position + offset) - 1)] = val;
                                    offset += length1;
                                }
                            }
                        }
                    }

                    //Move to the next property.
                    continue;
                }

                string value = (pi.GetValue(data) ?? "").ToString();
                var formatAttribute = pi.GetCustomAttribute<FormatCodeAttribute>();
                int length = pi.GetCustomAttribute<LengthAttribute>().IsNull(o => o.Length);
                try
                {
                    value = formatAttribute.FormatValue(value, length);
                }
                catch (InvalidFormatCodeLengthException ex)
                {
                    Console.WriteLine("Retrieved value {0} of length {1} for field {2} of class {3}.  Expected length of {4}", value, value.Length, pi.Name, data.GetType().Name, length);
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                catch (InvalidFormatDateException ex)
                {
                    Console.WriteLine("Retrieved invalid date of 01/01/1900 for field {1} of class {2}", pi.Name, data.GetType().Name);
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Retrieved value {0} of length {1} for field {2} of class {3}.  Expected length of {4}", value, value.Length, pi.Name, data.GetType().Name, length);
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                var pos = pi.GetCustomAttribute<LinePosAttribute>();
                if (pos != null)
                    foreach (int position in pos.Positions)
                        writeValues[position - 1] = value;
            }

            return GenerateLine(header, writeValues);
        }

        private static string GenerateLine(string header, Dictionary<int, string> writeValues)
        {
            StringBuilder line = new StringBuilder();
            line.Append(header);
            for (int i = header.Length; 3075 > i; i++)
            {
                if (writeValues.ContainsKey(i))
                {
                    string val = writeValues[i];
                    writeValues.Remove(i);
                    line.Append(val);
                    i += val.Length - 1;
                }
                else
                    line.Append(' ');
            }

            return line.ToString();
        }
    }
}
