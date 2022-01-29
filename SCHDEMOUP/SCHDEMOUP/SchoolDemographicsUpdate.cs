using System;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace SCHDEMOUP
{
    public class SchoolDemographicsUpdate : ScriptBase
    {
        DepartmentCodes _codes = new DepartmentCodes();
        UpdateDemographics _updates;
        string _departments = string.Empty;

        public SchoolDemographicsUpdate(ReflectionInterface ri)
            : base(ri, "SCHDEMOUP")
        {
        }

        public override void Main()
        {
            frmSchoolType type = new frmSchoolType(_codes);

            while (true)
            {
                DialogResult result = type.ShowDialog();
                ProcessingOption option = new ProcessingOption();
                frmDemoRun run = new frmDemoRun(option);

                if (result == DialogResult.Cancel) { EndDLLScript(); }
                else if (result == DialogResult.OK)
                {
                    LPSCSchoolData schoolData = new LPSCSchoolData();
                    if (CheckValidShoolCode(_codes))
                    {
                        if (_codes.Type != DepartmentCodes.UpdateType.General && _codes.Type != DepartmentCodes.UpdateType.All)
                        {
                            FastPath("LPSCI" + _codes.SchoolCode + _codes.OnelinkDepartment);
                        }
                        if (!Check4Text(1, 55, "INSTITUTION NAME/ID SEARCH"))
                        {
                            LoadFormFieldsAndLaunchForm(schoolData);
                            if (_codes.Type == DepartmentCodes.UpdateType.All)
                            {
                                FastPath("TX3Z/ITX0Y" + _codes.SchoolCode + "000");
                            }
                            else
                            {
                                FastPath("TX3Z/ITX0Y" + _codes.SchoolCode + _codes.CompassDepartment);
                            }
                            if (!Check4Text(1, 73, "TXX00"))
                            {
                                schoolData.First = GetText(16, 31, 10);
                                schoolData.Last = GetText(16, 56, 20);
                            }
                        }
                        frmDemoData data = new frmDemoData(TestModeProperty, schoolData, _codes);
                        DialogResult dataResult = data.ShowDialog();
                        ProcessDataResults(dataResult, run, option, schoolData, _codes);
                    }
                }
                else if (result == DialogResult.Yes)
                {
                    DialogResult runResult;
                    while ((runResult = run.ShowDialog()) != DialogResult.Cancel)
                    {
                        ProcessRunResults(runResult, option, new List<LPSCSchoolData>());
                    }
                    EndDLLScript();
                }
            }
        }

        /// <summary>
        /// Update the SchoolUpdates.txt file. If DialogResult is Yes, open frmDemoRun
        /// </summary>
        /// <param name="dataResult"></param>
        private void ProcessDataResults(DialogResult dataResult, frmDemoRun run, ProcessingOption option, LPSCSchoolData schoolData, DepartmentCodes codes)
        {
            List<LPSCSchoolData> dataObject = new List<LPSCSchoolData>();
            dataObject.Add(schoolData);
            if (dataResult == DialogResult.Cancel) { EndDLLScript(); }
            else if (dataResult == DialogResult.Yes)
            {
                DialogResult runResult;
                while ((runResult = run.ShowDialog()) != DialogResult.Cancel)
                {
                    ProcessRunResults(runResult, option, dataObject);
                }
                EndDLLScript();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runResult">Result of the frmDemoRun form</param>
        /// <param name="option"></param>
        /// <param name="dataObject"></param>
        private void ProcessRunResults(DialogResult runResult, ProcessingOption option, List<LPSCSchoolData> dataObject)
        {
            if (runResult == DialogResult.Cancel) { EndDLLScript(); }
            else if (runResult == DialogResult.OK)
            {
                switch (option.SelectedOption)
                {
                    case ProcessingOption.Option.Update:
                        Update(dataObject);
                        break;
                    case ProcessingOption.Option.Live:
                        Live(dataObject);
                        break;
                    case ProcessingOption.Option.Test:
                        Test(dataObject);
                        break;
                }
            }
        }

        /// <summary>
        /// Checks LPSCI to see if the school code entered exists in the system
        /// </summary>
        /// <param name="codes">School Code entered in frmSchoolType form</param>
        /// <returns>True if school exists</returns>
        private bool CheckValidShoolCode(DepartmentCodes codes)
        {
            FastPath("LPSCI" + codes.SchoolCode + "GEN");
            //Warn the user that the school code was not found and open the form again.
            if (Check4Text(1, 55, "INSTITUTION NAME/ID SEARCH"))
            {
                MessageBox.Show("You must enter a valid school code to proceed.", "Invalid School Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else { return true; }
        }

        /// <summary>
        /// Load schoolData object before passing it to the frmDemoData form
        /// </summary>
        /// <param name="schoolData"></param>
        private void LoadFormFieldsAndLaunchForm(LPSCSchoolData schoolData)
        {
            schoolData.SchoolName = GetText(5, 21, 40).Trim();
            schoolData.Address1 = GetText(8, 21, 40).Trim();
            schoolData.Address2 = GetText(9, 21, 40).Trim();
            schoolData.Address3 = GetText(10, 21, 40).Trim();
            schoolData.City = GetText(11, 21, 30).Trim();
            schoolData.Zip = GetText(11, 66, 17).Trim();
            if ((Check4Text(11, 59, string.Empty) && Check4Text(11, 59, "FC")) || Check4Text(15, 19, string.Empty) || Check4Text(15, 70, string.Empty))
            {
                schoolData.State = GetText(11, 59, 2);
                schoolData.Phone = GetText(15, 19, 10);
                schoolData.Extension = GetText(15, 34, 4);
                schoolData.Fax = GetText(15, 70, 10);
            }
            else
            {
                schoolData.ForeignState = GetText(12, 21, 15);
                schoolData.Country = GetText(12, 55, 25);
                schoolData.PhoneIC = GetText(16, 19, 3);
                schoolData.PhoneCNY = GetText(16, 22, 3);
                schoolData.PhoneCity = GetText(16, 25, 4);
                schoolData.PhoneLocal = GetText(16, 29, 7);
                schoolData.ForeignExtension = GetText(15, 34, 4);
                schoolData.FaxIC = GetText(16, 63, 3);
                schoolData.FaxCNY = GetText(16, 66, 3);
                schoolData.FaxCity = GetText(16, 69, 4);
                schoolData.FaxLocal = GetText(16, 73, 7);
            }
        }

        /// <summary>
        /// Updates both Compass and OneLink regions
        /// </summary>
        /// <param name="dataObject"></param>
        private void Update(List<LPSCSchoolData> dataObject)
        {
            if (dataObject.Count == 0)
            {
                MessageBox.Show("There are no updates to process.", "No Updates to Process", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (LPSCSchoolData data in dataObject)
            {
                //OneLink Updates
                _updates = new UpdateDemographics();
                //Update all departments
                if (data.SchoolCode == "All")
                {
                    UpdateOnlink("GEN", false, data, _updates);
                    UpdateOnlink("110", false, data, _updates);
                    UpdateOnlink("112", false, data, _updates);
                    UpdateOnlink("111", false, data, _updates);
                }
                else //Update specific department
                {
                    UpdateOnlink(data.OneLinkDepartment, true, data, _updates);
                }

                //add activity record if something changed
                if (_updates.NameUpdated || _updates.AddressUpdated || _updates.PhoneUpdated || _updates.FaxUpdated)
                {
                    string items = UpdatedItems(_updates);
                    FastPath("LP54A{0};{1};;;;{2}", data.SchoolCode, "001", "QMADD");
                    PutText(7, 2, "MS95");
                    PutText(11, 2, string.Format("{0} {1} for dept(s) {2} {3}", _updates.UpdateMode, items, _departments, "{SCHDEMOUP}"), Key.F6);
                }

                //Compass Updates
                _updates = new UpdateDemographics();
                if (data.CompassDepartment == "ALL")
                {
                    UpdateCompass("000", false, data, _updates);
                    UpdateCompass("004", false, data, _updates);
                    UpdateCompass("001", false, data, _updates);
                    UpdateCompass("003", false, data, _updates);
                }
                else
                {
                    UpdateCompass(data.CompassDepartment, false, data, _updates);
                }

                //add activity record if something changed
                if (_updates.NameUpdated || _updates.AddressUpdated || _updates.PhoneUpdated || _updates.FaxUpdated || _updates.ContactUpdated)
                {
                    string items = UpdatedItems(_updates);
                    FastPath("TX3Z/APO2X04{0}", data.SchoolCode);
                    PutText(8, 18, "0530020");
                    PutText(14, 2, string.Format("{0} {1} for dept(s) {3} {4}", _updates.UpdateMode, items, _departments, "{SCHDEMOUP}"), Key.F6);
                }
            }
            MessageBox.Show("OneLink and Compass have been successfully updated", "Updates Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataObject"></param>
        private void Live(List<LPSCSchoolData> dataObject)
        {
            Logon("Live");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataObject"></param>
        private void Test(List<LPSCSchoolData> dataObject)
        {
            Logon("Test");
        }

        /// <summary>
        /// Update OneLink
        /// </summary>
        /// <param name="department">Department Code to update with</param>
        /// <param name="addDept">True to add new department, False to use existing department</param>
        /// <param name="data">LPSCSchoolData object</param>
        private void UpdateOnlink(string department, bool addDept, LPSCSchoolData data, UpdateDemographics updates)
        {
            FastPath("LPSCC{0}{1}", data.SchoolCode, department);

            //switch to add mode and update text of update mode for comment if the dept isn't found
            if (Check4Text(1, 55, "INSTITUTION NAME/ID SEARCH"))
            {
                //Exit the function if the department should not be added
                if (!addDept) { return; }
                if (updates.UpdateMode == string.Empty)
                { updates.UpdateMode = "added"; }
                else if (updates.UpdateMode == "changed")
                { updates.UpdateMode = "added/changed"; }
                PutText(1, 7, "A", Key.Enter);
            }
            else
            {
                if (updates.UpdateMode == string.Empty)
                { updates.UpdateMode = "changed"; }
                else if (updates.UpdateMode == "added")
                { updates.UpdateMode = "added/changed"; }
            }

            string foreignPhone = data.PhoneIC + data.PhoneCNY + data.PhoneCity + data.PhoneLocal;
            string foreignFax = data.FaxIC + data.FaxCNY + data.FaxCity + data.FaxLocal;

            //Updates the fields to any new data and set addressUpdated bool to true if set
            if (!Check4Text(8, 21, data.Address1))
            {
                PutText(8, 21, data.Address1, true);
                updates.AddressUpdated = true;
            }
            if (!Check4Text(9, 21, data.Address2))
            {
                PutText(9, 21, data.Address2, true);
                updates.AddressUpdated = true;
            }
            if (!Check4Text(10, 21, data.Address3))
            {
                PutText(10, 21, data.Address3, true);
                updates.AddressUpdated = true;
            }
            if (!Check4Text(11, 21, data.City))
            {
                PutText(11, 21, data.City, true);
                updates.AddressUpdated = true;
            }
            if ((!Check4Text(11, 59, data.State) && !Check4Text(13, 23, "FC") && (data.State != string.Empty)))
            {
                PutText(11, 59, data.State, true);
                updates.AddressUpdated = true;
            }
            if (data.ForeignState != string.Empty || data.Country != string.Empty || foreignPhone != string.Empty || foreignFax != string.Empty || data.ForeignExtension != string.Empty)
            {
                PutText(11, 59, "FC", true);
                updates.AddressUpdated = true;
            }
            if (!Check4Text(11, 66, data.Zip))
            {
                PutText(11, 66, data.Zip, true);
                updates.AddressUpdated = true;
            }
            if (!Check4Text(12, 21, data.ForeignState))
            {
                PutText(12, 21, data.ForeignState, true);
                updates.AddressUpdated = true;
            }
            if (!Check4Text(12, 55, data.Country))
            {
                PutText(12, 55, data.Country, true);
                updates.AddressUpdated = true;
            }

            //Update the Phone information and set the phoneUpdated bool to true if changes are made
            if (data.Phone == string.Empty)
            {
                PutText(15, 19, string.Empty, true);
            }
            else if (!Check4Text(15, 19, data.Phone))
            {
                PutText(15, 19, data.Phone, true);
                updates.PhoneUpdated = true;
            }

            //Update the extention and set the phoneUpdated bool to true if changes are made
            if (data.Extension != string.Empty)
            {
                if (!Check4Text(15, 34, data.Extension))
                {
                    PutText(15, 34, data.Extension, true);
                    updates.PhoneUpdated = true;
                }
            }
            else if (data.ForeignExtension != string.Empty)
            {
                if (!Check4Text(15, 34, data.ForeignExtension))
                {
                    PutText(15, 34, data.ForeignExtension, true);
                    updates.PhoneUpdated = true;
                }
            }

            //Update the fax and set the phoneUpdated bool to true if changes are made
            if (data.Fax != string.Empty)
            {
                PutText(15, 70, string.Empty, true);
            }
            else if (!Check4Text(15, 70, data.Fax))
            {
                PutText(15, 70, data.Fax, true);
                updates.FaxUpdated = true;
            }

            //Update the foreign phone and fax and set the faxUpdated bool to true if changes are made
            if (!Check4Text(16, 19, foreignPhone))
            {
                PutText(16, 19, foreignPhone, true);
                updates.PhoneUpdated = true;
            }
            if (!Check4Text(16, 13, foreignFax))
            {
                PutText(16, 63, foreignFax, true);
                updates.FaxUpdated = true;
            }

            //Update school name if it has changed
            if (!Check4Text(5, 21, data.SchoolName) && updates.UpdateMode == "changed")
            {
                updates.NameUpdated = true;
                if (Check4Text(7, 15, "GEN"))
                {
                    PutText(15, 21, data.SchoolName, Key.Enter, true);
                    VerifyUpdate("OL");
                }
                else
                {
                    VerifyUpdate("OL");
                    FastPath("LPSCC{0}{1}", data.SchoolCode, "GEN");
                    PutText(5, 21, data.SchoolName, true);
                    VerifyUpdate("OL");
                }
            }
            else if (updates.AddressUpdated || updates.PhoneUpdated || updates.FaxUpdated)
            {
                VerifyUpdate("OL");
                if (_departments == string.Empty)
                {
                    _departments = department;
                }
                else
                {
                    _departments += ", " + department;
                }
            }
        }

        /// <summary>
        /// Update Compass
        /// </summary>
        /// <param name="department">Department Code to update with</param>
        /// <param name="addDept">True to add new department, False to use existing department</param>
        /// <param name="data">LPSCSchoolData object</param>
        private void UpdateCompass(string department, bool addDept, LPSCSchoolData data, UpdateDemographics updates)
        {
            FastPath("TX3Z/CTX0Y{0}{1}", data.SchoolCode, department);

            //Exit the function if the dept should not be added
            if (!addDept) { return; }
            //switch to add mode and update text of udpate mode for comment if the dept isn't found
            if (Check4Text(1, 73, "TXX00"))
            {
                PutText(1, 4, "A", Key.Enter);
            }
            if (updates.UpdateMode == string.Empty)
            {
                updates.UpdateMode = "changed";
            }
            else if (updates.UpdateMode == "added")
            {
                updates.UpdateMode = "added/changed";
            }

            //Get phone numbers from system since they are split
            string phone = GetText(18, 20, 3) + GetText(18, 26, 3) + GetText(18, 30, 4);
            string fax = GetText(19, 20, 3) + GetText(19, 26, 3) + GetText(19, 30, 4);
            string foreignPhone = GetText(20, 21, 3) + GetText(20, 34, 3) + GetText(20, 44, 4) + GetText(20, 56, 7);
            string foreignFax = GetText(21, 21, 3) + GetText(21, 34, 3) + GetText(21, 44, 4) + GetText(21, 56, 7);
            string foreignPhoneData = data.PhoneIC + data.PhoneCNY + data.PhoneCity + data.PhoneLocal;
            string foreignFaxData = data.FaxIC + data.FaxCNY + data.FaxCity + data.FaxLocal;

            if (!Check4Text(11, 23, data.Address1))
            {
                PutText(11, 23, data.Address1, true);
                _updates.AddressUpdated = true;
            }
            if (!Check4Text(12, 23, data.Address2))
            {
                PutText(12, 23, data.Address2, true);
                _updates.AddressUpdated = true;
            }
            if (!Check4Text(13, 23, data.Address3))
            {
                PutText(13, 23, data.Address3, true);
                _updates.AddressUpdated = true;
            }
            if (!Check4Text(14, 13, data.City))
            {
                PutText(13, 23, data.City, true);
                _updates.AddressUpdated = true;
            }
            if (!Check4Text(14, 53, data.State))
            {
                PutText(14, 53, data.State, true);
                _updates.AddressUpdated = true;
            }
            if (!Check4Text(14, 69, data.Zip))
            {
                PutText(14, 69, data.Zip, true);
                _updates.AddressUpdated = true;
            }
            if (!Check4Text(15, 21, data.ForeignState))
            {
                PutText(15, 21, data.ForeignState, true);
                _updates.AddressUpdated = true;
            }
            if (!Check4Text(15, 49, data.Country))
            {
                PutText(15, 49, data.Country, true);
                _updates.AddressUpdated = true;
            }

            //Update Contact
            if (!Check4Text(16, 31, data.First))
            {
                PutText(16, 31, data.First, true);
                _updates.ContactUpdated = true;
            }
            if (!Check4Text(16, 56, data.Last))
            {
                PutText(16, 56, data.Last, true);
                _updates.ContactUpdated = true;
            }

            //Update Phone
            if (data.Phone == string.Empty)
            {
                PutText(18, 20, string.Empty, true);
                PutText(18, 26, string.Empty, true);
                PutText(18, 30, string.Empty, true);
            }
            else if (phone != data.Phone)
            {
                PutText(18, 20, data.Phone);
                _updates.PhoneUpdated = true;
            }

            //Update Extension
            if (!Check4Text(18, 43, data.Extension))
            {
                PutText(18, 43, data.Extension, true);
                _updates.PhoneUpdated = true;
            }

            //Update Fax
            if (data.Fax == string.Empty)
            {
                PutText(19, 20, string.Empty, true);
                PutText(19, 26, string.Empty, true);
                PutText(19, 30, string.Empty, true);
            }
            else if (fax != data.Fax)
            {
                PutText(19, 20, data.Fax);
                _updates.FaxUpdated = true;
            }

            //Update Foreign Phone
            if (foreignPhoneData != foreignPhone)
            {
                PutText(20, 21, data.PhoneIC, true);
                PutText(20, 34, data.PhoneCNY, true);
                PutText(20, 44, data.PhoneCity, true);
                PutText(20, 55, data.PhoneLocal, true);
                _updates.PhoneUpdated = true;
            }
            if (!Check4Text(20, 75, data.ForeignExtension))
            {
                PutText(20, 75, data.ForeignExtension, true);
                _updates.PhoneUpdated = true;
            }

            //Update Foreign Fax
            if (foreignFaxData != foreignFax)
            {
                PutText(21, 21, data.FaxIC, true);
                PutText(21, 34, data.FaxCNY, true);
                PutText(21, 44, data.FaxCity, true);
                PutText(21, 56, data.FaxLocal, true);
                _updates.FaxUpdated = true;
            }

            PutText(22, 10, "Y"); //Validity
            PutText(22, 31, data.Date);

            //Update school name if changed
            if (!Check4Text(6, 19, data.SchoolName))
            {
                _updates.NameUpdated = true;
                if (Check4Text(8, 8, "000"))
                {
                    PutText(6, 19, data.SchoolName, true);
                    VerifyUpdate("CS");
                }
                else
                {
                    VerifyUpdate("CS");
                    FastPath("TX3Z/CTX0Y{0}{1}", data.SchoolCode, "000");
                    PutText(6, 19, data.SchoolName, true);
                    VerifyUpdate("CS");
                }
            }
            else if (_updates.AddressUpdated || _updates.PhoneUpdated || _updates.FaxUpdated || _updates.ContactUpdated)
            {
                VerifyUpdate("CS");
                if (_departments == string.Empty)
                {
                    _departments = department;
                }
                else
                {
                    _departments += ", " + department;
                }
            }
        }

        /// <summary>
        /// Checks to see if there was an error when writing to the session
        /// </summary>
        /// <param name="system">"OL" for OneLink and string.empty for Compass</param>
        private void VerifyUpdate(string system)
        {
            Hit(Key.Enter);

            //OneLink
            if (system == "OL")
            {
                if (Check4Text(22, 3, "49000", "48003"))
                {
                    return;
                }
            }
            //Compass
            else if (system == "CS")
            {
                if (Check4Text(23, 2, "01005", "01004"))
                {
                    return;
                }
            }
            MessageBox.Show("An error was encountered while changing or adding the record.  Click OK to pause the script, review and correct the information, and hit <Enter> to save the changes.  Hit <Insert> when you are done to resume the script.", "Information Not Updated", MessageBoxButtons.OK, MessageBoxIcon.Error);
            PauseForInsert();
        }

        /// <summary>
        /// Figures out what items were updated
        /// </summary>
        /// <returns>string of items that are being updated</returns>
        private string UpdatedItems(UpdateDemographics update)
        {
            string updatedItems = string.Empty;

            //Name
            if (update.NameUpdated)
            {
                updatedItems = "name";
            }

            //Address
            if (update.AddressUpdated)
            {
                if (updatedItems == "name")
                {
                    updatedItems += "/address";
                }
                else
                {
                    updatedItems = "address";
                }
            }

            //Phone
            if (update.PhoneUpdated)
            {
                if (updatedItems != string.Empty)
                {
                    updatedItems += "/phone";
                }
                else
                {
                    updatedItems = "phone";
                }
            }

            //Fax
            if (update.FaxUpdated)
            {
                if (updatedItems != string.Empty)
                {
                    updatedItems += "/fax";
                }
                else
                {
                    updatedItems = "fax";
                }
            }

            //Contact
            if (update.ContactUpdated)
            {
                if (updatedItems != string.Empty)
                {
                    updatedItems += "/contact";
                }
                else
                {
                    updatedItems = "contact";
                }
            }
            return updatedItems;
        }

        private void Logon(string logonMode)
        {
            switch (logonMode)
            {
                case "Live":
                    if (!Check4Text(16, 2, "LOGON"))
                    {
                        FastPath("Log");
                        Thread.Sleep(1000);
                    }
                    PutText(16, 12, "Pheaa", Key.Enter);
                    Thread.Sleep(1000);
                    PutText(20, 18, "UT00");
                    break;
                case "Test":
                    MessageBox.Show("Test Button");
                    if (!Check4Text(16, 2, "LOGON"))
                    {
                        FastPath("Log");
                        Thread.Sleep(1000);
                    }
                    PutText(16, 12, "QTOR", Key.Enter);
                    Thread.Sleep(1000);
                    MessageBox.Show(GetText(16, 2, 5));
                    PutText(20, 18, "UT00");
                    break;
            }

            while (Check4Text(20, 8, "USERID") || Check4Text(8, 22, "USERID"))
            {
                MessageBox.Show("Login in and press insert when done.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PauseForInsert();
                if (Check4Text(23, 2, "ON008"))
                {
                    MessageBox.Show("You are not authorized to access the OneLink and Compass test regions. Contact System Support if you need access to test.", "Test Region Access Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            switch (logonMode)
            {
                case "Live":
                    FastPath("LP00");
                    break;
                case "Test":
                    if (Check4Text(1, 24, "PLEASE SELECT ONE OF THE FOLLOWING"))
                    {
                        Coordinate location = FindText("RS/UT", 3, 5);
                        PutText(location.Row, location.Column - 2, "X", Key.Enter);
                    }
                    FastPath("LP00");
                    MessageBox.Show("Welcome to the OneLink and Compass test regions", "OneLink/Compass Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

    }//Class
}//NameSpace
