<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmScienceCredit.aspx.cs"
    Inherits="RegentsApp.ScienceCredit" Title="Regents' Application :: Science Credits"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="ScienceContent" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_Science" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    SaveAndReturn(Convert.ToInt32(lblClass.Text.Remove(0, 14)));
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <script language="javascript" type="text/javascript">

                window.onload = function() {
                    if (document.getElementById('<%= ddlClassTypeSelection.ClientID %>').value == "11583" || document.getElementById('<%= ddlClassTypeSelection.ClientID %>').value == "11584" || document.getElementById('<%= ddlClassTypeSelection.ClientID %>').value == "11585") {
                        document.getElementById('<%= divClassName.ClientID %>').style.display = "inline";
                    }
                    if (!NiftyCheck())
                        return;
                    Rounded("div#MainContainer", "Transparent", "White", "large");
                    DisableBackButton();
                }

                function viewSelection(sent) {
                    if (document.getElementById('<%= ddlClassTypeSelection.ClientID %>').value == "11583" || document.getElementById('<%= ddlClassTypeSelection.ClientID %>').value == "11584" || document.getElementById('<%= ddlClassTypeSelection.ClientID %>').value == "11585") {
                        document.getElementById('<%= divClassName.ClientID %>').style.display = "inline";
                        document.getElementById('<%= tbClassName.ClientID %>').value = "";
                    }
                    else {
                        document.getElementById('<%= divClassName.ClientID %>').style.display = "none";
                        document.getElementById('<%= tbClassName.ClientID %>').value = "";
                        ValidatorEnable(document.getElementById('<%= vaOtherClass.ClientID %>'), false);
                    }
                }

                function Close() {
                    document.getElementById("initialMessage").style.display = "none";
                }
            </script>

            <div id="divGradeLevel" runat="server" style="display: none; font-weight: normal;
                color: white; width: 70%; left: 15%; top: 900px; position: absolute; padding: 1em;
                background: #5d87a1; border: 5px ridge gray;">
                <p style="padding: 0px 15px 0px 15px; font-size: x-large;">
                    <b>NOTE: If the course is one that you will complete by the end of your 12th grade year
                        or are currently taking the course, but have not received a grade for a course leave
                        the term blank.</b></p>
                <asp:Button ID="btnContinue" runat="server" Text="Continue With Application" OnClick="btnContinue_Click"
                    CausesValidation="False" />
            </div>
            <div id="divConcurrent" runat="server" style="display: none; font-weight: normal;
                color: white; width: 70%; left: 15%; top: 900px; position: absolute; padding: 1em;
                background: #5d87a1; border: 5px ridge gray;">
                <p style="padding: 0px 15px 0px 15px; font-size: x-large;">
                    <b>If you completed or will complete a college or concurrent enrollment course worth
                        3 or more college credits record the credit earned as 1 high school credit. If your
                        high school transcript identifies the class as being worth more than 1 credit, report
                        the credit value as stated on your high school transcript.</b></p>
                <asp:Button ID="btnContinueApp" runat="server" Text="Continue With Application" OnClick="btnContinueApp_Click" />
            </div>
            <div id="initialMessage" style="display: block; color: White; width: 70%; left: 15%;
                top: 15%; position: absolute; padding: 1em; background: #5d87a1; border: 5px ridge gray;">
                <p style="padding: 0px 15px 0px 15px; font-size: x-large;">
                    In this section, you will be required to first enter your science classes in the
                    following order: Biology, Chemistry and Physics. You can then add any additional
                    science classes that meet the scholarship requirement such as Biology AP, Chemistry
                    AP, and Physics AP. If you come back to the Science section, you will be required to
                    save all three classes again for validation purposes. <span style="color: Lime;">ONLY
                        enter science classes that satisfy the Regents’ Scholarship. Earth Systems and Earth
                        Science do not satisfy the scholarship requirements.</span></p>
                <input type="button" id="closePopup" onclick="javascript: Close();" value="Close" />
            </div>
            <h1 style="font-family: Verdana;">
                Educational Information - Page 4
            </h1>
            <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
                <p style="color: Red;">
                    Please help us protect your personal information by logging out of the application
                    by using the &quot;Save and Return Later&quot; button at the bottom of the page.</p>
                <table>
                    <tr>
                        <td style="border-color: Black; border-style: solid; border-width: thin; width: 450px;
                            padding: 10px 5px 0px 5px;">
                            <h3>
                                Science &amp; Labs - 3 Credits Required, including one each of Biology, Chemistry,
                                and Physics</h3>
                        </td>
                    </tr>
                </table>
                <p>
                    Enter the Biology, Chemistry, and Physics courses you have taken, are taking, or
                    will take in grades 9, 10, 11, and 12. You must continue adding classes until you
                    reach the minimum credit requirement. You must fill out all required information
                    for each class. You will not be able to move on to the next section of the application
                    until the credit requirement has been entered for Science.</p>
                <p>
                    Note: If you have chosen to satisfy the scholarship requirement through completing
                    a college or concurrent enrollment course, you must enroll in both the lecture component
                    and the lab component of the science course. For example, if a student chooses to
                    complete Biology 1010 (the lecture component), then the student must also complete
                    Biology 1015 (the lab component). Both the lecture and the lab must appear on the
                    college transcript. Enter the grade you earned for the lecture first followed by
                    the lab grade. A college "lecture" course combined with a high school "lab" is not
                    accepted for the scholarship.</p>
                <p>
                    *Earth Systems and Earth Science <b>DO NOT</b> satisfy the scholarship science requirement.</p>
                <asp:SqlDataSource ID="dsGrade" runat="server" ConnectionString="<%$ ConnectionStrings:RegentsAppConnectionString %>"
                    SelectCommand="SELECT [Code], [Description] FROM [GradeLookup] ORDER BY [Code]">
                </asp:SqlDataSource>
                <br />
                <p style="padding: 0px 15px 0px 15px;">
                    For information regarding courses that will satisfy the Regents&#39; Scholarship
                    course requirements, <a href="http://www.higheredutah.org/wp-content/uploads/2010/03/11-12-RS-Program-Information.pdf"
                        target="_default">click here.</a></p>
                <div id="divClass" style="border: solid 1px black;" runat="server">
                    <br />
                    <asp:Label ID="lblClassType" runat="server" Text="Biology" Font-Bold="True" Font-Size="XX-Large"></asp:Label><br />
                    <asp:Label ID="lblClass" runat="server" Text="Science Class 1" Font-Bold="True" Font-Size="XX-Large"></asp:Label>
                    <br />
                    <br />
                    <asp:RadioButton ID="rdoAdvanced" runat="server" AutoPostBack="True" OnCheckedChanged="rdoAdvanced_CheckedChanged"
                        Text="Advanced Placement" />
                    <br />
                    <asp:RadioButton ID="rdoConcurrent" runat="server" AutoPostBack="True" OnCheckedChanged="rdoConcurrent_CheckedChanged"
                        Text="Concurrent Enrollment/College" CausesValidation="false" />
                    <br />
                    <table id="universityTable" style="display: none;" runat="server">
                        <tr>
                            <td style="text-align: right; width: 400px;">
                                <asp:Label ID="lblUnivPlanAttend" runat="server">Please select the college or university where you took this class?</asp:Label>
                            </td>
                            <td style="text-align: left; padding-left: 10px;">
                                <br />
                                <asp:DropDownList ID="ddlUnivPlanAttend" runat="server" Width="225px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vaAttend" runat="server" ControlToValidate="ddlUnivPlanAttend"
                                    ErrorMessage="Please select the college or university where the class was taken"
                                    Enabled="False">*</asp:RequiredFieldValidator>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnRemoveSelection" runat="server" CausesValidation="False" OnClick="btnRemoveSelection_Click"
                        Text="Remove Selection" />
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblCourseName" runat="server" Text="Course Name:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlClassTypeSelection" runat="server" onchange="javascript: viewSelection(this.options[this.selectedIndex].text);">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vaClassList" runat="server" ControlToValidate="ddlClassTypeSelection"
                                    ErrorMessage="Course name is required">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="text-align: left;">
                                <div id="divClassName" runat="server" style="display: none;">
                                    <asp:TextBox ID="tbClassName" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="vaOtherClass" runat="server" ErrorMessage="Other class name required"
                                        Text="*" ControlToValidate="tbClassName" Enabled="False"></asp:RequiredFieldValidator>
                                    <br />
                                    <asp:Label ID="lblInvalidClass" ForeColor="Red" Visible="false" runat="server" Text="You have entered an invalid course name"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblGradeLevel" runat="server" Text="Grade Level:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlGradeLevel" runat="server" CausesValidation="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlGradeLevel_SelectedIndexChanged">
                                    <asp:ListItem>9</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>11</asp:ListItem>
                                    <asp:ListItem>12</asp:ListItem>
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vaGradeLevel" runat="server" ErrorMessage="Grade Level Required"
                                    ControlToValidate="ddlGradeLevel" Enabled="False">*</asp:RequiredFieldValidator>
                            </td>
                            <td style="text-align: right; padding-left: 50px;">
                                <asp:Label ID="lblCredits" runat="server" Text="Credits:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="tbCredits" runat="server" Width="50px" MaxLength="4" CausesValidation="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vaCredits" runat="server" ErrorMessage="Amount of Credits Required"
                                    ControlToValidate="tbCredits" Enabled="False">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:RangeValidator ID="vaClass1Range" runat="server" ControlToValidate="tbCredits"
                                    ErrorMessage="Please enter between 0 and 4.00 credits" MaximumValue="4.00" MinimumValue="0.00"
                                    Type="Double"></asp:RangeValidator>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <p style="padding: 0px 15px 0px 15px;">
                        Enter your grades for this course below. For example, Term 1 will be associated
                        to first term/trimester, Term 2 will be associated to second term/trimester, etc.</p>
                    <p style="padding: 0px 15px 0px 15px;">
                        <table id="termTable" runat="server">
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lbl1Grade1" runat="server" Text="Term 1:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddl1Grade1" runat="server" DataSourceID="dsGrade" DataTextField="Description"
                                        DataValueField="Code">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right; padding-left: 50px;">
                                    <asp:Label ID="lbl1Grade2" runat="server" Text="Term 2:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddl1Grade2" runat="server" DataSourceID="dsGrade" DataTextField="Description"
                                        DataValueField="Code">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right; padding-left: 50px;">
                                    <asp:Label ID="lbl1Grade3" runat="server" Text="Term 3:"></asp:Label>
                                </td>
                                <td style="text-align: left; margin-left: 40px;">
                                    <asp:DropDownList ID="ddl1Grade3" runat="server" DataSourceID="dsGrade" DataTextField="Description"
                                        DataValueField="Code">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lbl1Grade4" runat="server" Text="Term 4:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddl1Grade4" runat="server" DataSourceID="dsGrade" DataTextField="Description"
                                        DataValueField="Code">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right; padding-left: 50px;">
                                    <asp:Label ID="lbl1Grade5" runat="server" Text="Term 5:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddl1Grade5" runat="server" DataSourceID="dsGrade" DataTextField="Description"
                                        DataValueField="Code">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right; padding-left: 50px;">
                                    <asp:Label ID="lbl1Grade6" runat="server" Text="Term 6:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddl1Grade6" runat="server" DataSourceID="dsGrade" DataTextField="Description"
                                        DataValueField="Code">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblGradeNeeded" runat="server" Text="At least 1 grade is needed per completed class"
                            ForeColor="#CC0000" Visible="False"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="lblClearClass" runat="server" ForeColor="Red" Text="You must clear the last class entered first"
                            Visible="False"></asp:Label>
                        <br />
                        <asp:Label ID="lblMustSave" runat="server" Text="Each class must be saved before you can move to the next class."></asp:Label>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnClearClass" runat="server" Text="Clear Class Info" OnClick="btnClearClass_Click"
                                        CausesValidation="False" Style="height: 26px" />
                                </td>
                                <td style="width: 150px;">
                                </td>
                                <td>
                                    <asp:Button ID="btnSaveClass" runat="server" Text="Save Class 1" OnClick="btnSaveClass_Click"
                                        Style="height: 26px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                </div>
                <br />
                <br />
                <asp:Label ID="lblNotSaved" runat="server" Font-Size="Small" Text="These buttons are for navigational purposes only. In order to save changes you make to the class information you must use the “Save Class” button before navigating to a different class."></asp:Label>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnClass1" runat="server" Text="Class 1" Enabled="False" OnClick="btnClass1_Click" />
                        </td>
                        <td style="width: 15px;">
                        </td>
                        <td>
                            <asp:Button ID="btnClass2" runat="server" Text="Class 2" Enabled="False" OnClick="btnClass2_Click" />
                        </td>
                        <td style="width: 15px;">
                        </td>
                        <td>
                            <asp:Button ID="btnClass3" runat="server" Text="Class 3" Enabled="False" OnClick="btnClass3_Click" />
                        </td>
                        <td style="width: 15px;">
                        </td>
                        <td>
                            <asp:Button ID="btnClass4" runat="server" Text="Class 4" Enabled="False" OnClick="btnClass4_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 29px">
                            <asp:Button ID="btnClass5" runat="server" Text="Class 5" Enabled="False" OnClick="btnClass5_Click" />
                        </td>
                        <td style="width: 15px; height: 29px;">
                        </td>
                        <td style="height: 29px">
                            <asp:Button ID="btnClass6" runat="server" Text="Class 6" Enabled="False" OnClick="btnClass6_Click" />
                        </td>
                        <td style="width: 15px; height: 29px;">
                        </td>
                        <td style="height: 29px">
                            <asp:Button ID="btnClass7" runat="server" Text="Class 7" Enabled="False" OnClick="btnClass7_Click" />
                        </td>
                        <td style="width: 15px; height: 29px;">
                        </td>
                        <td style="height: 29px">
                            <asp:Button ID="btnClass8" runat="server" Text="Class 8" Enabled="False" OnClick="btnClass8_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <asp:Label ID="lblCreditsCounted" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblCounter" runat="server" Text="   of 3 Science Credits Entered"></asp:Label>
                <br />
                <asp:Button ID="btnNextPage" runat="server" Text="Language Section" OnClick="btnNextPage_Click"
                    Enabled="False" CausesValidation="true" />
                <br />
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnBack" runat="server" Text="Back" Width="80px" OnClick="btnBack_Click"
                                CausesValidation="False" />
                        </td>
                        <td style="width: 85px;">
                        </td>
                        <td>
                            <asp:Button ID="btnSaveReturn" runat="server" Text="Save and Return Later" CausesValidation="false"
                                OnClick="btnSaveReturn_Click" />
                        </td>
                        <td style="width: 75px;">
                        </td>
                        <td>
                            <asp:Button ID="btnTop" runat="server" Text="Back to Top" onmousedown="javascript:scroll(0,0);"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
