using System;
using System.Web.UI;

namespace RegentsApp
{
    public partial class PersonalInfo : System.Web.UI.Page
    {
        #region Properties
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public int State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Dob { get; set; }
        public int Gender { get; set; }
        public int Ethnic { get; set; }
        public string Ssid { get; set; }
        Nullable<bool> criminalRecord = null;
        Nullable<bool> citizen = null;
        Nullable<bool> eligible = null;
        Nullable<bool> finAid = null;
        public int HowHear { get; set; }
        Nullable<bool> uesp = null;
        #endregion

        DataAccessLayer dal = new DataAccessLayer();
        string userName;
        bool update;
        bool updateAddress;
        bool _notValid;

        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Interval = 900000;
            MaintainScrollPositionOnPostBack = true;
            if (Properties.Settings.Default.TestMode)
            {
                dsEthnicLookup.ConnectionString = Properties.Resources.ConnStringTest;
                dsGenderLookup.ConnectionString = Properties.Resources.ConnStringTest;
                dsHowHear.ConnectionString = Properties.Resources.ConnStringTest;
                dsStateLookup.ConnectionString = Properties.Resources.ConnStringTest;
            }

            #region Personal Info Session Variables
            //Load the session variables with the data in the controls
            Session["FirstName"] = tbFirstName.Text;
            Session["MiddleName"] = tbMidName.Text;
            Session["LastName"] = tbLastName.Text;
            Session["Street1"] = tbAddy1.Text;
            Session["Street2"] = tbAddy2.Text;
            Session["City"] = tbCity.Text;
            Session["State"] = ddlState.SelectedIndex;
            Session["Zip1"] = tbZip1.Text;
            Session["Zip2"] = tbZip2.Text;
            Session["Phone"] = tbPhone.Text;
            Session["DOB"] = tbDOB.Text;
            Session["Gender"] = ddlGender.SelectedValue;
            Session["Ethnicity"] = ddlEthnic.SelectedValue;
            Session["EthnicOther"] = tbEthnic.Text;
            Session["SSID"] = tbSSID.Text;
            Session["Criminal"] = ddlCriminal.SelectedIndex;
            Session["Citizen"] = ddlCitizen.SelectedIndex;
            Session["Eligible"] = ddlEligible.SelectedIndex;
            Session["FinAid"] = ddlFinAid.SelectedIndex;
            Session["HowHear"] = ddlHowHear.SelectedValue;
            Session["HowHearOther"] = tbHowHear.Text;
            Session["UESP"] = ddlUESP.SelectedIndex;
            #endregion

            //Default the state drop down list to Utah
            int state = Convert.ToInt32(Session["State"].ToString());
            if (state != -1) { ddlState.SelectedIndex = state; }
            else { ddlState.SelectedIndex = 43; }

            //Check if the username is created, if not redirect to the Timeout screen
            try
            {
                userName = Session["UserName"].ToString();
            }
            catch (Exception)
            {
                Response.Redirect("frmTimeout.aspx");
            }

            bool isNewAccount = false;
            try
            {
                if (Session["NewAccount"].ToString() == "True")
                {
                    isNewAccount = true;
                }
            }
            catch (Exception)
            {
                isNewAccount = false;
            }

            lblWelcome.Text = "Welcome " + userName;
            ddlHowHear.Attributes.Add("onchange", "viewHowHear(this.options[this.selectedIndex].text);");
            ddlEthnic.Attributes.Add("onchange", "viewEthnic(this.options[this.selectedIndex].text);");
            ddlCitizen.Attributes.Add("onchange", "CheckEligible(this.options[this.selectedIndex].text);");

            StudentData studentData;
            if (isNewAccount)
            {
                studentData = dal.GetNewStudentData(userName);
            }
            else
            {
                studentData = dal.GetStudent(userName);
            }
            StudentAddress studentAddress = dal.GetStudentAddress(userName);

            if (studentData == null)
            {
                update = false;
            }
            else if (isNewAccount)
            {
                tbFirstName.Text = studentData.FirstName;
                tbMidName.Text = studentData.MiddleName;
                tbLastName.Text = studentData.LastName;
                tbDOB.Text = studentData.DateOfBirth.Value.ToString("MM/dd/yyyy");
                Session.Remove("NewAccount");
            }
            else
                update = true;

            if (studentAddress == null)
                updateAddress = false;
            else
                updateAddress = true;

            if (Page.IsPostBack == false)
            {
                Session.Remove("UpdateStudent");
                Session.Remove("UpdateStudentAddress");
            }

            //Check if the session variable 'UpdateStudent' has been created. If not, pull the 
            //data from the database to load the student data and create the session variable
            try
            {
                Session["UpdateStudent"].ToString();
            }
            catch (Exception)
            {
                if (update)
                {
                    Session["UpdateStudent"] = "Updated";
                    tbFirstName.Text = studentData.FirstName;
                    tbMidName.Text = studentData.MiddleName;
                    tbLastName.Text = studentData.LastName;
                    ddlGender.SelectedValue = studentData.GenderCode.ToString();
                    tbSSID.Text = studentData.StateStudentId;

                    if (studentData.FirstName != "" && studentData.LastName != "" && studentData.DateOfBirth.ToString() != "" && studentData.EthnicityCode != 0 && studentAddress != null)
                    { tbDOB.Text = studentData.DateOfBirth.Value.ToString("MM/dd/yyyy"); }

                    if (dal.GetEthnicityDescription(studentData.EthnicityCode) == "Other" || dal.GetEthnicityBoolean(studentData.EthnicityCode) == false)
                    {
                        tbEthnic.Text = dal.GetEthnicityDescription(studentData.EthnicityCode);
                        ddlEthnic.SelectedValue = dal.GetEthnicityCode().ToString();
                        divEthnic.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else
                    { ddlEthnic.SelectedValue = studentData.EthnicityCode.ToString(); }

                    if ((dal.GetHowHearBoolean(studentData.HowHearAboutCode) == false))
                    {
                        tbHowHear.Text = dal.GetHowHearDescription(studentData.HowHearAboutCode);
                        ddlHowHear.SelectedValue = dal.GetHowHear("Other").ToString();
                        divHowHear.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else
                    {
                        ddlHowHear.SelectedValue = studentData.HowHearAboutCode.ToString();
                        vaHowHearOther.Enabled = false;
                    }

                    if (studentData.HasCriminalRecord.HasValue && studentData.HasCriminalRecord.Value == true)
                    {
                        ddlCriminal.SelectedIndex = 1;
                        divCrimNotElig.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else if (studentData.HasCriminalRecord.HasValue && studentData.HasCriminalRecord.Value == false)
                    { ddlCriminal.SelectedIndex = 2; }
                    else if (!studentData.HasCriminalRecord.HasValue)
                    { ddlCriminal.SelectedIndex = 0; }

                    if (studentData.IsUsCitizen.HasValue && studentData.IsUsCitizen.Value == true)
                    { ddlCitizen.SelectedIndex = 1; }
                    else if (studentData.IsUsCitizen.HasValue && studentData.IsUsCitizen.Value == false)
                    { ddlCitizen.SelectedIndex = 2; }
                    else if (!studentData.IsUsCitizen.HasValue)
                    { ddlCitizen.SelectedIndex = 0; }

                    if (studentData.IsEligibleForFederalAid.HasValue && studentData.IsEligibleForFederalAid.Value == true)
                    {
                        ddlEligible.SelectedIndex = 1;
                        divEligibleDDL.Style.Add(HtmlTextWriterStyle.Display, "block");
                        divEligibleLabel.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else if (studentData.IsEligibleForFederalAid.HasValue && studentData.IsEligibleForFederalAid.Value == false)
                    {
                        ddlEligible.SelectedIndex = 2;
                        divEligibleDDL.Style.Add(HtmlTextWriterStyle.Display, "block");
                        divEligibleLabel.Style.Add(HtmlTextWriterStyle.Display, "block");
                        divNonCitNotElig.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else if (!studentData.IsEligibleForFederalAid.HasValue)
                    {
                        ddlEligible.SelectedIndex = 0;
                        if (studentData.IsUsCitizen.HasValue && studentData.IsUsCitizen.Value == false)
                        {
                            divEligibleDDL.Style.Add(HtmlTextWriterStyle.Display, "block");
                            divEligibleLabel.Style.Add(HtmlTextWriterStyle.Display, "block");
                        }
                        else
                        {
                            divEligibleDDL.Style.Add(HtmlTextWriterStyle.Display, "none");
                            divEligibleLabel.Style.Add(HtmlTextWriterStyle.Display, "none");
                        }
                    }

                    if (studentData.IntendsToApplyForFederalAid.HasValue && studentData.IntendsToApplyForFederalAid.Value == true)
                    { ddlFinAid.SelectedIndex = 1; }
                    else if (studentData.IntendsToApplyForFederalAid.HasValue && studentData.IntendsToApplyForFederalAid.Value == false)
                    { ddlFinAid.SelectedIndex = 2; }
                    else if (!studentData.IntendsToApplyForFederalAid.HasValue)
                    { ddlFinAid.SelectedIndex = 0; }

                    if (studentData.UESP.HasValue && studentData.UESP.Value == true)
                    { ddlUESP.SelectedIndex = 1; }
                    else if (studentData.UESP.HasValue && studentData.UESP.Value == false)
                    { ddlUESP.SelectedIndex = 2; }
                    else if (!studentData.UESP.HasValue)
                    { ddlUESP.SelectedIndex = 0; }

                    Phone = studentData.Phone;
                    string newPhone = "";
                    int i = 0;
                    foreach (char c in Phone)
                    {
                        newPhone = newPhone + c;
                        if (i == 2 || i == 5)
                            newPhone = newPhone + "-";
                        i++;
                    }
                    tbPhone.Text = newPhone;
                }
            }

            //Check if the session variable 'UpdateStudentAddress' has been created. If not, pull the 
            //data from the database to load the student data and create the session variable
            try
            {
                Session["UpdateStudentAddress"].ToString();
            }
            catch (Exception)
            {
                if (updateAddress)
                {
                    Session["UpdateStudentAddress"] = "Updated";
                    tbAddy1.Text = studentAddress.Address1;
                    tbAddy2.Text = studentAddress.Address2;
                    tbCity.Text = studentAddress.City;
                    ddlState.SelectedIndex = studentAddress.StateCode - 1;
                    if (studentAddress.Zip.Length > 5)
                    {
                        string zip1 = "";
                        string zip2 = "";
                        int i = 0;
                        foreach (char c in studentAddress.Zip)
                        {
                            if (i <= 4)
                                zip1 = zip1 += c.ToString();
                            else
                                zip2 = zip2 += c.ToString();
                            i++;
                        }
                        tbZip1.Text = zip1;
                        tbZip2.Text = zip2;
                    }
                    else
                        tbZip1.Text = studentAddress.Zip;
                }
            }

        }

        protected void ddlHowHear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlHowHear.Focus();
            if (ddlHowHear.Text.Contains("Other"))
            {
                tbHowHear.Visible = true;
                tbHowHear.Focus();
            }
        }

        protected void btnInfoNextPage_Click(object sender, EventArgs e)
        {
            if (Session["FirstName"].ToString() == "")
            {
                vaFirstName.Enabled = true;
                vaFirstName.IsValid = false;
                _notValid = true;
            }
            if (Session["LastName"].ToString() == "")
            {
                vaLastName.Enabled = true;
                vaLastName.IsValid = false;
                _notValid = true;
            }
            if (Session["Street1"].ToString() == "")
            {
                vaAddress.Enabled = true;
                vaAddress.IsValid = false;
                _notValid = true;
            }
            if (Session["City"].ToString() == "")
            {
                vaCity.Enabled = true;
                vaCity.IsValid = false;
                _notValid = true;
            }
            if (Session["Zip1"].ToString() == "")
            {
                vaZip.Enabled = true;
                vaZip.IsValid = false;
                _notValid = true;
            }
            if (Session["Phone"].ToString() == "")
            {
                vaPhone.Enabled = true;
                vaPhone.IsValid = false;
                _notValid = true;
            }
            if (Session["DOB"].ToString() == "")
            {
                vaDob.Enabled = true;
                vaDob.IsValid = false;
                _notValid = true;
            }
            if (Session["Gender"].ToString() == "1")
            {
                vaGender.Enabled = true;
                vaGender.IsValid = false;
                _notValid = true;
            }
            if (Session["SSID"].ToString() == "")
            {
                vaSsid.Enabled = true;
                vaSsid.IsValid = false;
                _notValid = true;
            }
            if (Session["Citizen"].ToString() == "0")
            {
                vaCitizen.Enabled = true;
                vaCitizen.IsValid = false;
                _notValid = true;
            }
            if (Session["Eligible"].ToString() == "2")
            {
                divNonCitNotElig.Style.Add(HtmlTextWriterStyle.Display, "block");
                divEligibleLabel.Style.Add(HtmlTextWriterStyle.Display, "block");
                divEligibleDDL.Style.Add(HtmlTextWriterStyle.Display, "block");
                ddlEligible.SelectedIndex = 2;
                _notValid = true;
            }
            else if (Session["Eligible"].ToString() == "0" && Session["Citizen"].ToString() == "2")
            {
                vaEligible.IsValid = false;
                vaEligible.Enabled = true;
                divEligibleLabel.Style.Add(HtmlTextWriterStyle.Display, "inline");
                divEligibleDDL.Style.Add(HtmlTextWriterStyle.Display, "inline");
                _notValid = true;
            }
            else
                divNonCitNotElig.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (Session["FinAid"].ToString() == "0")
            {
                vaFinAid.Enabled = true;
                vaFinAid.IsValid = false;
                _notValid = true;
            }
            if (Session["UESP"].ToString() == "0")
            {
                vaUESP.Enabled = true;
                vaUESP.IsValid = false;
                _notValid = true;
            }
            if (Session["Criminal"].ToString() == "0")
            {
                vaCriminal.Enabled = true;
                vaCriminal.IsValid = false;
                _notValid = true;
            }
            else if (Session["Criminal"].ToString() == "1")
            {
                divCrimNotElig.Style.Add(HtmlTextWriterStyle.Display, "block");
                ddlCriminal.SelectedIndex = 1;
                _notValid = true;
            }
            else
                divCrimNotElig.Style.Add(HtmlTextWriterStyle.Display, "none");
            string ssid;
            if (Session["HowHear"].ToString() == "1")
            {
                vaHowHear.Enabled = true;
                vaHowHear.IsValid = false;
                vaHowHearOther.Enabled = false;
                _notValid = true;
            }
            else if (Session["HowHear"].ToString() == "6")
            {
                vaHowHear.IsValid = true;
                vaHowHearOther.Enabled = true;
                if (Session["HowHearOther"].ToString().Trim() == "")
                {
                    tbHowHear.Text = "";
                    vaHowHearOther.IsValid = false;
                    divHowHear.Style.Add(HtmlTextWriterStyle.Display, "block");
                    _notValid = true;
                }
            }
            else
            {
                vaHowHear.Enabled = true;
                vaHowHear.IsValid = true;
                vaHowHearOther.Enabled = false;
            }
            if (Session["Ethnicity"].ToString() == "25")
            {
                vaEthnic.Enabled = true;
                if (Session["EthnicOther"].ToString().Trim() == "")
                {
                    tbEthnic.Text = "";
                    vaEthnic.IsValid = false;
                    vaEthnic.Enabled = true;
                    divEthnic.Style.Add(HtmlTextWriterStyle.Display, "inline");
                    _notValid = true;
                }
            }
            else
            {
                vaEthnic.Enabled = false;
                vaEthnic.IsValid = true;
                divEthnic.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            if (Session["Zip1"].ToString().Length < 5)
            {
                vaZip.Text = "Zip code must be 5 characters long";
                vaZip.ErrorMessage = "Zip code must be 5 characters long";
                vaZip.IsValid = false;
                vaZip.Enabled = true;
                _notValid = true;
            }
            else if (Session["Zip2"].ToString() != "" && Session["Zip2"].ToString().Length != 4)
            {
                vaZip.Text = "Zip code plus 4 must have 4 characters";
                vaZip.ErrorMessage = "Zip code plus 4 must have 4 characters";
                vaZip.IsValid = false;
                vaZip.Enabled = true;
                _notValid = true;
            }
            try
            {
                int tempZipPlus;
                int tempZip = Convert.ToInt32(Session["Zip1"].ToString());
                if (Session["Zip2"].ToString() != "")
                    tempZipPlus = Convert.ToInt32(Session["Zip2"].ToString());
            }
            catch (Exception)
            {
                vaZip.IsValid = false;
                vaZip.Enabled = true;
                vaZip.Text = "Invalid ZIP Code";
                _notValid = true;
            }
            if (Session["Gender"].ToString() == "0")
            {
                vaGender.IsValid = false;
                vaGender.Enabled = true;
                _notValid = true;
            }
            if (Session["SSID"].ToString().Trim().Replace(" ", "").Length == 0)
            {
                vaSsid.Enabled = true;
                vaSsid.IsValid = false;
                _notValid = true;
                vaSSIDAlpha.Enabled = true;
                vaSSIDAlpha.IsValid = false;
            }
            else if (Session["SSID"].ToString().Trim().Replace(" ", "").Length < 6)
            {
                vaSSID6.Enabled = true;
                vaSSID6.IsValid = false;
                _notValid = true;
            }
            else if ((ssid = dal.GetSSIDOnly(Session["SSID"].ToString())) != null)
            {
                if (!update || (ssid = dal.GetSSID(userName, Session["SSID"].ToString())) == null && update)
                {
                    vaSsid.ErrorMessage = "The State Stident ID you have entered already exists in our database. Please call customer service at 1 (877) 336-7378 to speak with a customer service representative.";
                    vaSsid.IsValid = false;
                    vaSsid.Enabled = true;
                    _notValid = true;
                    vaSSIDAlpha.Enabled = true;
                    vaSSIDAlpha.IsValid = false;
                }
            }
            else
            {
                int count = 0;
                foreach (char c in Session["SSID"].ToString())
                {
                    try
                    {
                        if (Convert.ToInt32(c.ToString()) >= 0)
                        {
                            count++;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (count < 6)
                {
                    vaSSID6.Enabled = true;
                    vaSSID6.IsValid = false;
                    _notValid = true;
                }
            }
            try
            {
                if (DateTime.Parse(Session["DOB"].ToString()) >= DateTime.Now.AddYears(-10))
                {
                    vaRegDob.IsValid = false;
                    vaRegDob.Enabled = true;
                    vaRegDob.ErrorMessage = "To be eligible for the Regents' Scholarship you must apply during your senior year of high school.  Please check the date entered for accuracy.";
                    vaRegDob.Text = "To be eligible for the Regents' Scholarship you must apply during your senior year of high school.  Please check the date entered for accuracy.";
                    _notValid = true;
                }
                if (DateTime.Parse(Session["DOB"].ToString()) <= DateTime.Now.AddYears(-23))
                {
                    vaRegDob.IsValid = false;
                    vaRegDob.Enabled = true;
                    vaRegDob.ErrorMessage = "To be eligible for the Regents' Scholarship you must apply during your senior year of high school.  Please check the date entered for accuracy.";
                    vaRegDob.Text = "To be eligible for the Regents' Scholarship you must apply during your senior year of high school.  Please check the date entered for accuracy.";
                    _notValid = true;
                }
                if (DateTime.Parse(Session["DOB"].ToString()) > DateTime.Now)
                {
                    vaRegDob.IsValid = false;
                    vaRegDob.Enabled = true;
                    vaRegDob.Text = "To be eligible for the Regents' Scholarship you must apply during your senior year of high school.  Please check the date entered for accuracy.";
                    _notValid = true;
                }
            }
            catch (Exception)
            {
                vaRegDob.IsValid = false;
                vaRegDob.Enabled = true;
                vaRegDob.ErrorMessage = "You entered a birth date that is invalid. Please correct this and try again.";
                vaRegDob.Text = "You entered a birth date that is invalid. Please correct this and try again.";
                _notValid = true;
            }
            if (_notValid)
            {
                return;
            }
            if (SaveAndReturn())
            {
                //Remove the session variables created in the Personal Information screen
                Session.Remove("UpdateStudent");
                Session.Remove("UpdateStudentAddress");
                Response.Redirect("frmHighSchoolInfo.aspx");
            }
            else
            {
                //Remove the session variables created in the Personal Information screen
                Session.Remove("UpdateStudent");
                Session.Remove("UpdateStudentAddress");
                Response.Redirect("Error.aspx");
            }
        }

        protected void btnInfoSaveReturn_Click(object sender, EventArgs e)
        {
            if (SaveAndReturn())
            {
                //Remove the session variables created in the Personal Information screen
                Session.Remove("UpdateStudent");
                Session.Remove("UpdateStudentAddress");
                Response.Redirect("frmLogout.aspx");
            }
            else
            {
                //Remove the session variables created in the Personal Information screen
                Session.Remove("UpdateStudent");
                Session.Remove("UpdateStudentAddress");
                Response.Redirect("SaveError.aspx");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (SaveAndReturn())
            {
                //Remove the session variables created in the Personal Information screen
                Session.Remove("UpdateStudent");
                Session.Remove("UpdateStudentAddress");
                Response.Redirect("frmAppInfo.aspx");
            }
            else
            {
                //Remove the session variables created in the Personal Information screen
                Session.Remove("UpdateStudent");
                Session.Remove("UpdateStudentAddress");
                Response.Redirect("Error.aspx");
            }
        }

        public bool SaveAndReturn()
        {
            FirstName = Session["FirstName"].ToString().Replace("'", "''");
            MiddleName = Session["MiddleName"].ToString().Replace("'", "''");
            LastName = Session["LastName"].ToString().Replace("'", "''");
            Street1 = Session["Street1"].ToString().Replace("'", "''");
            Street2 = Session["Street2"].ToString().Replace("'", "''");
            City = Session["City"].ToString().Replace("'", "''");
            State = Convert.ToInt32(Session["State"].ToString()) + 1;
            Phone = Session["Phone"].ToString().Replace("-", "");
            if (!string.IsNullOrEmpty(Session["Zip1"].ToString()) && !string.IsNullOrEmpty(Session["Zip2"].ToString()))
                Zip = Session["Zip1"].ToString().Replace("'", "''") + Session["Zip2"].ToString().Replace("'", "''");
            else if (!string.IsNullOrEmpty(Session["Zip1"].ToString()) && string.IsNullOrEmpty(Session["Zip2"].ToString()))
                Zip = Session["Zip1"].ToString().Replace("'", "''");
            if (Session["DOB"].ToString() != "")
                Dob = Session["DOB"].ToString();
            Gender = Convert.ToInt32(Session["Gender"].ToString());
            Ssid = dal.GetSSID(userName, Session["SSID"].ToString());
            string checkSsid = dal.GetSSIDOnly(Session["SSID"].ToString());
            if (Ssid != null || (Ssid == null && checkSsid == null))
            {
                Ssid = Session["SSID"].ToString();
            }
            else
            {
                Ssid = "";
            }

            if (Session["Criminal"].ToString() == "1")
            { criminalRecord = true; }
            else if (Session["Criminal"].ToString() == "2")
            { criminalRecord = false; }

            if (Session["Citizen"].ToString() == "1")
            { citizen = true; }
            else if (Session["Citizen"].ToString() == "2")
            { citizen = false; }

            if (Session["Eligible"].ToString() == "1")
            { eligible = true; }
            else if (Session["Eligible"].ToString() == "2")
            { eligible = false; }

            if (Session["FinAid"].ToString() == "1")
            { finAid = true; }
            else if (Session["FinAid"].ToString() == "2")
            { finAid = false; }

            if (Session["UESP"].ToString() == "1")
            { uesp = true; }
            else if (Session["UESP"].ToString() == "2")
            { uesp = false; }

            if (Session["Ethnicity"].ToString() == "25")
            {
                if (Session["Ethnicity"].ToString().Trim() != string.Empty)
                {
                    if ((Ethnic = dal.GetNewEthnicity(Session["EthnicOther"].ToString().Replace("'", "''"))) == 0)
                    {
                        dal.InsertEthnicityLookup(Session["EthnicOther"].ToString().Replace("'", "''"));
                        Ethnic = dal.GetNewEthnicity(Session["EthnicOther"].ToString().Replace("'", "''"));
                    }
                }
            }
            else
            { Ethnic = Convert.ToInt32(Session["Ethnicity"].ToString()); }
            if (Session["HowHear"].ToString() == "6")
            {
                if ((HowHear = dal.GetNewHowHear(Session["HowHearOther"].ToString().Replace("'", "''"))) == 0)
                {
                    dal.InsertHowHear(Session["HowHearOther"].ToString().Replace("'", "''"));
                    HowHear = dal.GetNewHowHear(Session["HowHearOther"].ToString().Replace("'", "''"));
                }
            }
            else
            { HowHear = Convert.ToInt32(Session["HowHear"].ToString()); }

            bool saved = false;
            //Always do an update, never an insert. The account should be created when they create the account.
            if (dal.UpdateStudent(FirstName, MiddleName, LastName, Dob, Gender, Ethnic, Ssid, criminalRecord, citizen, eligible, finAid, HowHear, Phone, userName, uesp))
            { saved = true; }
            else
            { saved = false; }

            if (updateAddress)
            {
                if (dal.UpdateStudentAddress(userName, Street1, Street2, City, State, Zip))
                { saved = true; }
                else
                { saved = false; }
            }
            else
            {
                if (dal.InsertStudentAddress(userName, Street1, Street2, City, State, Zip))
                { saved = true; }
                else
                { saved = false; }
            }
            if (saved)
            { return true; }
            else
            { return false; }
        }
    }
}
