using static System.Console;

namespace PHONESUCSN
{
    public class InvalidateDuplicate
    {
        public UpdateHelper Helper { get; set; }

        public InvalidateDuplicate(UpdateHelper helper, int count)
        {
            Helper = helper;
            WriteLine("Loading duplicate data to invalidate into the PhoneSuccession table in ULS");
            Helper.DA.LoadDuplicateDataEndorser();
            Helper.Data = Helper.DA.GetDuplicateData(count);
        }

        public void Process()
        {
            WriteLine($"Invalidating duplicate numbers, count: {Helper.Data.Count}");

            foreach (PhoneData data in Helper.Data)
            {
                if (data.HomeIsValid && data.AltIsValid && (data.HomePhone == data.AltPhone && data.HomeExt == data.AltExt))
                {
                    UpdateData uData = SetupAltData(data);
                    if (Helper.Invalidate(uData))
                    {
                        string comment = $"Invalidated duplicate alt phone that matched home phone for {(data.IsEndorser ? $"endorser": "borrower")}: {uData.Ssn}.";
                        Helper.AddComments(uData, comment);
                    }
                }
                if (data.HomeIsValid && data.WorkIsValid && (data.HomePhone == data.WorkPhone && data.HomeExt == data.WorkExt))
                {
                    UpdateData uData = SetupWorkData(data);
                    if (Helper.Invalidate(uData))
                    {
                        string comment = $"Invalidated duplicate work phone that matched home phone for {(data.IsEndorser ? $"endorser" : "borrower")}: {uData.Ssn}.";
                        Helper.AddComments(uData, comment);
                    }
                }
                if (data.AltIsValid && data.WorkIsValid && (data.AltPhone == data.WorkPhone && data.AltExt == data.WorkExt))
                {
                    UpdateData uData = SetupWorkData(data);
                    if (Helper.Invalidate(uData))
                    {
                        string comment = $"Invalidated duplicate work phone that matched alt phone for {(data.IsEndorser ? $"endorser" : "borrower")}: {uData.Ssn}.";
                        Helper.AddComments(uData, comment);
                    }
                }
            }
        }

        private UpdateData SetupAltData(PhoneData data)
        {
            WriteLine("Invalidating alt phone number that matches valid home phone.");
            return new UpdateData()
            {
                PhoneSuccessionId = data.PhoneSuccessionId,
                Ssn = data.Ssn,
                Phone = data.AltPhone,
                Ext = data.AltExt,
                Src = data.AltSrc,
                Ind = data.AltInd,
                Consent = data.AltConsent,
                IsValid = data.AltIsValid ? "Y" : "N",
                VerifiedDate = data.AltVerifiedDate.Value,
                FromType = "A",
                IsEndorser = data.IsEndorser
            };
        }

        private UpdateData SetupWorkData(PhoneData data)
        {
            WriteLine("Invalidating work phone number that matches valid home phone.");
            return new UpdateData()
            {
                PhoneSuccessionId = data.PhoneSuccessionId,
                Ssn = data.Ssn,
                Phone = data.WorkPhone,
                Ext = data.WorkExt,
                Src = data.WorkSrc,
                Ind = data.WorkInd,
                Consent = data.WorkConsent,
                IsValid = data.WorkIsValid ? "Y" : "N",
                VerifiedDate = data.WorkVerifiedDate.Value,
                FromType = "W",
                IsEndorser = data.IsEndorser
            };
        }
    }
}