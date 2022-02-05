using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsBankImport
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Read all the lines from a Stream.
        /// </summary>
        public static IEnumerable<string> ReadAllLines(this Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return line;
            }
        }
        /// <summary>
        /// Returns and removes the first item from the given list.
        /// </summary>
        public static T Pop<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new IndexOutOfRangeException();
            var item = list.First();
            list.RemoveAt(0);
            return item;
        }
        /// <summary>
        /// Read all the lines in a given ZipEntry file.
        /// </summary>
        public static List<string> ReadAllLines(this ZipEntry zip)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                zip.Extract(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ReadAllLines().ToList();
            }
        }
    }
}
