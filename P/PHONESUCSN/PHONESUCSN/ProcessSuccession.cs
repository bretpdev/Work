using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using static System.Console;

namespace PHONESUCSN
{
    public class ProcessSuccession
    {
        public UpdateHelper Helper { get; set; }
        public enum ToType { H, A, W }

        public ProcessSuccession(UpdateHelper helper, int count)
        {
            Helper = helper;
            WriteLine("Loading Succession data into the PhoneSuccession table in ULS");
            Helper.DA.LoadSuccessionDataEndorser();
            Helper.Data = Helper.DA.GetSuccessionData(count);
        }

        public void Process()
        {
            WriteLine($"Processing phone succession, count: {Helper.Data.Count}");
            UpdateHomewithAltPhone(Helper.Data.Where(p => !p.HomeIsValid && p.AltIsValid).ToList());
            UpdateHomeWithWorkPhone(Helper.Data.Where(p => !p.HomeIsValid && !p.AltIsValid && p.WorkIsValid).ToList());
            UpdateAltWithWorkPhone(Helper.Data.Where(p => p.HomeIsValid && !p.AltIsValid && p.WorkIsValid).ToList());
        }

        private void UpdateHomewithAltPhone(List<PhoneData> pData)
        {
            WriteLine($"Updating home phone with alt phone, count: {pData.Count}");
            foreach (PhoneData data in pData)
            {
                UpdateData altData = GetAltData(data, ToType.H);
                if (Helper.UpdatePhone(altData)) //Only add the comment if the phone was updated and the invalid phone is moved up
                {
                    string comment = $"Updated invalid home phone with a valid alt phone. Moved invalid home phone to alt phone for {(data.IsEndorser ? $"endorser" : "borrower")}: {data.Ssn}.";
                    Helper.AddComments(altData, comment);

                    UpdateData homeData = GetHomeData(data, ToType.A);
                    if (Helper.MoveInvalidPhone(homeData))
                    {
                        //If the valid work phone was moved to home and home was moved to Alt but Alt is not empty, move the invalid alt to work
                        if (data.WorkIsValid)
                        {
                            UpdateData workData = GetWorkData(data, ToType.A);
                            if (Helper.MoveInvalidPhone(workData))
                            {
                                homeData = GetHomeData(data, ToType.W);
                                Helper.MoveInvalidPhone(homeData);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateHomeWithWorkPhone(List<PhoneData> pData)
        {
            WriteLine($"Updating home phone with work phone, count: {pData.Count}");
            foreach (PhoneData data in pData)
            {
                UpdateData workData = GetWorkData(data, ToType.H);
                if (Helper.UpdatePhone(workData))
                {
                    string comment = $"Updated invalid home phone with a valid work phone. Moved invalid home phone to work phone for {(data.IsEndorser ? $"endorser" : "borrower")}: {data.Ssn}.";
                    Helper.AddComments(workData, comment);

                    UpdateData homeData = GetHomeData(data, ToType.A);
                    if (Helper.MoveInvalidPhone(homeData))
                    {
                        if (data.AltPhone.IsPopulated())
                        {
                            //If the valid work phone was moved to home and home was moved to Alt but Alt is not empty, move the invalid alt to work
                            UpdateData altData = GetAltData(data, ToType.W);
                            Helper.MoveInvalidPhone(altData);
                        }
                        else
                            Helper.Invalidate(workData);
                    }
                }
            }
        }


        // <summary>
        // Update the alt phone number with the work phone after updating the home phone with the alt phone.
        // </summary>
        private void UpdateAltWithWorkPhone(List<PhoneData> pData)
        {
            WriteLine($"Updating alt phone with work phone, count: {pData.Count}");
            foreach (PhoneData data in pData)
            {
                UpdateData workData = GetWorkData(data, ToType.A);
                if (Helper.UpdatePhone(workData))
                {
                    string comment = $"Updated invalid alt phone with a valid work phone. Moved invalid alt phone to work phone for {(data.IsEndorser ? $"endorser" : "borrower")}: {data.Ssn}.";
                    Helper.AddComments(workData, comment);

                    UpdateData altData = GetAltData(data, ToType.W);
                    Helper.UpdatePhone(altData);
                }
            }
        }

        /// <summary>
        /// Sets up the home data to send into the new phone type
        /// </summary>
        private UpdateData GetHomeData(PhoneData data, ToType type)
        {
            return new UpdateData()
            {
                PhoneSuccessionId = data.PhoneSuccessionId,
                Ssn = data.Ssn,
                Phone = data.HomePhone,
                Ext = data.HomeExt,
                Src = data.HomeSrc,
                Ind = data.HomeInd,
                Consent = data.HomeConsent,
                IsValid = data.HomeIsValid ? "Y" : "N",
                IsForeign = data.HomeIsForeign,
                VerifiedDate = data.HomeVerifiedDate.Value,
                FromType = "H",
                ToType = type.ToString(),
                IsEndorser = data.IsEndorser
            };
        }

        /// <summary>
        /// Sets up the alt data to send into the new phone type
        /// </summary>
        private UpdateData GetAltData(PhoneData data, ToType type)
        {
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
                IsForeign = data.AltIsForeign,
                VerifiedDate = data.AltVerifiedDate.Value,
                FromType = "A",
                ToType = type.ToString(),
                IsEndorser = data.IsEndorser
            };
        }


        /// <summary>
        /// Sets up the work data to send into the new phone type
        /// </summary>
        private UpdateData GetWorkData(PhoneData data, ToType type)
        {
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
                IsForeign = data.WorkIsForeign,
                VerifiedDate = data.WorkVerifiedDate.Value,
                FromType = "W",
                ToType = type.ToString(),
                IsEndorser = data.IsEndorser
            };
        }
    }
}