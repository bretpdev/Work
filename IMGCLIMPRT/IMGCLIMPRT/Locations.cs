using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IMGCLIMPRT
{
    /// <summary>
    /// Handles the Test/Live versions of locations used in this app.
    /// </summary>
    public class Locations
    {
        /// <summary>
        /// The location of the folder where images will be picked up by the imaging system.
        /// </summary>
        public string ImageLocation { get { return EnterpriseFileSystem.GetPath("IMGCLIMPRT_Image"); } }
        /// <summary>
        /// The location of the folder where pending zip files are waiting to be processed.
        /// </summary>
        public string ZipLocation { get { return EnterpriseFileSystem.GetPath("IMGCLIMPRT_Zip"); } }
        /// <summary>
        /// The location where completed zip files are moved to.
        /// </summary>
        public string ArchiveLocation { get { return EnterpriseFileSystem.GetPath("IMGCLIMPRT_Archive"); } }
    }
}
