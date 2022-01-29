using System;
using System.Diagnostics;
using Uheaa.Common.Scripts;

namespace ACURINTR.DemographicsProcessors
{
    class PdemRecovery : GeneralRecovery
	{
        /// <summary>
        /// Creates a facade for a RecoveryLog object, customized for the PDEM method of processing.
        /// </summary>
        /// <param name="recoveryLog">
		/// The RecoveryLog object from the script
		/// (normally exposed through the BatchScriptBase.Recovery property).
		/// </param>
        public PdemRecovery(RecoveryLog recoveryLog, DataAccess da) : base(recoveryLog, da)
        {
        }

		/// <summary>
		/// Deletes a PDEM record from the database and clears all recovery variables that are finer-grained than AccountNumber.
		/// </summary>
		/// <param name="task">QueueTask object representing the PDEM to be removed from the database.</param>
        public void Delete(QueueTask pdem)
        {
            //Delete the recovery record from BSYS.
            DA.DeletePendingPdemRecoveryRecord(pdem);
            //Set the AccountNumber property, which will clear out the Path, Phase, and Step properties.
            AccountNumber = AccountNumber;
        }
    }
}
