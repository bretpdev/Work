using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsBankImport
{
    /// <summary>
    /// Handles the Test/Live versions of locations used in this app.
    /// </summary>
    public class Locations
    {
        private bool testMode;
        public Locations(bool testMode)
        {
            this.testMode = testMode;
        }

        const string testZipLocation = @"X:\PADD\US Bank\Test\";
        const string liveZipLocation = @"X:\PADD\US Bank\";
        /// <summary>
        /// The location of the folder where pending zip files are waiting to be processed.
        /// </summary>
        public string ZipLocation { get { return testMode ? testZipLocation : liveZipLocation; } }

        const string testImageLocation = @"\\imgdevkofax\ascent$\UTCROther_imp\";
        const string liveImageLocation = @"\\imgprodkofax\ascent$\UTCROther_imp\";
        /// <summary>
        /// The location of the folder where images will be picked up by the imaging system.
        /// </summary>
        public string ImageLocation { get { return testMode ? testImageLocation : liveImageLocation; } }

        const string testArchiveLocation = @"X:\Archive\US Bank\Processed\Test\";
        const string liveArchiveLocation = @"X:\Archive\US Bank\Processed\";
        /// <summary>
        /// The location where completed zip files are moved to.
        /// </summary>
        public string ArchiveLocation { get { return testMode ? testArchiveLocation : liveArchiveLocation; } }
    }
}
