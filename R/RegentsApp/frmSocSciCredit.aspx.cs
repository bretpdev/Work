using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Collections.Generic;

namespace RegentsApp
{
    public partial class SocSciCredit : System.Web.UI.Page
    {
        #region Properties
        int _classTitle;
        string _classTitleOther;
        string _gradeLevel;
        double _credits;
        string _weight;
        int _grade1;
        int _grade2;
        int _grade3;
        int _grade4;
        int _grade5;
        int _grade6;
        int _seq;
        int _collegeCode;
        bool _isAllClear = false;
        ClassData cData;
        GradeData gData1;
        GradeData gData2;
        GradeData gData3;
        GradeData gData4;
        GradeData gData5;
        GradeData gData6;
        #endregion

        DataAccessLayer dal = new DataAccessLayer();
        string _userName;

        /// <summary>
        /// Loads Class 1 before loading the page
        /// </summary>
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            base.InitializeCulture();

            try
            {
                _userName = Session["UserName"].ToString();
            }
            catch (Exception)
            {
                Response.Redirect("frmTimeOut.aspx");
            }
            if (Page.IsPostBack == false)
            {
                Session.Remove("UpdateClass");
            }
            try
            {
                Session["UpdateClass"].ToString();
            }
            catch (Exception)
            {
                Session["UpdateClass"] = "Updated";
                cData = dal.GetClass(_userName, 4, 1);
                gData1 = dal.GetGrade(_userName, 4, 1, 1);
                gData2 = dal.GetGrade(_userName, 4, 1, 2);
                gData3 = dal.GetGrade(_userName, 4, 1, 3);
                gData4 = dal.GetGrade(_userName, 4, 1, 4);
                gData5 = dal.GetGrade(_userName, 4, 1, 5);
                gData6 = dal.GetGrade(_userName, 4, 1, 6);
                if (cData != null)
                {
                    Session["UpdateClass1"] = "true";
                    btnClass1.Enabled = true;
                    if (cData.WeightCode == "  ")
                    {
                        rdoConcurrent.Checked = false;
                        rdoAdvanced.Checked = false;
                        ddlClassTypeSelection.DataSource = dsStandardCourse;
                        ddlClassTypeSelection.DataTextField = "Description";
                        ddlClassTypeSelection.DataValueField = "Code";
                        ddlClassTypeSelection.DataSourceID = "";
                        ddlClassTypeSelection.DataBind();
                        if (dal.GetClassBoolean(cData.ClassTitleCode) == false)
                        {
                            tbClassName.Text = dal.GetClassDescription(cData.ClassTitleCode);
                            ddlClassTypeSelection.SelectedValue = dal.GetClassCode(4, null).ToString();
                            divClassName.Style.Add(HtmlTextWriterStyle.Display, "inline");
                        }
                        else
                        {
                            ddlClassTypeSelection.SelectedValue = cData.ClassTitleCode.ToString();
                            tbClassName.Text = "";
                        }
                    }
                    else if (cData.WeightCode == "AP")
                    {
                        rdoConcurrent.Checked = false;
                        rdoAdvanced.Checked = true;
                        ddlClassTypeSelection.DataSource = dsAdvancedCourse;
                        ddlClassTypeSelection.DataTextField = "Description";
                        ddlClassTypeSelection.DataValueField = "Code";
                        ddlClassTypeSelection.DataSourceID = "";
                        ddlClassTypeSelection.DataBind();
                        if (dal.GetClassBoolean(cData.ClassTitleCode) == false)
                        {
                            tbClassName.Text = dal.GetClassDescription(cData.ClassTitleCode);
                            ddlClassTypeSelection.SelectedValue = dal.GetClassCode(4, "AP").ToString();
                            divClassName.Style.Add(HtmlTextWriterStyle.Display, "inline");
                        }
                        else
                        {
                            ddlClassTypeSelection.SelectedValue = cData.ClassTitleCode.ToString();
                        }
                    }
                    else if (cData.WeightCode == "CE")
                    {
                        rdoConcurrent.Checked = true;
                        rdoAdvanced.Checked = false;
                        ddlClassTypeSelection.DataSource = dsConcurrentCourse;
                        ddlClassTypeSelection.DataTextField = "Description";
                        ddlClassTypeSelection.DataValueField = "Code";
                        ddlClassTypeSelection.DataSourceID = "";
                        ddlClassTypeSelection.DataBind();
                        if (dal.GetClassBoolean(cData.ClassTitleCode) == false)
                        {
                            tbClassName.Text = dal.GetClassDescription(cData.ClassTitleCode);
                            ddlClassTypeSelection.SelectedValue = dal.GetClassCode(4, "CE").ToString();
                            divClassName.Style.Add(HtmlTextWriterStyle.Display, "inline");
                        }
                        else
                        {
                            ddlClassTypeSelection.SelectedValue = cData.ClassTitleCode.ToString();
                        }
                        universityTable.Style.Add(HtmlTextWriterStyle.Display, "inline");
                        ddlUnivPlanAttend.SelectedValue = cData.CollegeCode.ToString();
                    }
                    ddlGradeLevel.SelectedValue = cData.GradeLevel;
                    tbCredits.Text = cData.Credits.ToString();
                }
                if (gData1 != null)
                {
                    Session["Grade1Update"] = "true";
                    ddl1Grade1.SelectedIndex = gData1.GradeCode;
                }
                if (gData2 != null)
                {
                    Session["Grade2Update"] = "true";
                    ddl1Grade2.SelectedIndex = gData2.GradeCode;
                }
                if (gData3 != null)
                {
                    Session["Grade3Update"] = "true";
                    ddl1Grade3.SelectedIndex = gData3.GradeCode;
                }
                if (gData4 != null)
                {
                    Session["Grade4Update"] = "true";
                    ddl1Grade4.SelectedIndex = gData4.GradeCode;
                }
                if (gData5 != null)
                {
                    Session["Grade5Update"] = "true";
                    ddl1Grade5.SelectedIndex = gData5.GradeCode;
                }
                if (gData6 != null)
                {
                    Session["Grade6Update"] = "true";
                    ddl1Grade6.SelectedIndex = gData6.GradeCode;
                }

                ClassData cData2 = dal.GetClass(_userName, 4, 2);
                ClassData cData3 = dal.GetClass(_userName, 4, 3);
                ClassData cData4 = dal.GetClass(_userName, 4, 4);
                ClassData cData5 = dal.GetClass(_userName, 4, 5);
                ClassData cData6 = dal.GetClass(_userName, 4, 6);
                ClassData cData7 = dal.GetClass(_userName, 4, 7);
                ClassData cData8 = dal.GetClass(_userName, 4, 8);
                if (cData2 != null)
                {
                    btnClass2.Enabled = true;
                }
                if (cData3 != null)
                {
                    btnClass3.Enabled = true;
                }
                if (cData4 != null)
                {
                    btnClass4.Enabled = true;
                }
                if (cData5 != null)
                {
                    btnClass5.Enabled = true;
                }
                if (cData6 != null)
                {
                    btnClass6.Enabled = true;
                }
                if (cData7 != null)
                {
                    btnClass7.Enabled = true;
                }
                if (cData8 != null)
                {
                    btnClass8.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Sets Timer interval, sets up test mode, creates session variables, sets the current class and enables next page button
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            timer.Interval = 600000;
            MaintainScrollPositionOnPostBack = true;

            // Check for test mode
            if (Properties.Settings.Default.TestMode)
            {
                dsStandardCourse.ConnectionString = Properties.Resources.ConnStringTest;
                dsAdvancedCourse.ConnectionString = Properties.Resources.ConnStringTest;
                dsConcurrentCourse.ConnectionString = Properties.Resources.ConnStringTest;
                dsGrade.ConnectionString = Properties.Resources.ConnStringTest;
            }

            #region Social Science Session Variables
            Session["ClassTitle"] = ddlClassTypeSelection.SelectedValue;
            Session["ClassTitleOther"] = tbClassName.Text;
            Session["GradeLevel"] = ddlGradeLevel.SelectedValue;
            Session["Credits"] = tbCredits.Text;
            Session["Grade1"] = ddl1Grade1.SelectedValue;
            Session["Grade2"] = ddl1Grade2.SelectedValue;
            Session["Grade3"] = ddl1Grade3.SelectedValue;
            Session["Grade4"] = ddl1Grade4.SelectedValue;
            Session["Grade5"] = ddl1Grade5.SelectedValue;
            Session["Grade6"] = ddl1Grade6.SelectedValue;
            if (rdoAdvanced.Checked == true)
            {
                Session["Weight"] = "AP";
            }
            else if (rdoConcurrent.Checked == true)
            {
                Session["Weight"] = "CE";
                Session["CollegeCode"] = ddlUnivPlanAttend.SelectedValue;
            }
            else
            {
                Session["Weight"] = "";
            }
            #endregion

            //Find the current class listed by the current sequence
            try
            {
                _seq = Convert.ToInt32(Session["Sequence"].ToString());
            }
            catch (Exception)
            {
                _seq = 1;
                Session["Sequence"] = _seq;
            }
            lblCreditsCounted.Text = dal.GetCreditsTotal(_userName, 4).ToString();
            if (Convert.ToDouble(lblCreditsCounted.Text) >= 3.5)
            {
                btnNextPage.Enabled = true;
            }
            else
            {
                btnNextPage.Enabled = false;
            }
            if (ddlClassTypeSelection.Text == "11586" || ddlClassTypeSelection.Text == "11587" || ddlClassTypeSelection.Text == "11588")
            {
                divClassName.Style.Add(HtmlTextWriterStyle.Display, "inline");
            }

            if (ddlUnivPlanAttend.Items.Count < 1)
            {
                List<ClassList> colleges = dal.GetListOfColleges();
                colleges.RemoveAt(colleges.FindIndex(p => p.Description == "Undecided"));
                colleges.Insert(1, new ClassList() { Code = 1, Description = "Other" });
                ddlUnivPlanAttend.DataSource = colleges;
                ddlUnivPlanAttend.DataValueField = "Code";
                ddlUnivPlanAttend.DataTextField = "Description";
                ddlUnivPlanAttend.DataBind();
            }
        }

        /// <summary>
        /// Unchecks both radio buttons and clears the resets the class selection drop down to default
        /// </summary>
        protected void btnRemoveSelection_Click(object sender, EventArgs e)
        {
            ClearClassList();
            lblInvalidClass.Visible = false;
            universityTable.Style.Add(HtmlTextWriterStyle.Display, "none");
        }

        /// <summary>
        /// Calls the ClearCurrentClass method and displays appropriate error message if false
        /// </summary>
        protected void btnClearClass_Click(object sender, EventArgs e)
        {
            if (ClearCurrentClass())
            {
                lblClearClass.Visible = false;
                vaOtherClass.Enabled = false;
                vaOtherClass.IsValid = true;
            }
            else
            {
                lblClearClass.Visible = true;
            }
        }

        /// <summary>
        /// Validates the current class, implements the save and sets up the controls for the next class
        /// </summary>
        protected void btnSaveClass_Click(object sender, EventArgs e)
        {
            if (ValidateClass(false))
            {
                divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                if (!ImplementSave())
                {
                    Response.Redirect("Error.aspx");
                }
            }
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Saves the current class, opens all classes and validates them 1 by 1 and displays error message for any
        /// class that has invalid data input.  If all classes are valid, it deletes the session variables
        /// and redirects to the Social Science subject.
        /// </summary>
        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            if (ValidateClass(true))
            {
                if (SaveAndReturn(Convert.ToInt32(lblClass.Text.Remove(0, 21))))
                {
                    ClassData cData1 = dal.GetClass(_userName, 4, 1);
                    ClassData cData2 = dal.GetClass(_userName, 4, 2);
                    ClassData cData3 = dal.GetClass(_userName, 4, 3);
                    ClassData cData4 = dal.GetClass(_userName, 4, 4);
                    ClassData cData5 = dal.GetClass(_userName, 4, 5);
                    ClassData cData6 = dal.GetClass(_userName, 4, 6);
                    ClassData cData7 = dal.GetClass(_userName, 4, 7);
                    ClassData cData8 = dal.GetClass(_userName, 4, 8);
                    bool isNextPageReady = false;
                    if (cData1 != null)
                    {
                        if (ValidateAllClasses(1, cData1))
                        {
                            isNextPageReady = true;
                        }
                        else
                        {
                            RetrieveNextClass(1);
                            ValidateClass(false);
                            SwitchClassLabel("0");
                            SwitchButtonLabel("0");
                            return;
                        }
                    }
                    if (cData2 != null)
                    {
                        if (ValidateAllClasses(2, cData2))
                        {
                            isNextPageReady = true;
                        }
                        else
                        {
                            RetrieveNextClass(2);
                            ValidateClass(false);
                            SwitchClassLabel("1");
                            SwitchButtonLabel("1");
                            return;
                        }
                    }
                    if (cData3 != null)
                    {
                        if (ValidateAllClasses(3, cData3))
                        {
                            isNextPageReady = true;
                        }
                        else
                        {
                            RetrieveNextClass(3);
                            ValidateClass(false);
                            SwitchClassLabel("2");
                            SwitchButtonLabel("2");
                            return;
                        }
                    }
                    if (cData4 != null)
                    {
                        if (ValidateAllClasses(4, cData4))
                        {
                            isNextPageReady = true;
                        }
                        else
                        {
                            RetrieveNextClass(4);
                            ValidateClass(false);
                            SwitchClassLabel("3");
                            SwitchButtonLabel("3");
                            return;
                        }
                    }
                    if (cData5 != null)
                    {
                        if (ValidateAllClasses(5, cData5))
                        {
                            isNextPageReady = true;
                        }
                        else
                        {
                            RetrieveNextClass(5);
                            ValidateClass(false);
                            SwitchClassLabel("4");
                            SwitchButtonLabel("4");
                            return;
                        }
                    }
                    if (cData6 != null)
                    {
                        if (ValidateAllClasses(6, cData6))
                        {
                            isNextPageReady = true;
                        }
                        else
                        {
                            RetrieveNextClass(6);
                            ValidateClass(false);
                            SwitchClassLabel("5");
                            SwitchButtonLabel("5");
                            return;
                        }
                    }
                    if (cData7 != null)
                    {
                        if (ValidateAllClasses(7, cData7))
                        {
                            isNextPageReady = true;
                        }
                        else
                        {
                            RetrieveNextClass(7);
                            ValidateClass(false);
                            SwitchClassLabel("6");
                            SwitchButtonLabel("6");
                            return;
                        }
                    }
                    if (cData8 != null)
                    {
                        if (ValidateAllClasses(8, cData8))
                        {
                            isNextPageReady = true;
                        }
                        else
                        {
                            RetrieveNextClass(8);
                            ValidateClass(false);
                            SwitchClassLabel("7");
                            SwitchButtonLabel("7");
                            return;
                        }
                    }
                    if (isNextPageReady)
                    {
                        lblCreditsCounted.Text = dal.GetCreditsTotal(_userName, 4).ToString();
                        if (Convert.ToDouble(lblCreditsCounted.Text) >= 3.5)
                        {
                            DeleteClassSessionVariables();
                            Response.Redirect("frmScienceCredit.aspx");
                        }
                        else
                        {
                            btnNextPage.Enabled = false;
                        }
                    }
                }
                else
                {
                    Response.Redirect("Error.aspx");
                }
            }
        }

        /// <summary>
        /// Saves the current class, deletes the class session variables and goes to previous screen.
        /// </summary>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (SaveAndReturn(Convert.ToInt32(lblClass.Text.Remove(0, 21))))
            {
                DeleteClassSessionVariables();
                Response.Redirect("frmMathCredit.aspx");
            }
            else
            {
                Response.Redirect("Error.apsx");
            }
        }

        /// <summary>
        /// Saves the current class and redirects user to the logout screen
        /// </summary>
        protected void btnSaveReturn_Click(object sender, EventArgs e)
        {
            if (SaveAndReturn(Convert.ToInt32(lblClass.Text.Remove(0, 21))))
            {
                DeleteClassSessionVariables();
                Response.Redirect("frmLogout.aspx");
            }
            else
            {
                Response.Redirect("error.apsx");
            }
        }

        /// <summary>
        /// Retrieves class 1 data then validates the class
        /// </summary>
        protected void btnClass1_Click(object sender, EventArgs e)
        {
            lblClass.Text = "Social Science Class 1";
            btnSaveClass.Text = "Save Class 1";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            RetrieveNextClass(1);
            ValidateClass(false);
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Retrieves class 2 data then validates the class
        /// </summary>
        protected void btnClass2_Click(object sender, EventArgs e)
        {
            lblClass.Text = "Social Science Class 2";
            btnSaveClass.Text = "Save Class 2";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            RetrieveNextClass(2);
            ValidateClass(false);
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Retrieves class 3 data then validates the class
        /// </summary>
        protected void btnClass3_Click(object sender, EventArgs e)
        {
            lblClass.Text = "Social Science Class 3";
            btnSaveClass.Text = "Save Class 3";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            RetrieveNextClass(3);
            ValidateClass(false);
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Retrieves class 4 data then validates the class
        /// </summary>
        protected void btnClass4_Click(object sender, EventArgs e)
        {
            lblClass.Text = "Social Science Class 4";
            btnSaveClass.Text = "Save Class 4";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            RetrieveNextClass(4);
            ValidateClass(false);
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Retrieves class 5 data then validates the class
        /// </summary>
        protected void btnClass5_Click(object sender, EventArgs e)
        {
            lblClass.Text = "Social Science Class 5";
            btnSaveClass.Text = "Save Class 5";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            RetrieveNextClass(5);
            ValidateClass(false);
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Retrieves class 6 data then validates the class
        /// </summary>
        protected void btnClass6_Click(object sender, EventArgs e)
        {
            lblClass.Text = "Social Science Class 6";
            btnSaveClass.Text = "Save Class 6";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            RetrieveNextClass(6);
            ValidateClass(false);
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Retrieves class 7 data then validates the class
        /// </summary>
        protected void btnClass7_Click(object sender, EventArgs e)
        {
            lblClass.Text = "Social Science Class 7";
            btnSaveClass.Text = "Save Class 7";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            RetrieveNextClass(7);
            ValidateClass(false);
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Retrieves class 8 data then validates the class
        /// </summary>
        protected void btnClass8_Click(object sender, EventArgs e)
        {
            lblClass.Text = "Social Science Class 8";
            btnSaveClass.Text = "Save Class 8";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            RetrieveNextClass(8);
            ValidateClass(false);
            lblClearClass.Visible = false;
        }

        /// <summary>
        /// Changes datasourse of class title drop down to the advances class list, clears any 
        /// classes selected, deletes anything typed into the other class title box and makes it invisible
        /// </summary>
        protected void rdoAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            rdoConcurrent.Checked = false;
            ddlClassTypeSelection.DataSource = dsAdvancedCourse;
            ddlClassTypeSelection.DataTextField = "Description";
            ddlClassTypeSelection.DataValueField = "Code";
            ddlClassTypeSelection.DataSourceID = "";
            ddlClassTypeSelection.DataBind();
            tbClassName.Text = "";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            universityTable.Style.Add(HtmlTextWriterStyle.Display, "none");
        }

        /// <summary>
        /// Changes datasourse of class title drop down to the concurrent class list, clears any 
        /// classes selected, deletes anything typed into the other class title box and makes it invisible
        /// </summary>
        protected void rdoConcurrent_CheckedChanged(object sender, EventArgs e)
        {
            foreach (IValidator v in Validators)
            {
                v.Validate();
                if (!v.IsValid)
                {
                    rdoConcurrent.Checked = false;
                    return;
                }
            }
            divConcurrent.Style.Add(HtmlTextWriterStyle.Display, "block");
            DisableButtons();
            rdoAdvanced.Checked = false;
            ddlClassTypeSelection.DataSource = dsConcurrentCourse;
            ddlClassTypeSelection.DataTextField = "Description";
            ddlClassTypeSelection.DataValueField = "Code";
            ddlClassTypeSelection.DataSourceID = "";
            ddlClassTypeSelection.DataBind();
            tbClassName.Text = "";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            universityTable.Style.Add(HtmlTextWriterStyle.Display, "inline");
        }

        /// <summary>
        /// Saves the data from all the controls on the page according to the seq number
        /// </summary>
        /// <returns>True if data is saved to the database properly</returns>
        public bool SaveAndReturn(int seq)
        {
            if (tbClassName.Text != "")
            {
                int code;
                if ((code = dal.CheckClassNameByCode(tbClassName.Text.Replace("'", "''"))) != 4 && code != 0)
                {
                    _classTitle = Convert.ToInt32(Session["ClassTitle"].ToString());
                    _classTitleOther = "";
                }
                else
                {
                    _classTitle = Convert.ToInt32(Session["ClassTitle"].ToString());
                    _classTitleOther = Session["ClassTitleOther"].ToString();
                }
            }
            else
            {
                _classTitle = Convert.ToInt32(Session["ClassTitle"].ToString());
                _classTitleOther = Session["ClassTitleOther"].ToString();
            }
            _gradeLevel = Session["GradeLevel"].ToString();
            _weight = Session["Weight"].ToString();
            try
            {
                _credits = Convert.ToDouble(Session["Credits"].ToString());
            }
            catch (Exception)
            {
                _credits = 0;
            }
            try
            {
                _grade1 = Convert.ToInt32(Session["Grade1"].ToString());
            }
            catch (Exception)
            {
                _grade1 = 0;
            }
            try
            {
                _grade2 = Convert.ToInt32(Session["Grade2"].ToString());
            }
            catch (Exception)
            {
                _grade2 = 0;
            }
            try
            {
                _grade3 = Convert.ToInt32(Session["Grade3"].ToString());
            }
            catch (Exception)
            {
                _grade3 = 0;
            }
            try
            {
                _grade4 = Convert.ToInt32(Session["Grade4"].ToString());
            }
            catch (Exception)
            {
                _grade4 = 0;
            }
            try
            {
                _grade5 = Convert.ToInt32(Session["Grade5"].ToString());
            }
            catch (Exception)
            {
                _grade5 = 0;
            }
            try
            {
                _grade6 = Convert.ToInt32(Session["Grade6"].ToString());
            }
            catch (Exception)
            {
                _grade6 = 0;
            }
            try
            {
                _collegeCode = Convert.ToInt32(Session["CollegeCode"].ToString());
            }
            catch (Exception)
            {
                _collegeCode = 0;
            }
            bool classSaved = true;
            bool grade1Saved = true;
            bool grade2Saved = true;
            bool grade3Saved = true;
            bool grade4Saved = true;
            bool grade5Saved = true;
            bool grade6Saved = true;
            try
            {
                switch (seq)
                {
                    case 1:
                        Session["UpdateClass1"].ToString();
                        break;
                    case 2:
                        Session["UpdateClass2"].ToString();
                        break;
                    case 3:
                        Session["UpdateClass3"].ToString();
                        break;
                    case 4:
                        Session["UpdateClass4"].ToString();
                        break;
                    case 5:
                        Session["UpdateClass5"].ToString();
                        break;
                    case 6:
                        Session["UpdateClass6"].ToString();
                        break;
                    case 7:
                        Session["UpdateClass7"].ToString();
                        break;
                    case 8:
                        Session["UpdateClass8"].ToString();
                        break;
                }
                if (ddlClassTypeSelection.SelectedItem.Text.Contains("Other"))
                {
                    if ((_classTitle = dal.GetClassName(_classTitleOther.ToString().Replace("'", ""))) == 0)
                    {
                        if (dal.InsertClassName(_classTitleOther.ToString().Replace("'", ""), 4, _weight))
                        {
                            _classTitle = dal.GetClassName(_classTitleOther.ToString().Replace("'", ""));
                        }
                    }
                    else
                    {
                        if ((_weight = dal.GetWeightCode(_classTitle)) == null)
                        {
                            _weight = string.Empty;
                        }
                    }
                    if (dal.UpdateClass(_userName, 4, seq, _classTitle, _gradeLevel, _weight, _credits, _collegeCode))
                    {
                        classSaved = true;
                    }
                    else
                    {
                        classSaved = false;
                    }
                }
                else
                {

                    if (dal.UpdateClass(_userName, 4, seq, _classTitle, _gradeLevel, _weight, _credits, _collegeCode))
                    {
                        classSaved = true;
                    }
                    else
                    {
                        classSaved = false;
                    }
                }
            }
            catch (Exception)
            {
                if (ddlClassTypeSelection.SelectedItem.Text.Contains("Other"))
                {
                    if ((_classTitle = dal.GetClassName(_classTitleOther.ToString().Replace("'", ""))) == 0)
                    {
                        if (dal.InsertClassName(_classTitleOther.ToString().Replace("'", ""), 4, _weight))
                        {
                            _classTitle = dal.GetClassName(_classTitleOther.ToString().Replace("'", ""));
                        }
                    }
                    if (dal.InsertClass(_userName, 4, seq, _classTitle, _gradeLevel, _weight, _credits, _collegeCode))
                    {
                        classSaved = true;
                    }
                    else
                    {
                        classSaved = false;
                    }
                }
                else
                {
                    if (_classTitle != 11601 && _classTitle != 11602 && _classTitle != 11603)
                    {
                        if (dal.InsertClass(_userName, 4, seq, _classTitle, _gradeLevel, _weight, _credits, _collegeCode))
                        {
                            classSaved = true;
                        }
                        else
                        {
                            classSaved = false;
                        }
                    }
                }
            }
            try
            {
                if (ddl1Grade1.Text != "0")
                {
                    Session["Grade1Update"].ToString();
                    if (dal.UpdateGrade(_userName, 4, seq, 1, _grade1))
                    {
                        grade1Saved = true;
                    }
                    else
                    {
                        grade1Saved = false;
                    }
                }
                else if (dal.DeleteGrade(_userName, 4, seq, 1))
                {
                    grade1Saved = true;
                }
            }
            catch (Exception)
            {
                if (dal.InsertGrade(_userName, 4, seq, 1, _grade1))
                {
                    grade1Saved = true;
                }
                else
                {
                    grade1Saved = false;
                }
            }
            try
            {
                if (ddl1Grade2.Text != "0")
                {
                    Session["Grade2Update"].ToString();
                    if (dal.UpdateGrade(_userName, 4, seq, 2, _grade2))
                    {
                        grade2Saved = true;
                    }
                    else
                    {
                        grade2Saved = false;
                    }
                }
                else if (dal.DeleteGrade(_userName, 4, seq, 2))
                {
                    grade2Saved = true;
                }
            }
            catch (Exception)
            {
                if (dal.InsertGrade(_userName, 4, seq, 2, _grade2))
                {
                    grade2Saved = true;
                }
                else
                {
                    grade2Saved = false;
                }
            }
            try
            {
                if (ddl1Grade3.Text != "0")
                {
                    Session["Grade3Update"].ToString();
                    if (dal.UpdateGrade(_userName, 4, seq, 3, _grade3))
                    {
                        grade3Saved = true;
                    }
                    else
                    {
                        grade3Saved = false;
                    }
                }
                else if (dal.DeleteGrade(_userName, 4, seq, 3))
                {
                    grade3Saved = true;
                }
            }
            catch (Exception)
            {
                if (dal.InsertGrade(_userName, 4, seq, 3, _grade3))
                {
                    grade3Saved = true;
                }
                else
                {
                    grade3Saved = false;
                }
            }
            try
            {
                if (ddl1Grade4.Text != "0")
                {
                    Session["Grade4Update"].ToString();
                    if (dal.UpdateGrade(_userName, 4, seq, 4, _grade4))
                    {
                        grade4Saved = true;
                    }
                    else
                    {
                        grade4Saved = false;
                    }
                }
                else if (dal.DeleteGrade(_userName, 4, seq, 4))
                {
                    grade4Saved = true;
                }
            }
            catch (Exception)
            {
                if (dal.InsertGrade(_userName, 4, seq, 4, _grade4))
                {
                    grade4Saved = true;
                }
                else
                {
                    grade4Saved = false;
                }
            }
            try
            {
                if (ddl1Grade5.Text != "0")
                {
                    Session["Grade5Update"].ToString();
                    if (dal.UpdateGrade(_userName, 4, seq, 5, _grade5))
                    {
                        grade5Saved = true;
                    }
                    else
                    {
                        grade5Saved = false;
                    }
                }
                else if (dal.DeleteGrade(_userName, 4, seq, 5))
                {
                    grade5Saved = true;
                }
            }
            catch (Exception)
            {
                if (dal.InsertGrade(_userName, 4, seq, 5, _grade5))
                {
                    grade5Saved = true;
                }
                else
                {
                    grade5Saved = false;
                }
            }
            try
            {
                if (ddl1Grade6.Text != "0")
                {
                    Session["Grade6Update"].ToString();
                    if (dal.UpdateGrade(_userName, 4, seq, 6, _grade6))
                    {
                        grade6Saved = true;
                    }
                    else
                    {
                        grade6Saved = false;
                    }
                }
                else if (dal.DeleteGrade(_userName, 4, seq, 6))
                {
                    grade6Saved = true;
                }
            }
            catch (Exception)
            {
                if (dal.InsertGrade(_userName, 4, seq, 6, _grade6))
                {
                    grade6Saved = true;
                }
                else
                {
                    grade6Saved = false;
                }
            }

            if (classSaved && grade1Saved && grade2Saved && grade3Saved && grade4Saved && grade5Saved && grade6Saved)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks each control to make sure their are all filled out completely and that they have valid data
        /// </summary>
        /// <returns>True if all fields pass validation</returns>
        private bool ValidateClass(bool nextPage)
        {
            bool validated = true;
            if ((ddlClassTypeSelection.Text == "11601" || ddlClassTypeSelection.Text == "11602" || ddlClassTypeSelection.Text == "11603") && !nextPage)
            {
                vaClassList.Enabled = true;
                vaClassList.IsValid = false;
                validated = false;
            }
            else if ((ddlClassTypeSelection.Text == "11601" || ddlClassTypeSelection.Text == "11602" || ddlClassTypeSelection.Text == "11603") && nextPage && (ddlGradeLevel.Text != "" || tbCredits.Text != ""))
            {
                vaClassList.Enabled = true;
                vaClassList.IsValid = false;
                validated = false;
            }
            else
            {
                vaClassList.Enabled = false;
                vaClassList.IsValid = true;
            }
            if (((ddlClassTypeSelection.Text == "11586" || ddlClassTypeSelection.Text == "11587" || ddlClassTypeSelection.Text == "11588") && tbClassName.Text == "") && !nextPage)
            {
                vaOtherClass.Enabled = true;
                vaOtherClass.IsValid = false;
                validated = false;
            }
            else
            {
                vaOtherClass.Enabled = false;
                vaOtherClass.IsValid = true;
            }
            if ((ddlGradeLevel.Text == "") && !nextPage)
            {
                vaGradeLevel.Enabled = true;
                vaGradeLevel.IsValid = false;
                validated = false;
            }
            else
            {
                vaGradeLevel.Enabled = false;
                vaGradeLevel.IsValid = true;
            }
            if ((tbCredits.Text.Replace(" ", "") == "" || Convert.ToDouble(tbCredits.Text) == 0) && !nextPage)
            {
                vaCredits.Enabled = true;
                vaCredits.IsValid = false;
                validated = false;
            }
            else if (tbCredits.Text != "" && Convert.ToDouble(tbCredits.Text) < .001)
            {
                vaCredits.Enabled = true;
                vaCredits.IsValid = false;
                validated = false;
                vaCredits.ErrorMessage = "Credits must be greater than 0 and can only have 3 decimal places.";
            }
            else
            {
                vaCredits.Enabled = false;
                vaCredits.IsValid = true;
            }
            bool empty = false;
            if (ddl1Grade1.Text == "0" && ddl1Grade2.Text == "0" && ddl1Grade3.Text == "0" && ddl1Grade4.Text == "0" && ddl1Grade5.Text == "0" && ddl1Grade6.Text == "0")
            {
                empty = true;
            }
            else
            {
                empty = false;
            }
            if ((ddlGradeLevel.Text != "12" && empty) && !nextPage)
            {
                termTable.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                termTable.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
                termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "1");
                lblGradeNeeded.Visible = true;
                validated = false;
            }
            else
            {
                termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                lblGradeNeeded.Visible = false;
            }
            int code;
            if (tbClassName.Text != "")
            {
                if ((code = dal.CheckClassNameByCode(tbClassName.Text.Replace("'", "''"))) != 4 && code != 0)
                {
                    lblInvalidClass.Visible = true;
                    validated = false;
                }
                else
                {
                    lblInvalidClass.Visible = false;
                }
            }
            if (ddlUnivPlanAttend.Text != "0" && rdoConcurrent.Checked)
            {
                vaAttend.Enabled = false;
                vaAttend.IsValid = true;
            }
            else if (ddlUnivPlanAttend.Text == "0" && !rdoConcurrent.Checked)
            {
                vaAttend.Enabled = false;
                vaAttend.IsValid = true;
            }
            else if (ddlUnivPlanAttend.Text == "0" && rdoConcurrent.Checked)
            {
                vaAttend.Enabled = true;
                vaAttend.IsValid = false;
                validated = false;
            }
            return validated;
        }

        /// <summary>
        /// Uses the class being passed in to validate all the fields stored in the database before moving 
        /// to the next class
        /// </summary>
        /// <param name="seq">Current class number</param>
        /// <param name="newData">data object of the current class being validated</param>
        /// <returns>True if current class is valid</returns>
        private bool ValidateAllClasses(int seq, ClassData newData)
        {
            gData1 = dal.GetGrade(_userName, 4, seq, 1);
            gData2 = dal.GetGrade(_userName, 4, seq, 2);
            gData3 = dal.GetGrade(_userName, 4, seq, 3);
            gData4 = dal.GetGrade(_userName, 4, seq, 4);
            gData5 = dal.GetGrade(_userName, 4, seq, 5);
            gData6 = dal.GetGrade(_userName, 4, seq, 6);
            bool isValid = false;
            if (newData.ClassTitleCode == 11601 || newData.ClassTitleCode == 11602 || newData.ClassTitleCode == 11603)
            {
                isValid = false;
            }
            else if (newData.ClassTitleCode != 0 && newData.Credits > 0 && newData.GradeLevel != "")
            {
                if (newData.GradeLevel != "12" && (gData1 == null && gData2 == null && gData3 == null && gData4 == null && gData5 == null && gData6 == null))
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }
            if (newData.Credits == 0 || newData.Credits > 4)
            {
                isValid = false;
                vaClass1Range.IsValid = false;
            }
            int code;
            if (!dal.GetClassBoolean(newData.ClassTitleCode))
            {
                if ((code = dal.CheckClassNameByCode(tbClassName.Text.Replace("'", "''"))) != 4 && code != 0)
                {
                    lblInvalidClass.Visible = true;
                    isValid = false;
                }
                else
                {
                    lblInvalidClass.Visible = false;
                }
                if (dal.GetClassDescription(newData.ClassTitleCode) == "")
                {
                    lblInvalidClass.Visible = true;
                    isValid = false;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Saves the current class, removes the session variables and loads the next class
        /// </summary>
        /// <returns>True if all data is stored in database</returns>
        private bool ImplementSave()
        {
            bool saved = false;
            if (SaveAndReturn(Convert.ToInt32(lblClass.Text.Remove(0, 21))))
            {
                string curClass = lblClass.Text.Remove(0, 21);
                SwitchClassLabel(curClass);
                SwitchButtonLabel(curClass);
                OpenNextClassButton(curClass);
                ClearClass();
                Session.Remove("Grade1Update");
                Session.Remove("Grade2Update");
                Session.Remove("Grade3Update");
                Session.Remove("Grade4Update");
                Session.Remove("Grade5Update");
                Session.Remove("Grade6Update");
                if ((Convert.ToInt32(curClass) + 1) < 9)
                {
                    RetrieveNextClass(Convert.ToInt32(curClass) + 1);
                }
                else
                {
                    RetrieveNextClass(8);
                }
                saved = true;
            }
            else
            {
                Response.Redirect("Error.aspx");
            }
            return saved;
        }

        /// <summary>
        /// Finds the current class, removes the class from the database, clears all the controls and retrieves
        /// the previous class
        /// </summary>
        /// <returns>True if all fields are deleted from the database and new class is ready to load</returns>
        private bool ClearCurrentClass()
        {
            bool isComplete = false;
            switch (_seq)
            {
                case 1:
                    if (btnClass2.Enabled == false || _isAllClear == true)
                    {
                        DeleteClass(_seq);
                        ClearClass();
                        isComplete = true;
                        _isAllClear = false;
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                        termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                        lblGradeNeeded.Visible = false;
                        Session.Remove("UpdateClass1");
                    }
                    break;
                case 2:
                    if (btnClass3.Enabled == false || _isAllClear == true)
                    {
                        DeleteClass(_seq);
                        ClearClass();
                        btnClass2.Enabled = false;
                        isComplete = true;
                        _isAllClear = false;
                        lblClass.Text = "Social Science Class 1";
                        btnSaveClass.Text = "Save Class 1";
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                        RetrieveNextClass(1);
                        termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                        lblGradeNeeded.Visible = false;
                        Session.Remove("UpdateClass2");
                    }
                    break;
                case 3:
                    if (btnClass4.Enabled == false || _isAllClear == true)
                    {
                        DeleteClass(_seq);
                        ClearClass();
                        btnClass3.Enabled = false;
                        isComplete = true;
                        _isAllClear = false;
                        lblClass.Text = "Social Science Class 2";
                        btnSaveClass.Text = "Save Class 2";
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                        RetrieveNextClass(2);
                        termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                        lblGradeNeeded.Visible = false;
                        Session.Remove("UpdateClass3");
                    }
                    break;
                case 4:
                    if (btnClass5.Enabled == false || _isAllClear == true)
                    {
                        DeleteClass(_seq);
                        ClearClass();
                        btnClass4.Enabled = false;
                        isComplete = true;
                        _isAllClear = false;
                        lblClass.Text = "Social Science Class 3";
                        btnSaveClass.Text = "Save Class 3";
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                        RetrieveNextClass(3);
                        termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                        lblGradeNeeded.Visible = false;
                        Session.Remove("UpdateClass4");
                    }
                    break;
                case 5:
                    if (btnClass6.Enabled == false || _isAllClear == true)
                    {
                        DeleteClass(_seq);
                        ClearClass();
                        btnClass5.Enabled = false;
                        isComplete = true;
                        _isAllClear = false;
                        lblClass.Text = "Social Science Class 4";
                        btnSaveClass.Text = "Save Class 4";
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                        RetrieveNextClass(4);
                        termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                        lblGradeNeeded.Visible = false;
                        Session.Remove("UpdateClass5");
                    }
                    break;
                case 6:
                    if (btnClass7.Enabled == false || _isAllClear)
                    {
                        DeleteClass(_seq);
                        ClearClass();
                        btnClass6.Enabled = false;
                        isComplete = true;
                        _isAllClear = false;
                        lblClass.Text = "Social Science Class 5";
                        btnSaveClass.Text = "Save Class 5";
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                        RetrieveNextClass(5);
                        termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                        lblGradeNeeded.Visible = false;
                        Session.Remove("UpdateClass6");
                    }
                    break;
                case 7:
                    if (btnClass8.Enabled == false || _isAllClear)
                    {
                        DeleteClass(_seq);
                        ClearClass();
                        btnClass7.Enabled = false;
                        isComplete = true;
                        _isAllClear = false;
                        lblClass.Text = "Social Science Class 6";
                        btnSaveClass.Text = "Save Class 6";
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                        RetrieveNextClass(6);
                        termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                        lblGradeNeeded.Visible = false;
                        Session.Remove("UpdateClass7");
                    }
                    break;
                case 8:
                    DeleteClass(_seq);
                    ClearClass();
                    btnClass8.Enabled = false;
                    isComplete = true;
                    _isAllClear = false;
                    lblClass.Text = "Social Science Class 7";
                    btnSaveClass.Text = "Save Class 7";
                    divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
                    RetrieveNextClass(7);
                    termTable.Style.Add(HtmlTextWriterStyle.BorderWidth, "0");
                    lblGradeNeeded.Visible = false;
                    Session.Remove("UpdateClass8");
                    break;
            }
            return isComplete;
        }

        /// <summary>
        /// Clears all the radio buttons and resets the class list drop down to the standard course
        /// </summary>
        private void ClearClassList()
        {
            rdoConcurrent.Checked = false;
            rdoAdvanced.Checked = false;
            ddlClassTypeSelection.DataSource = dsStandardCourse;
            ddlClassTypeSelection.DataTextField = "Description";
            ddlClassTypeSelection.DataValueField = "Code";
            ddlClassTypeSelection.DataSourceID = "";
            ddlClassTypeSelection.DataBind();
            tbClassName.Text = "";
            divClassName.Style.Add(HtmlTextWriterStyle.Display, "none");
            universityTable.Style.Add(HtmlTextWriterStyle.Display, "none");
            lblInvalidClass.Visible = false;
            ddlUnivPlanAttend.SelectedIndex = 0;
        }

        /// <summary>
        /// Calls the ClearClassList() method then resets the remaining form controls to default values.
        /// </summary>
        private void ClearClass()
        {
            ClearClassList();
            ddlGradeLevel.SelectedIndex = 4;
            tbClassName.Text = "";
            tbCredits.Text = "";
            ddl1Grade1.SelectedIndex = 0;
            ddl1Grade2.SelectedIndex = 0;
            ddl1Grade3.SelectedIndex = 0;
            ddl1Grade4.SelectedIndex = 0;
            ddl1Grade5.SelectedIndex = 0;
            ddl1Grade6.SelectedIndex = 0;
            ddlUnivPlanAttend.SelectedIndex = 0;
            Session.Remove("Grade1Update");
            Session.Remove("Grade2Update");
            Session.Remove("Grade3Update");
            Session.Remove("Grade4Update");
            Session.Remove("Grade5Update");
            Session.Remove("Grade6Update");
        }

        /// <summary>
        /// Changes the class label to reflect the current class
        /// </summary>
        /// <param name="str">The current class number</param>
        private void SwitchClassLabel(string str)
        {
            switch (str)
            {
                case "0":
                    lblClass.Text = "Social Science Class 1";
                    break;
                case "1":
                    lblClass.Text = "Social Science Class 2";
                    break;
                case "2":
                    lblClass.Text = "Social Science Class 3";
                    break;
                case "3":
                    lblClass.Text = "Social Science Class 4";
                    break;
                case "4":
                    lblClass.Text = "Social Science Class 5";
                    break;
                case "5":
                    lblClass.Text = "Social Science Class 6";
                    break;
                case "6":
                    lblClass.Text = "Social Science Class 7";
                    break;
                case "7":
                    lblClass.Text = "Social Science Class 8";
                    break;
            }
        }

        /// <summary>
        /// Changes the Save Class button to reflect the next class number
        /// </summary>
        /// <param name="str">The current class number</param>
        private void SwitchButtonLabel(string str)
        {
            switch (str)
            {
                case "0":
                    btnSaveClass.Text = "Save Class 1";
                    break;
                case "1":
                    btnSaveClass.Text = "Save Class 2";
                    break;
                case "2":
                    btnSaveClass.Text = "Save Class 3";
                    break;
                case "3":
                    btnSaveClass.Text = "Save Class 4";
                    break;
                case "4":
                    btnSaveClass.Text = "Save Class 5";
                    break;
                case "5":
                    btnSaveClass.Text = "Save Class 6";
                    break;
                case "6":
                    btnSaveClass.Text = "Save Class 7";
                    break;
                case "7":
                    btnSaveClass.Text = "Save Class 8";
                    break;
            }
        }

        /// <summary>
        /// Makes the class buttons visible after the class has been saved
        /// </summary>
        /// <param name="str">The current class number</param>
        private void OpenNextClassButton(string str)
        {
            switch (str)
            {
                case "1":
                    btnClass1.Enabled = true;
                    break;
                case "2":
                    btnClass2.Enabled = true;
                    break;
                case "3":
                    btnClass3.Enabled = true;
                    break;
                case "4":
                    btnClass4.Enabled = true;
                    break;
                case "5":
                    btnClass5.Enabled = true;
                    break;
                case "6":
                    btnClass6.Enabled = true;
                    break;
                case "7":
                    btnClass7.Enabled = true;
                    break;
                case "8":
                    btnClass8.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Retrieves a class depending on the seq number sent in
        /// </summary>
        /// <param name="seq">The class number that is being opened</param>
        private void RetrieveNextClass(int seq)
        {
            Session.Remove("Grade1Update");
            Session.Remove("Grade2Update");
            Session.Remove("Grade3Update");
            Session.Remove("Grade4Update");
            Session.Remove("Grade5Update");
            Session.Remove("Grade6Update");
            _seq = seq;
            Session["Sequence"] = _seq;
            cData = dal.GetClass(_userName, 4, _seq);
            gData1 = dal.GetGrade(_userName, 4, _seq, 1);
            gData2 = dal.GetGrade(_userName, 4, _seq, 2);
            gData3 = dal.GetGrade(_userName, 4, _seq, 3);
            gData4 = dal.GetGrade(_userName, 4, _seq, 4);
            gData5 = dal.GetGrade(_userName, 4, _seq, 5);
            gData6 = dal.GetGrade(_userName, 4, _seq, 6);
            if (cData != null)
            {
                SetSessionUpdates(seq);
                if (cData.WeightCode == "  ")
                {
                    rdoConcurrent.Checked = false;
                    rdoAdvanced.Checked = false;
                    ddlClassTypeSelection.DataSource = dsStandardCourse;
                    ddlClassTypeSelection.DataTextField = "Description";
                    ddlClassTypeSelection.DataValueField = "Code";
                    ddlClassTypeSelection.DataSourceID = "";
                    ddlClassTypeSelection.DataBind();
                    if (dal.GetClassBoolean(cData.ClassTitleCode) == false)
                    {
                        tbClassName.Text = dal.GetClassDescription(cData.ClassTitleCode);
                        ddlClassTypeSelection.SelectedValue = dal.GetClassCode(4, null).ToString();
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "inline");
                    }
                    else
                    {
                        ddlClassTypeSelection.SelectedValue = cData.ClassTitleCode.ToString();
                        tbClassName.Text = "";
                    }
                    universityTable.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                else if (cData.WeightCode == "AP")
                {
                    rdoConcurrent.Checked = false;
                    rdoAdvanced.Checked = true;
                    ddlClassTypeSelection.DataSource = dsAdvancedCourse;
                    ddlClassTypeSelection.DataTextField = "Description";
                    ddlClassTypeSelection.DataValueField = "Code";
                    ddlClassTypeSelection.DataSourceID = "";
                    ddlClassTypeSelection.DataBind();
                    if (dal.GetClassBoolean(cData.ClassTitleCode) == false)
                    {
                        tbClassName.Text = dal.GetClassDescription(cData.ClassTitleCode);
                        ddlClassTypeSelection.SelectedValue = dal.GetClassCode(4, "AP").ToString();
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "inline");
                    }
                    else
                    {
                        ddlClassTypeSelection.SelectedValue = cData.ClassTitleCode.ToString();
                        tbClassName.Text = "";
                    }
                    universityTable.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                else if (cData.WeightCode == "CE")
                {
                    rdoConcurrent.Checked = true;
                    rdoAdvanced.Checked = false;
                    ddlClassTypeSelection.DataSource = dsConcurrentCourse;
                    ddlClassTypeSelection.DataTextField = "Description";
                    ddlClassTypeSelection.DataValueField = "Code";
                    ddlClassTypeSelection.DataSourceID = "";
                    ddlClassTypeSelection.DataBind();
                    if (dal.GetClassBoolean(cData.ClassTitleCode) == false)
                    {
                        tbClassName.Text = dal.GetClassDescription(cData.ClassTitleCode);
                        ddlClassTypeSelection.SelectedValue = dal.GetClassCode(4, "CE").ToString();
                        divClassName.Style.Add(HtmlTextWriterStyle.Display, "inline");
                    }
                    else
                    {
                        ddlClassTypeSelection.SelectedValue = cData.ClassTitleCode.ToString();
                        tbClassName.Text = "";
                    }
                    universityTable.Style.Add(HtmlTextWriterStyle.Display, "inline");
                    ddlUnivPlanAttend.SelectedValue = cData.CollegeCode.ToString();
                }
                ddlGradeLevel.SelectedValue = cData.GradeLevel;
                tbCredits.Text = cData.Credits.ToString();
            }
            if (gData1 != null)
            {
                Session["Grade1Update"] = "true";
                ddl1Grade1.SelectedIndex = gData1.GradeCode;
            }
            else
            {
                ddl1Grade1.SelectedIndex = 0;
            }
            if (gData2 != null)
            {
                Session["Grade2Update"] = "true";
                ddl1Grade2.SelectedIndex = gData2.GradeCode;
            }
            else
            {
                ddl1Grade2.SelectedIndex = 0;
            }
            if (gData3 != null)
            {
                Session["Grade3Update"] = "true";
                ddl1Grade3.SelectedIndex = gData3.GradeCode;
            }
            else
            {
                ddl1Grade3.SelectedIndex = 0;
            }
            if (gData4 != null)
            {
                Session["Grade4Update"] = "true";
                ddl1Grade4.SelectedIndex = gData4.GradeCode;
            }
            else
            {
                ddl1Grade4.SelectedIndex = 0;
            }
            if (gData5 != null)
            {
                Session["Grade5Update"] = "true";
                ddl1Grade5.SelectedIndex = gData5.GradeCode;
            }
            else
            {
                ddl1Grade5.SelectedIndex = 0;
            }
            if (gData6 != null)
            {
                Session["Grade6Update"] = "true";
                ddl1Grade6.SelectedIndex = gData6.GradeCode;
            }
            else
            {
                ddl1Grade6.SelectedIndex = 0;
            }

            lblCreditsCounted.Text = dal.GetCreditsTotal(_userName, 4).ToString();
            if (Convert.ToDouble(lblCreditsCounted.Text) >= 3.5)
            {
                btnNextPage.Enabled = true;
            }
            else
            {
                btnNextPage.Enabled = false;
            }
        }

        /// <summary>
        /// Creates a session variable after a class is created, the session variable shows which classes have
        /// been created in order to update them later.
        /// </summary>
        /// <param name="seq">The current class number</param>
        private void SetSessionUpdates(int seq)
        {
            switch (seq)
            {
                case 1:
                    Session["UpdateClass1"] = "Updated";
                    break;
                case 2:
                    Session["UpdateClass2"] = "Updated";
                    break;
                case 3:
                    Session["UpdateClass3"] = "Updated";
                    break;
                case 4:
                    Session["UpdateClass4"] = "Updated";
                    break;
                case 5:
                    Session["UpdateClass5"] = "Updated";
                    break;
                case 6:
                    Session["UpdateClass6"] = "Updated";
                    break;
                case 7:
                    Session["UpdateClass7"] = "Updated";
                    break;
                case 8:
                    Session["UpdateClass8"] = "Updated";
                    break;
            }
        }

        /// <summary>
        /// Delete the class and all grades for the class number
        /// </summary>
        /// <param name="seq">The class that is being deleted</param>
        private void DeleteClass(int seq)
        {
            dal.DeleteClass(_userName, 4, seq);
            dal.DeleteAllGrades(_userName, 4, seq);
        }

        /// <summary>
        /// Removes all session variables so the next Subject can reuse the same variables
        /// </summary>
        private void DeleteClassSessionVariables()
        {
            Session.Remove("Grade1Update");
            Session.Remove("Grade2Update");
            Session.Remove("Grade3Update");
            Session.Remove("Grade4Update");
            Session.Remove("Grade5Update");
            Session.Remove("Grade6Update");
            Session.Remove("ClassTitle");
            Session.Remove("ClassTitleOther");
            Session.Remove("Sequence");
            Session.Remove("Weight");
            Session.Remove("GradeLevel");
            Session.Remove("Credits");
            Session.Remove("Grade1");
            Session.Remove("Grade2");
            Session.Remove("Grade3");
            Session.Remove("Grade4");
            Session.Remove("Grade5");
            Session.Remove("Grade6");
            Session.Remove("UpdateClass1");
            Session.Remove("UpdateClass2");
            Session.Remove("UpdateClass3");
            Session.Remove("UpdateClass4");
            Session.Remove("UpdateClass5");
            Session.Remove("UpdateClass6");
            Session.Remove("UpdateClass7");
            Session.Remove("UpdateClass8");
            Session.Remove("UpdateClass");
        }

        protected void btnContinueApp_Click(object sender, EventArgs e)
        {
            divConcurrent.Style.Add(HtmlTextWriterStyle.Display, "none");
            EnableButtons();
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            divGradeLevel.Style.Add(HtmlTextWriterStyle.Display, "none");
            EnableButtons();
        }

        protected void ddlGradeLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (IValidator v in Validators)
            {
                v.Validate();
                if (!v.IsValid)
                {
                    rdoConcurrent.Checked = false;
                    return;
                }
            }
            if (ddlGradeLevel.Text == "12")
            {
                divGradeLevel.Style.Add(HtmlTextWriterStyle.Display, "block");
                DisableButtons();
            }
        }

        protected void EnableButtons()
        {
            btnBack.Enabled = true;
            btnClearClass.Enabled = true;
            btnRemoveSelection.Enabled = true;
            btnSaveClass.Enabled = true;
            btnSaveReturn.Enabled = true;
            btnTop.Enabled = true;
            if (Convert.ToDouble(lblCreditsCounted.Text) >= 3.5)
            {
                btnNextPage.Enabled = true;
            }
            else
            {
                btnNextPage.Enabled = false;
            }
        }

        protected void DisableButtons()
        {
            btnBack.Enabled = false;
            btnClearClass.Enabled = false;
            btnNextPage.Enabled = false;
            btnRemoveSelection.Enabled = false;
            btnSaveClass.Enabled = false;
            btnSaveReturn.Enabled = false;
            btnTop.Enabled = false;
        }

    }//End Class
}//End Namespace
