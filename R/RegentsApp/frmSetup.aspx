<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmSetup.aspx.cs"
    Inherits="RegentsApp.Setup" Title="Regents' Scholarship Application :: Account Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_Setup" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <script type="text/javascript" src="swfobject.js"></script>

            <div id="ytapiplayer">
                You need Flash player 8+ and JavaScript enabled to view this video.
            </div>

            <script type="text/javascript">

                var params = { allowScriptAccess: "always", allowFullScreen: "true" };
                var atts = { id: "myytplayer" };
                swfobject.embedSWF("/Regents.swf", "ytapiplayer", "768", "415", "8", null, null, params, atts, StartTimer);

                function StartTimer() {
                    setTimeout(OpenMainDiv, 412000);
                }
                
                function OpenMainDiv() {
                    document.getElementById('<%= mainDiv.ClientID %>').style.display = "block";
                    document.getElementById('<%= setup.ClientID %>').style.display = "none";
                }
                
            </script>
            
            <div id="setup" runat="server" style="display: inline;">
                <p>The account setup will be available after you watch the video.</p>
            </div>

            <h1 style="font-family: Verdana;">
                Account Setup
            </h1>
            <br />
            <br />
            <div id="mainDiv" runat="server" style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;
                display: none; text-align: center;">
                <h3>
                    Account setup is easy! Simply create a username and password. Then select your security
                    questions.
                </h3>
                <br />
                <p>
                    Consider the following while setting up your username and password:</p>
                <ul style="text-align: left;">
                    <li>Your username needs to be easy to remember, unique, and contain at least <b>6 characters.</b></li>
                    <li>Your password needs to be at least <b>6 characters</b> long and include both letters
                        and numbers.</li>
                    <li>The email address you select will be used to communicate important information.</li>
                    <li>Security questions will be used to help you recover password and username information
                        as needed.</li>
                    <li>Make a note of your username in a safe place and never share your password with
                        anyone.</li>
                </ul>
                <asp:SqlDataSource ID="dsSecQuestion" runat="server" ConnectionString="<%$ ConnectionStrings:RegentsAppConnectionString %>"
                    SelectCommand="SELECT * FROM [SecurityLookup]"></asp:SqlDataSource>
                <br />
                <br />
                <table>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblFirstName" runat="server" Text="First Name:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtFirstName" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaFirstName" runat="server" ErrorMessage="First Name is required"
                                ControlToValidate="txtFirstName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblMiddleName" runat="server" Text="Middle Name:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtMiddleName" runat="server" Width="250px" MaxLength="25"></asp:TextBox>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblLastName" runat="server" Text="Last Name:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtLastName" runat="server" Width="250px" MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Last Name is required"
                                ControlToValidate="txtLastName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblDOB" runat="server" Text="Date of Birth:"></asp:Label>
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtDOB" runat="server" Width="250px" MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaDOB" runat="server" ErrorMessage="Date of Birth is required"
                                ControlToValidate="txtFirstName">*</asp:RequiredFieldValidator>
                        <asp:Label ID="lblDOBFormat" runat="server" Text="MM/DD/YYYY" Enabled="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:RegularExpressionValidator ID="vaRegDob" runat="server" ControlToValidate="txtDOB"
                                ErrorMessage="You entered a birth date that is invalid. Please correct this and try again."
                                ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"
                                Font-Size="Smaller"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblUserName" runat="server" Text="Username:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbUserName" runat="server" Width="250px" MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaUserName" runat="server" ErrorMessage="Username is required"
                                ControlToValidate="tbUserName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr style="height: 15px;">
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="text-align: left; color: Red;">
                            <asp:Label ID="lblNameUsed" runat="server" Text="User name is already in use" Visible="False"
                                Font-Size="Smaller"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbPassword" runat="server" Width="250px" TextMode="Password" MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaPassword" runat="server" ErrorMessage="Password is required"
                                ControlToValidate="tbPassword">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 200px;">
                            <asp:Label ID="lblPassConf" runat="server" Text="Password Confirmation:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbPassConf" runat="server" Width="250px" TextMode="Password" MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaPassConfReq" runat="server" ControlToValidate="tbPassConf"
                                ErrorMessage="Confirmation Password required">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:RegularExpressionValidator ID="vaPassChar" runat="server" ControlToValidate="tbPassword"
                                ErrorMessage="Password must be at least 6 characters and have at least 1 number"
                                Font-Size="Smaller" ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"></asp:RegularExpressionValidator>
                            <br />
                            <asp:CompareValidator ID="vaPassConf" runat="server" ControlToCompare="tbPassword"
                                ControlToValidate="tbPassConf" ErrorMessage="Passwords do not match" Font-Size="Smaller">Passwords do not match</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblEmlAddy" runat="server" Text="Email Address:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbEmlAddy" runat="server" Width="250px" MaxLength="56"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaEmailAddy" runat="server" ControlToValidate="tbEmlAddy"
                                ErrorMessage="Email required">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Not a valid email address"
                                ValidationExpression="[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}" ControlToValidate="tbEmlAddy">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblEmlConf" runat="server" Text="Email Confirmation:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbEmlConf" runat="server" Width="250px" MaxLength="56"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaEmailConfReq" runat="server" ControlToValidate="tbEmlConf"
                                ErrorMessage="Confirmation Email required">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:CompareValidator ID="vaEmlComf" runat="server" ControlToCompare="tbEmlAddy"
                                ControlToValidate="tbEmlConf" ErrorMessage="Email Address do not match" Font-Size="Smaller">Email 
                    addresses do not match</asp:CompareValidator>
                            <br />
                            <asp:Label ID="lblEmailUsed" runat="server" Font-Size="Smaller" ForeColor="#CC3300"
                                Text="The Email address you are using is already registered with another account. Please use another email address and submit again.  If you have already set up your account and forgotten your password, &lt;a href=&quot;frmForgotPassword.aspx&quot;&gt;click here.&lt;/a&gt;"
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblSecQ1" runat="server" Text="Security Question #1:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlSecQ1" runat="server" Height="22px" Width="255px" DataSourceID="dsSecQuestion"
                                DataTextField="Description" DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaQuestion1" runat="server" ControlToValidate="ddlSecQ1"
                                ErrorMessage="Question 1 required">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblAns1" runat="server" Text="Answer #1:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbAns1" runat="server" Width="250px" MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaAns1" runat="server" ErrorMessage="Answer #1 required"
                                ControlToValidate="tbAns1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblSecQ2" runat="server" Text="Security Question #2:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlSecQ2" runat="server" Height="22px" Width="255px" DataSourceID="dsSecQuestion"
                                DataTextField="Description" DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaQuestion2" runat="server" ControlToValidate="ddlSecQ2"
                                ErrorMessage="Question 2 required">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 190px;">
                            <asp:Label ID="lblAns2" runat="server" Text="Answer #2:"></asp:Label><br />
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbAns2" runat="server" Width="250px" MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaAns2" runat="server" ErrorMessage="Answer #2 required"
                                ControlToValidate="tbAns2">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:CompareValidator ID="vaCompare" runat="server" ControlToCompare="ddlSecQ1" ControlToValidate="ddlSecQ2"
                                ErrorMessage="Please choose a different question" Font-Size="Smaller" Operator="NotEqual"></asp:CompareValidator>
                            <br />
                            <asp:Label ID="lblQuestionsNeeded" runat="server" Text="Please choose two questions"
                                Font-Size="Smaller" ForeColor="Red" Visible="False"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnSubmitSetup" runat="server" Text="Submit" Width="123px" OnClick="btnSubmitSetup_Click" />
                <asp:ValidationSummary ID="ValidationSummary" runat="server" />
                <br />
                <br />
                <asp:LinkButton ID="btnHome" PostBackUrl="~/Default.aspx" runat="server" CausesValidation="false">Home</asp:LinkButton>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
