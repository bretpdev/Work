<%@ Page Language="C#" MasterPageFile="~/UHEAAOperationsTrackingSystems.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="UHEAAOperationsTrackingSystems.Default"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>Portal Login</title>
    <link rel="Stylesheet" href="Default.css" type="text/css" />
    <link rel="shortcut icon" href="Shared/Images/ACDCfavicon.ico" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div class="ImageDiv">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <table style="width: 100%">
            <tr>
                <td style="width: 50%" align="right">
                    <asp:Label ID="Label1" runat="server" Text="User Name" ForeColor="White"></asp:Label>
                </td>
                <td style="width: 50%" align="left">
                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" Text="Password" ForeColor="White"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="75"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblBadLoginInfo" runat="server" ForeColor="Red" Text="Either an invalid user name or password was provided.  Please try again."
                        Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
