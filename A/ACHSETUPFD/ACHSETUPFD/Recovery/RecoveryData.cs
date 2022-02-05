using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACHSETUPFD
{
    class RecoveryData
    {

        public UserProvidedMainMenuData.UserSelectedACHAction UserSelectedOption { get; set; }
        public string SSNOrAccountNumber { get; set; }
        public string FirstName { get; set; }
        public List<RecoveryProcessor.RecoveryPhases> Phases { get; set; }

        public RecoveryData()
        {
            Phases = new List<RecoveryProcessor.RecoveryPhases>();
        }

    }
}
