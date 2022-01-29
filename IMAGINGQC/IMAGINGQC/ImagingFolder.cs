using System;
using System.Collections.Generic;

namespace IMAGINGQC
{
	class ImagingFolder
	{
		private readonly string _path;
		public string Path { get { return _path; } }

		public DateTime OldestFileCreateDate { get; set; }

		public ImagingFolder(string path)
		{
			_path = path;
			OldestFileCreateDate = DateTime.Now.Date;
		}
	}//class
}//namespace
