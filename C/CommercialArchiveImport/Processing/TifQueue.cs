using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommercialArchiveImport
{
    public partial class MainProcessor
    {
        public class TifQueue
        {
            public List<Document> Tifs { get; set; }
            private MainProcessor Parent { get; set; }
            public TifQueue(List<Document> allDocs, int firstCount, MainProcessor parent)
            {
                Tifs = new List<Document>();
                this.Parent = parent;
                var first = allDocs[firstCount];
                if (!IsFirstTif(first.ImagePath))
                {
                    first.SetError("Found an out of place .T*** file.  Expected a .T001 file.", DocumentErrorType.BadTif);
                    Tifs.Add(first);
                    return;
                }
                for (int i = firstCount; i < allDocs.Count; i++)
                {
                    Document tif = allDocs[i];
                    Parent.ValidateDocument(tif);
                    if (!tif.ImagePath.EndsWith("T" + (i - firstCount + 1).ToString("d3")))
                        break;
                    Tifs.Add(tif);
                    if (!tif.Invalid)
                        CopyFile(tif, Path.Combine(Parent.ResultsLocation, NewName(tif.ImagePath)));

                    ProgressHelper.Increment();
                }
                if (!Tifs.Any(t => t.Invalid))
                    Parent.CreateCTLFile(Tifs.First(), Tifs.Select(o => NewName(o.ImagePath)));
                else
                    foreach (var t in Tifs.Where(t => !t.Invalid))
                        t.SetError("Couldn't copy multi-row tiff due to an error on another row.", DocumentErrorType.BadTif);
            }

            private bool CopyFile(Document doc, string newPath)
            {
                var orig = Parent.Zip.Find(doc.ImagePath);
                var bytes = orig.GetBytes();
                using (FileStream fs = File.Create(newPath))
                    fs.Write(bytes, 0, bytes.Length);
                lock (Parent.Zip)
                {
                    Parent.Zip.RemoveEntry(orig);
                }
                return true;
            }

            static string match = @"(.*)\.T([0-9][0-9][0-9])";
            public string NewName(string path)
            {
                return Regex.Replace(path, match, @"${1}_${2}.tif");
            }

            public static bool IsTif(string path)
            {
                if (string.IsNullOrEmpty(path)) return false;
                return Regex.IsMatch(path, match);
            }

            public static bool IsFirstTif(string path)
            {
                if (string.IsNullOrEmpty(path)) return false;
                return path.ToLower().EndsWith(".t001");
            }
        }
    }
}
