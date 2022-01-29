using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace E_Oscar
{
	class Program
	{
		static int Main(string[] args)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(ACDVTransactions));
			List<string> files = new List<string>();
			string destination = EnterpriseFileSystem.GetPath("ARCHIVE");
			foreach(string f in Directory.EnumerateFiles(EnterpriseFileSystem.FtpFolder).Where(x => x.EndsWith(".xml")))
			{
				files.Add(f);
			}

			foreach (string filename in files)
			{
				Stream reader = new FileStream(filename, FileMode.Open);
				ACDVTransactions test = (ACDVTransactions)serializer.Deserialize(reader);
				if (Path.GetFileName(filename).StartsWith("DF") && filename.EndsWith("manifest.xml")) //THIS IS AN IMAGE FILE
				{
					foreach (ACDVImageRecordType record in test.Items)
					{
						//CALL IMAGE PROCESSOR
						
					}
				}
				else if (Path.GetFileName(filename).StartsWith("DF")) //THIS IS A REQUEST FILE
				{
					foreach (ACDVRequestType record in test.Items)
					{
						//CALL REQUEST PROCESSOR
						
					}
				}
				else if (Path.GetFileName(filename).StartsWith("EO")) //THIS IS A RESPONSE FILE
				{
					foreach (ACDVResponseType record in test.Items)
					{
						//CALL ReSPOMSE PROCESSOR

					}
				}
				else
				{
					//TODO: Log unkmnown file error

				}
				reader.Dispose();
				reader.Close();
				File.Move(filename, destination);
			}
			return 0;
		}
	}
}
