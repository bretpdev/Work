<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs"
    Inherits="RegentsApp.Login" Title="Regents' Scholarship :: Login" %>

<asp:Content ID="loginPage" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_Login" runat="server">
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
                <h1>
                    Regents' Scholarship Application Login
                </h1>
                <br />
                <br />
                <h3>
                    Welcome back! Please enter your username and password below to access your application.</h3>
                <asp:Label ID="lblWelcome" runat="server" Visible="False"></asp:Label>
                <br />
                <br />
                <table>
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
                <br />
                <asp:LinkButton ID="lbtn" PostBackUrl="~/frmForgotPassword.aspx" runat="server" CausesValidation="False">Forgot your username or password?</asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
