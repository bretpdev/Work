using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;

namespace RegentsApp
{
    public partial class HighSchoolInfo : System.Web.UI.Page
    {
        Nullable<bool> willGraudateInUtah = null;
        public string JuniorHighCode { get; set; }
        public string HighSchoolCode { get; set; }
        Nullable<bool> didAttendOther = null;
        Nullable<bool> GraduationYear = null;
        public int GradeLevel { get; set; }
        public float GPA { get; set; }
        public int CompositeGPA { get; set; }
        public int EnglishACT { get; set; }
        public int MathACT { get; set; }
        public int ScienceACT { get; set; }
        public int ReadingACT { get; set; }
        public int CollegeCode { get; set; }

        DataAccessLayer dal = new DataAccessLayer();
        string userName;
        bool updateSchool;
        bool updateAct;
        bool updateEnglish;
        bool updateMath;
        bool updateScience;
        bool updateReading;
        bool _notValid;

        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Interval = 900000;
            MaintainScrollPositionOnPostBack = true;
            if (Properties.Settings.Default.TestMode)
            {
                dsACT.ConnectionString = Properties.Resources.ConnStringTest;
                dsCollege.ConnectionString = Properties.Resources.ConnStringTest;
            }

            if (!IsPostBack)
            {
                List<SchoolNames> jrHigh = dal.GetSchoolNames("MID");
                List<SchoolNames> highSchool = dal.GetSchoolNames("HIGH");

                //Remove these ceeb codes and add them at the top of the list
                SchoolNames highSchoolUnlisted = highSchool.Single(p => p.CeebCode == "00-002");
                SchoolNames highSchoolNonUtah = highSchool.Single(p => p.CeebCode == "00-003");
                SchoolNames jrHighUnlisted = jrHigh.Single(p => p.CeebCode == "00-001");
                SchoolNames jrHighNonUtah = jrHigh.Single(p => p.CeebCode == "00-000");
                highSchool.Remove(highSchoolNonUtah);
                highSchool.Remove(highSchoolUnlisted);
                jrHigh.Remove(jrHighNonUtah);
                jrHigh.Remove(jrHighUnlisted);
                highSchool.Insert(0, highSchoolUnlisted);
                highSchool.Insert(0, highSchoolNonUtah);
                jrHigh.Insert(0, jrHighUnlisted);
                jrHigh.Insert(0, jrHighNonUtah);

                //Add the Jr High and High School lists together
                jrHigh.AddRange(highSchool);

                //Order lists
                jrHigh = jrHigh.OrderBy(p => p.Name).ToList();
                jrHigh.Insert(0, new SchoolNames() { Name = "", CeebCode = "" });

                ddlJuniorHigh.DataSource = jrHigh;
                ddlJuniorHigh.DataTextField = "Name";
                ddlJuniorHigh.DataValueField = "CeebCode";
                ddlJuniorHigh.DataBind();
                highSchool = highSchool.OrderBy(p => p.Name).ToList();
                highSchool.Insert(0, new SchoolNames() { Name = "", CeebCode = "" });
                ddlHighSchool.DataSource = highSchool;
                ddlHighSchool.DataTextField = "Name";
                ddlHighSchool.DataValueField = "CeebCode";
                ddlHighSchool.DataBind();
            }

            #region High School Info Session Variables
            //Load the session variables with the data in the controls
            Session["WillGraduate"] = ddlWillGraduate.SelectedIndex;
            Session["JuniorHighCode"] = ddlJuniorHigh.SelectedValue;
            Session["HighSchoolCode"] = ddlHighSchool.SelectedValue;
            Session["DidAttendOther"] = ddlAttendOther.SelectedIndex;
            Session["GraduationYear"] = ddlGraduationYear.SelectedIndex;
            Session["GradeLevel"] = ddlGradeLevel.SelectedValue;
            Session["GPA"] = tbGPA.Text;
            Session["CompositeGPA"] = ddlCompACT.SelectedValue;
            Session["EnglishACT"] = ddlEngACT.SelectedValue;
            Session["MathACT"] = ddlMathACT.SelectedValue;
            Session["ScienceACT"] = ddlSciACT.SelectedValue;
            Session["ReadingACT"] = ddlReadACT.SelectedValue;
            Session["CollegeCode"] = ddlUnivPlanAttend.SelectedValue;
            #endregion

            //Check if the username is created, if not redirect to the Timeout screen
            try
            {
                userName = Session["UserName"].ToString();
            }
            catch (Exception)
            {
                Response.Redirect("frmTimeOut.aspx");
            }

            SchoolInfo schoolInfo = dal.GetSchoolInfo(userName);
            CompositeACT compositeAct = dal.GetCompositeACT(userName);
            EnglishACT engAct = dal.GetEnglishACT(userName);
            MathACT mathAct = dal.GetMathACT(userName);
            ScienceACT scienceAct = dal.GetScienceACT(userName);
            ReadingACT readingAct = dal.GetReadingACT(userName);

            if (schoolInfo != null) { updateSchool = true; }
            if (compositeAct != null) { updateAct = true; }
            if (engAct != null) { updateEnglish = true; }
            if (mathAct != null) { updateMath = true; }
            if (scienceAct != null) { updateScience = true; }
            if (readingAct != null) { updateReading = true; }

            if (Page.IsPostBack == false)
            {
                Session.Remove("UpdateSchool");
                Session.Remove("UpdateAct");
                Session.Remove("UpdateEnglish");
                Session.Remove("UpdateMath");
                Session.Remove("UpdateScience");
                Session.Remove("UpdateReading");
            }

            //Check if the session variable 'UpdateSchool' has been created. If not, pull the 
            //data from the database to load the student data and create the session variable
            try
            {
                Session["UpdateSchool"].ToString();
            }
            catch (Exception)
            {
                if (updateSchool)
                {
                    Session["UpdateSchool"] = "Updated";
                    ddlJuniorHigh.SelectedValue = schoolInfo.JuniorHighCeebCode;
                    ddlHighSchool.SelectedValue = schoolInfo.HighSchoolCeebCode;
                    tbGPA.Text = schoolInfo.HighSchoolCumulativeGPA.ToString();
                    ddlUnivPlanAttend.SelectedValue = schoolInfo.CollegeCode.ToString();

                    if (Convert.ToInt32(schoolInfo.GradeLevel) >= 8 && Convert.ToInt32(schoolInfo.GradeLevel) <= 12)
                    { ddlGradeLevel.SelectedValue = schoolInfo.GradeLevel.ToString(); }
                    else
                    { ddlGradeLevel.SelectedValue = ""; }

                    if (schoolInfo.GraduationYear.HasValue && schoolInfo.GraduationYear.Value == true)
                    { ddlGraduationYear.SelectedIndex = 1; }
                    else if (schoolInfo.GraduationYear.HasValue && schoolInfo.GraduationYear.Value == false)
                    {
                        ddlGraduationYear.SelectedIndex = 2;
                        divGradYear.Style.Add(HtmlTextWriterStyle.Display, "inline");
                        divButtons.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                    else if (!schoolInfo.GraduationYear.HasValue)
                    { ddlGraduationYear.SelectedIndex = 0; }

                    if (schoolInfo.HaveAttendedOther.HasValue && schoolInfo.HaveAttendedOther.Value == true)
                    { ddlAttendOther.SelectedIndex = 1; }
                    else if (schoolInfo.HaveAttendedOther.HasValue && schoolInfo.HaveAttendedOther.Value == false)
                    { ddlAttendOther.SelectedIndex = 2; }
                    else if (!schoolInfo.HaveAttendedOther.HasValue)
                    { ddlAttendOther.SelectedIndex = 0; }

                    if (schoolInfo.HighSchoolIsInUtah.HasValue && schoolInfo.HighSchoolIsInUtah.Value == true)
                    { ddlWillGraduate.SelectedIndex = 1; }
                    else if (schoolInfo.HighSchoolIsInUtah.HasValue && schoolInfo.HighSchoolIsInUtah.Value == false)
                    {
                        ddlWillGraduate.SelectedIndex = 2;
                        divNotApproved.Style.Add(HtmlTextWriterStyle.Display, "inline");
                        divButtons.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                    else if (!schoolInfo.HighSchoolIsInUtah.HasValue)
                    { ddlWillGraduate.SelectedIndex = 0; }

                    if (tbGPA.Text == "0")
                    { tbGPA.Text = ""; }
                }
            }

            //Check if the session variable 'UpdateAct' has been created. If not, pull the 
            //data from the database to load the student data and create the session variable
            try
            {
                Session["UpdateAct"].ToString();
            }
            catch (Exception)
            {
                if (updateAct)
                {
                    ddlCompACT.SelectedIndex = compositeAct.Code;
                    Session["UpdateAct"] = "Updated";
                }
            }

            //Check if the session variable 'UpdateEnglish' has been created. If not, pull the 
            //data from the database to load the student data and create the session variable
            try
            {
                Session["UpdateEnglish"].ToString();
            }
            catch (Exception)
            {
                if (updateEnglish)
                {
                    ddlEngACT.SelectedIndex = engAct.Code;
                    Session["UpdateEnglish"] = "Updated";
                }
            }

            //Check if the session variable 'UpdateMath' has been created. If not, pull the 
            //data from the database to load the student data and create the session variable
            try
            {
                Session["UpdateMath"].ToString();
            }
            catch (Exception)
            {
                if (updateMath)
                {
                    ddlMathACT.SelectedIndex = mathAct.Code;
                    Session["UpdateMath"] = "Updated";
                }
            }

            //Check if the session variable 'UpdateScience' has been created. If not, pull the 
            //data from the database to load the student data and create the session variable
            try
            {
                Session["UpdateScience"].ToString();
            }
            catch (Exception)
            {
                if (updateScience)
                {
                    ddlSciACT.SelectedIndex = scienceAct.Code;
                    Session["UpdateScience"] = "Updated";
                }
            }

            //Check if the session variable 'UpdateReading' has been created. If not, pull the 
            //data from the database to load the student data and create the session variable
            try
            {
                Session["UpdateReading"].ToString();
            }
            catch (Exception)
            {
                if (updateReading)
                {
                    ddlReadACT.SelectedIndex = readingAct.Code;
                    Session["UpdateReading"] = "Updated";
                }
            }
        }

        protected void btnInfoSaveReturn_Click(object sender, EventArgs e)
        {
            if (SaveAndReturn())
            {
                //Remove the session variables created in the High School Information page
                Session.Remove("UpdateSchool");
                Session.Remove("UpdateAct");
                Session.Remove("UpdateEnglish");
                Session.Remove("UpdateMath");
                Session.Remove("UpdateScience");
                Session.Remove("UpdateReading");
                Response.Redirect("frmLogout.aspx");
            }
            else
            {
                //Remove the session variables created in the High School Information page
                Session.Remove("UpdateSchool");
                Session.Remove("UpdateAct");
                Session.Remove("UpdateEnglish");
                Session.Remove("UpdateMath");
                Session.Remove("UpdateScience");
                Session.Remove("UpdateReading");
                Response.Redirect("Error.aspx"); 
            }
        }

        protected void btnInfoNextPage_Click(object sender, EventArgs e)
        {
            if (ddlWillGraduate.Text == "")
            {
                vaWillGraduate.Enabled = true;
                vaWillGraduate.IsValid = false;
                divNotApproved.Style.Add(HtmlTextWriterStyle.Display, "none");
                _notValid = true;
            }
            else if (ddlWillGraduate.Text == "No")
            {
                divButtons.Style.Add(HtmlTextWriterStyle.Display, "none");
                divNotApproved.Style.Add(HtmlTextWriterStyle.Display, "inline");
                _notValid = true;
            }
            else
            {
                divNotApproved.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            if (ddlJuniorHigh.SelectedItem.Value == "")
            {
                vaJuniorHigh.Enabled = true;
                vaJuniorHigh.IsValid = false;
                _notValid = true;
            }
            if (ddlHighSchool.SelectedItem.Value == "")
            {
                vaHighSchool.Enabled = true;
                vaHighSchool.IsValid = false;
                _notValid = true;
            }
            if (ddlAttendOther.Text == "")
            {
                vaAttendedOther.Enabled = true;
                vaAttendedOther.IsValid = false;
                _notValid = true;
            }
            if (ddlGraduationYear.Text == "")
            {
                vaGradYear.Enabled = true;
                vaGradYear.IsValid = false;
                divGradYear.Style.Add(HtmlTextWriterStyle.Display, "none");
                _notValid = true;
            }
            else if (ddlGraduationYear.Text == "No")
            {
                divButtons.Style.Add(HtmlTextWriterStyle.Display, "none");
                divGradYear.Style.Add(HtmlTextWriterStyle.Display, "inline");
                _notValid = true;
            }
            else
            {
                divGradYear.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            if (ddlGradeLevel.Text == "")
            {
                vaGradeLevel.Enabled = true;
                vaGradeLevel.IsValid = false;
                _notValid = true;
            }
            if (tbGPA.Text == "")
            {
                vaGPA.Enabled = true;
                vaGPA.IsValid = false;
                _notValid = true;
            }
            if (ddlCompACT.SelectedItem.Value == "0")
            {
                vaComposite.Enabled = true;
                vaComposite.IsValid = false;
                _notValid = true;
            }
            if (ddlEngACT.SelectedItem.Value == "0")
            {
                vaEnglish.Enabled = true;
                vaEnglish.IsValid = false;
                _notValid = true;
            }
            if (ddlMathACT.SelectedItem.Value == "0")
            {
                vaMath.Enabled = true;
                vaMath.IsValid = false;
                _notValid = true;
            }
            if (ddlSciACT.SelectedItem.Value == "0")
            {
                vaScience.Enabled = true;
                vaScience.IsValid = false;
                _notValid = true; 
            }
            if (ddlReadACT.SelectedItem.Value == "0")
            {
                vaReading.Enabled = true;
                vaReading.IsValid = false;
                _notValid = true;
            }
            if (ddlUnivPlanAttend.SelectedItem.Value == "0")
            {
                vaAttend.Enabled = true;
                vaAttend.IsValid = false;
                _notValid = true;
            }
            if (tbGPA.Text != "")
            {
                double gpaConvert = 0.00;
                try
                {
                    gpaConvert = Convert.ToDouble(tbGPA.Text);
                }
                catch (Exception)
                {
                    vaGPA.IsValid = false;
                    vaGPA.Text = "Incorrect format";
                    _notValid = true;
                }
                if (gpaConvert == 0.00)
                {
                    vaGPA.IsValid = false;
                    vaGPA.Text = "GPA Must be greater than 0.00";
                    _notValid = true;
                }
                if (gpaConvert > 4.00)
                {
                    vaGPA.IsValid = false;
                    vaGPA.Text = "GPA Must be less than 4.00";
                    _notValid = true;
                }
            }
            if (_notValid)
            {
                return;
            }
            if (SaveAndReturn())
            {
                //Remove the session variables created in the High School Information page
                Session.Remove("UpdateSchool");
                Session.Remove("UpdateAct");
                Session.Remove("UpdateEnglish");
                Session.Remove("UpdateMath");
                Session.Remove("UpdateScience");
                Session.Remove("UpdateReading");
                Response.Redirect("frmCreditInstructions.aspx");
            }
            else
            {
                //Remove the session variables created in the High School Information page
                Session.Remove("UpdateSchool");
                Session.Remove("UpdateAct");
                Session.Remove("UpdateEnglish");
                Session.Remove("UpdateMath");
                Session.Remove("UpdateScience");
                Session.Remove("UpdateReading");
                Response.Redirect("Error.aspx");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (SaveAndReturn())
            {
                //Remove the session variables created in the High School Information page
                Session.Remove("UpdateSchool");
                Session.Remove("UpdateAct");
                Session.Remove("UpdateEnglish");
                Session.Remove("UpdateMath");
                Session.Remove("UpdateScience");
                Session.Remove("UpdateReading");
                Response.Redirect("frmPersonalInfo.aspx"); 
            }
            else
            {
                //Remove the session variables created in the High School Information page
                Session.Remove("UpdateSchool");
                Session.Remove("UpdateAct");
                Session.Remove("UpdateEnglish");
                Session.Remove("UpdateMath");
                Session.Remove("UpdateScience");
                Session.Remove("UpdateReading");
                Response.Redirect("Error.aspx"); 
            }
        }

        public bool SaveAndReturn()
        {
            JuniorHighCode = Session["JuniorHighCode"].ToString();
            HighSchoolCode = Session["HighSchoolCode"].ToString();
            if (Session["GradeLevel"].ToString() != "")
            {
                GradeLevel = Convert.ToInt32(Session["GradeLevel"].ToString());
            }
            if (Session["DidAttendOther"].ToString() == "1")
            { didAttendOther = true; }
            else if (Session["DidAttendOther"].ToString() == "2")
            { didAttendOther = false; }
            if (Session["WillGraduate"].ToString() == "1")
            { willGraudateInUtah = true; }
            else if (Session["WillGraduate"].ToString() == "2")
            { willGraudateInUtah = false; }
            if (Session["GraduationYear"].ToString() == "1")
            { GraduationYear = true; }
            else if (Session["GraduationYear"].ToString() == "2")
            { GraduationYear = false; }
            double temp;
            if (Session["GPA"].ToString() != "")
            {
                temp = Convert.ToDouble(Session["GPA"].ToString());
                GPA = (float)temp;
            }
            if (Session["CompositeGPA"].ToString() != "")
            {
                CompositeGPA = Convert.ToInt32(Session["CompositeGPA"].ToString());
            }
            if (Session["EnglishACT"].ToString() != "")
            {
                EnglishACT = Convert.ToInt32(Session["EnglishACT"].ToString());
            }
            if (Session["MathACT"].ToString() != "")
            {
                MathACT = Convert.ToInt32(Session["MathACT"].ToString());
            }
            if (Session["ScienceACT"].ToString() != "")
            {
                ScienceACT = Convert.ToInt32(Session["ScienceACT"].ToString());
            }
            if (Session["ReadingACT"].ToString() != "")
            {
                ReadingACT = Convert.ToInt32(Session["ReadingACT"].ToString());
            }
            if (Session["CollegeCode"].ToString() != "")
            {
                CollegeCode = Convert.ToInt32(Session["CollegeCode"].ToString());
            }

            bool saved = false;
            if (updateSchool)
            {
                if (dal.UpdateSchoolInfo(userName, willGraudateInUtah, HighSchoolCode, GradeLevel, GraduationYear, GPA, CollegeCode, JuniorHighCode, didAttendOther))
                { saved = true; }
            }
            else
            {
                if (dal.InsertSchoolInfo(userName, willGraudateInUtah, HighSchoolCode, GradeLevel, GraduationYear, GPA, CollegeCode, JuniorHighCode, didAttendOther))
                { saved = true; }
            }
            if (updateAct)
            {
                if (CompositeGPA > -1)
                {
                    if (dal.UpdateACT(userName, 1, CompositeGPA))
                    { saved = true; }
                }
            }
            else
            {
                if (CompositeGPA > -1)
                {
                    if (dal.InsertACT(userName, 1, CompositeGPA))
                    { saved = true; }
                }
            }
            if (updateEnglish)
            {
                if (EnglishACT > -1)
                {
                    if (dal.UpdateACT(userName, 2, EnglishACT))
                    { saved = true; }
                }
            }
            else
            {
                if (EnglishACT > -1)
                {
                    if (dal.InsertACT(userName, 2, EnglishACT))
                    { saved = true; }
                }
            }
            if (updateMath)
            {
                if (MathACT > -1)
                {
                    if (dal.UpdateACT(userName, 3, MathACT))
                    { saved = true; }
                }
            }
            else
            {
                if (MathACT > -1)
                {
                    if (dal.InsertACT(userName, 3, MathACT))
                    { saved = true; }
                }
            }
            if (updateScience)
            {
                if (ScienceACT > -1)
                {
                    if (dal.UpdateACT(userName, 4, ScienceACT))
                    { saved = true; }
                }
            }
            else
            {
                if (ScienceACT > -1)
                {
                    if (dal.InsertACT(userName, 4, ScienceACT))
                    { saved = true; }
                }
            }
            if (updateReading)
            {
                if (ReadingACT > -1)
                {
                    if (dal.UpdateACT(userName, 5, ReadingACT))
                    { saved = true; }
                }
            }
            else
            {
                if (ReadingACT > -1)
                {
                    if (dal.InsertACT(userName, 5, ReadingACT))
                    { saved = true; }
                }
            }

            if (saved)
            { return true; }
            else
            { return false; }
        }
    }
}
