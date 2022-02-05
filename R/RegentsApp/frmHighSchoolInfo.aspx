<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmHighSchoolInfo.aspx.cs"
    Inherits="RegentsApp.HighSchoolInfo" Title="Regents' Scholarship :: School Information" %>

<asp:Content ID="Content1" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_HighSchoolInfo" runat="server">
        <ContentTemplate>
            <br />
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    SaveAndReturn();
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <script language="javascript" type="text/javascript">
                function viewLabel(ddl) {
                    if (ddl.valueOf().toString() == "No") {
                        document.getElementById('<%= divNotApproved.ClientID %>').style.display = "block";
                        document.getElementById('<%= divButtons.ClientID %>').style.display = "none";
                    }
                    else {
                        document.getElementById('<%= divNotApproved.ClientID %>').style.display = "none";
                        document.getElementById('<%= divButtons.ClientID %>').style.display = "block";
                    }
                }
                function viewUniversity(sent) {
                    if (sent.toString() == "Undecided") {
                        document.getElementById("divUniversity").style.display = "block";
                    }
                    else {
                        document.getElementById("divUniversity").style.display = "none";
                    }
                }
                function viewGraduationYear(sent) {
                    if (sent.toString() == "No") {
                        document.getElementById('<%= divGradYear.ClientID %>').style.display = "block";
                        document.getElementById('<%= divButtons.ClientID %>').style.display = "none";
                    }
                    else {
                        document.getElementById('<%= divGradYear.ClientID %>').style.display = "none";
                        document.getElementById('<%= divButtons.ClientID %>').style.display = "block";
                    }
                }
                
            </script>

            <h1 style="font-family: Verdana;">
                High School Information
            </h1>
            <asp:SqlDataSource ID="dsACT" runat="server" ConnectionString="<%$ ConnectionStrings:RegentsAppConnectionStringTest %>"
                SelectCommand="SELECT * FROM [ActScoreLookup] ORDER BY [Code]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="dsCollege" runat="server" ConnectionString="<%$ ConnectionStrings:RegentsAppConnectionString %>"
                SelectCommand="SELECT [Description], [Code] FROM [CollegeLookup] ORDER BY [Description]">
            </asp:SqlDataSource>
            <div id="top" style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
                <p style="color: Red;">
                    Please help us protect your personal information by logging out of the 
                    application by using the &quot;Save and Return Later&quot; button at the bottom of the 
                    page.</p>
                <p>
                    Enter the following information. It is the student&#39;s responsibility to ensure
                    data is accurate before proceeding to the next section.</p>
                <br />
                <div style="border-width: thin; border-color: Black; border-style: solid;">
                    <h3>
                        High School Information
                    </h3>
                </div>
                <br />
                <table>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <asp:Label ID="lblWillGraduate" runat="server">Will you graduate from a Utah high school?</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <asp:DropDownList ID="ddlWillGraduate" runat="server" Width="75px" onchange="javascript: viewLabel(this.options[this.selectedIndex].value);">
                                <asp:ListItem Selected="True"></asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaWillGraduate" runat="server" ControlToValidate="ddlWillGraduate"
                                ErrorMessage="You must select whether or not you will graduate in Utah" Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <div id="divNotApproved" runat="server" style="color: Red; display: none">
                                <p>
                                    You are not eligible to receive the Regents' Scholarship.</p>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblAttendNinth" runat="server">Where did you attend 9th grade?</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlJuniorHigh" runat="server" Width="450px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaJuniorHigh" runat="server" ControlToValidate="ddlJuniorHigh"
                                ErrorMessage="Please select where you attended 9th grade" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblGraduateHigh" runat="server">Where will you graduate high school?</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlHighSchool" runat="server" Width="450px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaHighSchool" runat="server" ControlToValidate="ddlHighSchool"
                                ErrorMessage="Please select which high school you attended" Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblAttendOther" runat="server">Have you attended any other schools during grades
                    9 through 12 other than those identified above?</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlAttendOther" runat="server" Width="75px">
                                <asp:ListItem Selected="True"></asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaAttendedOther" runat="server" ControlToValidate="ddlAttendOther"
                                ErrorMessage="Please select whether or not you attended any other schools during grades 9 - 12"
                                Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblGraduatingYear" runat="server">Are you graduating in the 2011/2012 academic year?</asp:Label>
                            <br />
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlGraduationYear" runat="server" onchange="javascript: viewGraduationYear(this.options[this.selectedIndex].text);">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaGradYear" runat="server" ControlToValidate="ddlGraduationYear"
                                ErrorMessage="You must answer the question regarding graduating in the current academic year"
                                Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div id="divGradYear" runat="server" style="color: Red; display: none">
                                <p>
                                    You are not eligible to receive the Regents' Scholarship.</p>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblGradeLevel" runat="server">Which grade level will you be in when you graduate?</asp:Label>
                            <br />
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlGradeLevel" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaGradeLevel" runat="server" ControlToValidate="ddlGradeLevel"
                                ErrorMessage="You must select which grade level you will be in when you graduate"
                                Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblGPA" runat="server">Cumulative High School GPA:</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:TextBox ID="tbGPA" runat="server" MaxLength="5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaGPA" runat="server" ControlToValidate="tbGPA" ErrorMessage="Please provide your cumulative GPA"
                                Enabled="False">*</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="vaRangeGPA" runat="server" ControlToValidate="tbGPA" ErrorMessage="GPA must be between 0.00 and 4.00"
                                MaximumValue="4.000" MinimumValue="0.000">*</asp:RangeValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblCompACT" runat="server">Composite ACT Score:</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlCompACT" runat="server" DataSourceID="dsACT" DataTextField="Description"
                                Width="60px" DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaComposite" runat="server" ControlToValidate="ddlCompACT"
                                ErrorMessage="Please provide your composite ACT score" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblEngACT" runat="server">English ACT Score:</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlEngACT" runat="server" DataSourceID="dsACT" DataTextField="Description"
                                Width="60px" DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaEnglish" runat="server" ControlToValidate="ddlEngACT"
                                ErrorMessage="Please provide your English ACT score" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblMathACT" runat="server">Math ACT Score:</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlMathACT" runat="server" DataSourceID="dsACT" DataTextField="Description"
                                Width="60px" DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaMath" runat="server" ControlToValidate="ddlMathACT"
                                ErrorMessage="Please provide your Math ACT score" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblSciACT" runat="server">Science ACT Score:</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlSciACT" runat="server" DataSourceID="dsACT" DataTextField="Description"
                                Width="60px" DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaScience" runat="server" ControlToValidate="ddlSciACT"
                                ErrorMessage="Please provide your Science ACT score" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <br />
                            <asp:Label ID="lblReadACT" runat="server">Reading ACT Score:</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlReadACT" runat="server" DataSourceID="dsACT" DataTextField="Description"
                                Width="60px" DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaReading" runat="server" ControlToValidate="ddlReadACT"
                                ErrorMessage="Please provide your Reading ACT score" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 400px;">
                            <asp:Label ID="lblUnivPlanAttend" runat="server">Which Utah college or university
                    do you plan on attending?</asp:Label>
                        </td>
                        <td style="text-align: left; padding-left: 10px;">
                            <br />
                            <asp:DropDownList ID="ddlUnivPlanAttend" runat="server" DataSourceID="dsCollege"
                                onchange="javascript: viewUniversity(this.options[this.selectedIndex].text);"
                                DataTextField="Description" Width="225px" DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaAttend" runat="server" ControlToValidate="ddlUnivPlanAttend"
                                ErrorMessage="Please select which college or university you plan to attend" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div id="divUniversity" style="display: none; color: Red;">
                                Reminder: You must attend an eligible Utah college or university to be eligible
                                for the Regents' Scholarship. The eligible Utah institutions are listed in the drop
                                down above.
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="SchoolValidSummary" runat="server" />
                <div id="divButtons" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnInfoNextPage" runat="server" Text="Next Page" OnClick="btnInfoNextPage_Click"
                                    Style="height: 26px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Width="80px"
                                CausesValidation="False" />
                        </td>
                        <td style="width: 85px;">
                        </td>
                        <td>
                            <asp:Button ID="btnInfoSaveReturn" runat="server" Text="Save and Return Later" OnClick="btnInfoSaveReturn_Click"
                                CausesValidation="false" />
                        </td>
                        <td style="width: 75px;">
                        </td>
                        <td>
                            <asp:Button ID="btnTop" runat="server" Text="Back to Top" onmousedown="javascript:scroll(0,0);"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
