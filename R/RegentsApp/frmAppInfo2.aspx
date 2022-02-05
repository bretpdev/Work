<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmAppInfo2.aspx.cs"
    Inherits="RegentsApp.frmAppInfo2" Title="Regents' Scholarship Application :: Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_AppInfo2" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>
            <link href="RegentsStyle.css" rel="Stylesheet" type="text/css" />

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <script type="text/javascript" language="javascript">
            
                window.onload=function(){
                    if(!NiftyCheck())
                        return;
                    Rounded("div#Login","Transparent","Silver","large");
                }
                
            </script>

            <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;
                text-align: center;">
                <img id="Img1" src="Resources/2012-Header-No-Back.jpg" alt="Regents' Scholarship"
                    runat="server" />
                <h3 style="text-align: left; color: #4967a4;">
                    SUBMITTING YOUR APPLICATION</h3>
                <p>
                    <b>It is highly recommended</b>, if possible, that the applicant collect all required
                    documents and submit them in one envelope to the address below. It is suggested
                    that the applicant <b>keep a copy of all documents submitted.</b> Certified mail
                    is strongly encouraged as a way to track submitted documents. The mailing address
                    is:</p>
                <p>
                    Utah System of Higher Education<br />
                    Regents' Scholarship<br />
                    PO Box 145114<br />
                    Salt Lake City, Utah 84114-5114</p>
                <h3 style="text-align: left; color: #4967a4;">
                    TIMELINE OF REVIEW</h3>
                <p>
                    There are two phases to the review process for the Regents’ Scholarship, the 
                    initial review and the final review. Once your application has been submitted, 
                    it will be reviewed to determine if you are “on-track” to qualify for the 
                    scholarship. You will be notified of the outcome of the initial review by <b>May 1.</b> If you have
                    not received notification by May 1, please contact our office at 801-321-7294. Instructions
                    regarding the final review will be included in the notification to those who are
                    found to be “on-track”.</p>
                <h3 style="text-align: left; color: #4967a4;">
                    START YOUR APPLICATION</h3>
                <p>
                    In an effort to improve your understanding of the application process you will 
                    be required to watch a tutorial video before you begin the online application. 
                    After you have watched the video you will create a username and password and 
                    select your security questions. This will allow you to save your application and 
                    return to it as needed before you submit it. <b>If you would like to print a copy
                    of your application you will need:</b>
                </p>
                <table>
                    <tr>
                        <td style="text-align: left;">
                            <p>
                                1) <a target="_blank" href="http://get.adobe.com/reader/">Adobe reader </a>
                                <br />
                                2) Access to a printer.
                            </p>
                        </td>
                        <td>
                            <a target="_blank" href="http://get.adobe.com/reader/">
                                <img src="Resources/get_adobe_reader.jpg" alt="Get Adobe Reader" /></a>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="Login" style="width: 400px; margin-left: auto; margin-right: auto;">
                    <div style="background-color: Silver;">
                        <b class="rtop"><b class="r1"></b><b class="r2"></b><b class="r3"></b><b class="r4">
                        </b></b>
                        <table>
                            <tr>
                                <td style="width: 45%; text-align: left; margin-top: -15px;">
                                    <h3>
                                        Sign In Here</h3>
                                </td>
                                <td style="width: 45%; color: Blue; text-align: right;">
                                    <h5>
                                        <a href="frmForgotPassword.aspx">Forgot your password?</a></h5>
                                </td>
                            </tr>
                        </table>
                        <p style="border-top: 1px dashed black; width: 94%; margin-top: -10px;">
                        </p>
                        <table style="margin-top: -20px;">
                            <tr>
                                <td style="text-align: right;">
                                </td>
                                <td style="width: 10px;">
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblLoginError" runat="server" ForeColor="Red" Font-Size="Smaller"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblLoginUserName" runat="server" Text="Username:"></asp:Label>
                                </td>
                                <td style="width: 10px;">
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="tbLoginUserName" runat="server" Width="200px" MaxLength="25"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="vaLoginUserName" runat="server" ControlToValidate="tbLoginUserName"
                                        ErrorMessage="Username required">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblLoginPassword" runat="server" Text="Password:"></asp:Label>
                                </td>
                                <td style="width: 10px;">
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="tbLoginPassword" runat="server" Width="200px" TextMode="Password"
                                        MaxLength="25"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="vaLoginPassword" runat="server" ControlToValidate="tbLoginPassword"
                                        ErrorMessage="Password required">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                        <br />
                        <asp:Button ID="btnLoginSubmit" runat="server" Text="Login" OnClick="btnLoginSubmit_Click" />
                        <br />
                        <br />
                        <asp:LinkButton ID="btnHome" PostBackUrl="~/Default.aspx" runat="server" CausesValidation="false">Home</asp:LinkButton>
                        <br />
                        <a href="frmSetup.aspx">Create Account</a> <b class="rbottom"><b class="r4"></b><b
                            class="r3"></b><b class="r2"></b><b class="r1"></b></b>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
