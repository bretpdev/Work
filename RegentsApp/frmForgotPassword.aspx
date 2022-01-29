<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmForgotPassword.aspx.cs"
    Inherits="RegentsApp.ForgotPassword" Title="Regents' Application :: Forgot Password?" %>

<asp:Content ID="contentForgotPassword" ContentPlaceHolderID="regentsPlaceHolder"
    runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
                <br />
                <h1>
                    Forgot Username or Password?</h1>
                <p>
                    No problem! Answer two questions about yourself, and we will help you remember 
                    your login information.</p>
                <br />
                <p>
                    Enter your username or email address so we can locate your security questions.</p>
                <asp:Label ID="lblInvalidAccount" runat="server" Visible="false" Text="USER NAME OR EMAIL ADDRESS NOT ON FILE"
                    ForeColor="#CC3300"></asp:Label>
                <table>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblUserName" runat="server">Username:</asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbUserName" runat="server" MaxLength="25" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            OR
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblEmail" runat="server">Email Address:</asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbEmail" runat="server" MaxLength="56" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="btnFindUser" runat="server" Text="Submit" OnClick="btnFindUser_Click"
                    Width="150px" CausesValidation="False" />
                <br />
                <br />
                <table>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblSeq1" runat="server" Text="Security Question #1:" Visible="False"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblQuestion1" runat="server" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblAnswer1" runat="server" Text="Answer #1:" Visible="False"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbAnswer1" runat="server" Width="200" MaxLength="25" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblInvalidAnswer1" runat="server" Visible="False" Text="Answer #1 is incorrect"
                                ForeColor="#CC3300"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblSeq2" runat="server" Text="Security Question #2:" Visible="False"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblQuestion2" runat="server" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblAnswer2" runat="server" Text="Answer #1:" Visible="False"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbAnswer2" runat="server" Width="200" MaxLength="25" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblInvalidAnswer2" runat="server" Visible="False" Text="Answer #2 is incorrect"
                                ForeColor="#CC3300"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Button ID="btnSecuritySubmit" runat="server" Text="Submit" Width="150" OnClick="btnSecuritySubmit_Click"
                    Visible="False" />
                <br />
                <br />
                <asp:Label ID="lblResetPass" runat="server" Visible="False">
            Thank you. Your username information is displayed below. Please reset your password
            and click "submit" to log into your application. Remember that your password must be at
            least 6 characters long and contain at least one number. Make a note of your username
            in a safe place and never share your password with anyone.
                </asp:Label>
                <br />
                <br />
                <table>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblUserNameFound" runat="server" Text="Username:" Visible="False"></asp:Label>
                        </td>
                        <td style="text-align: left;" width="250">
                            <asp:TextBox ID="tbUserNameFound" runat="server" Width="200" Visible="False" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblNewPass" runat="server" Text="New Password:" Visible="False"></asp:Label>
                        </td>
                        <td style="text-align: left;" width="250">
                            <asp:TextBox ID="tbNewPass" runat="server" Width="200" Visible="False" EnableTheming="True"
                                TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaNewPass" runat="server" ControlToValidate="tbNewPass"
                                Enabled="False" ErrorMessage="New password is required">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblConfPass" runat="server" Text="Confirm Password:" Visible="False"></asp:Label>
                        </td>
                        <td style="text-align: left;" width="250">
                            <asp:TextBox ID="tbConfPass" runat="server" Width="200" Visible="False" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaConfPass" runat="server" ControlToValidate="tbNewPass"
                                Enabled="False" ErrorMessage="Confirmation password required">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left;" width="250">
                            <asp:RegularExpressionValidator ID="vaPassChar" runat="server" ControlToValidate="tbNewPass"
                                ErrorMessage="Password must be at least 6 characters and have at least 1 number"
                                Font-Size="Smaller" ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z]).*$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:CompareValidator ID="vaCompare" runat="server" ControlToCompare="tbNewPass"
                                ControlToValidate="tbConfPass" ErrorMessage="Passwords do not match" Font-Size="Smaller"></asp:CompareValidator>
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                <asp:Button ID="btnLogin" runat="server" Text="Submit" Visible="False" OnClick="btnLogin_Click"
                    Width="150px" />
                <br />
                <br />
                <asp:LinkButton ID="btnHome" PostBackUrl="~/Default.aspx" runat="server" CausesValidation="false">Home</asp:LinkButton>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
