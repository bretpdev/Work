using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SasCoordinator
{
    public class Coordinator
    {
        public string SasFileLocation { get; private set; }
        public SasTunnelHelper TunnelHelper { get; private set; }
        public SasSoftwareHelper SoftwareHelper{get; private set;}
        public SasRegion Region { get; private set; }
        /// <summary>
        /// Construct a Coordinator using default Helper settings.  A null username and password indicate a Local-Only job
        /// </summary>
        public Coordinator(string username, string password, string sysParm, string sasFileLocation, SasRegion region, ProcessLogData data)
            : this(sasFileLocation, region, new SasTunnelHelper(username, password, sysParm, DataAccessHelper.TestMode, new SasSoftwareHelper(), data), new SasSoftwareHelper())
        {
        }

        /// <summary>
        /// Construct a Coordinator using manual Helper settings.
        /// </summary>
        public Coordinator(string sasFileLocation, SasRegion region, SasTunnelHelper tunnelHelper, SasSoftwareHelper softwareHelper)
        {
            this.SasFileLocation = sasFileLocation;
            this.SoftwareHelper = softwareHelper;
            this.TunnelHelper = tunnelHelper;
            this.Region = region;
            InitializationErrorCheck();
        }
        
        public void Coordinate()
        {
            TunnelHelper.OpenTunnel(Region);

            bool success = TunnelHelper.ExecuteScript(SasFileLocation);

            TunnelHelper.CloseTunnel();

            if (!success)
                throw new SasNotExecutedSuccessfullyException();
        }

        public class SasNotExecutedSuccessfullyException : Exception {}

        #region Errors
        public bool HasInitializationErrors { get { return InitializationErrors.Any(); } }
        public List<string> InitializationErrors { get; private set; }
        private void InitError(string error, params object[] formatArgs)
        {
            InitializationErrors.Add(string.Format(error, formatArgs));
        }
        private void InitializationErrorCheck()
        {
            InitializationErrors = new List<string>();
            AesLinkCheck();
            SasFileCheck();
            SoftwareHelper.ClearOldTempDirectoryFiles();
        }
        private void AesLinkCheck()
        {
            if (!File.Exists(SoftwareHelper.AesLinkLocation))
                InitError("Couldn't find AES Link Software at {0}", SoftwareHelper.AesLinkLocation);
        }
        private void SasFileCheck()
        {
            if (!File.Exists(SasFileLocation))
                InitError("The given Sas File Location does not exist: {0}", SasFileLocation);
        }
        #endregion
    }
}
