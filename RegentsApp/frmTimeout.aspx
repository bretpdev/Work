﻿<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmTimeout.aspx.cs"
    Inherits="RegentsApp.Timeout" Title="Regents' Application :: Session Timed Out" %>

<asp:Content ID="contentTimeout" ContentPlaceHolderID="regentsPlaceHolder" runat="server">

    <script type="text/javascript" language="javascript">
        function WindowClose() {
            window.close();
        }
    </script>

    <br />
    <br />
    <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
        <h2>
            Regents' Scholarship Application<br />
            Session Timeout</h2>
        <br />
        <p>
            You have been logged out of your application due to inactivity. The information
            you have submitted so far has been saved. Please ensure that you complete the entire
            application and submit it prior to the application deadline of February 1, 2012.
            You will not be able to access your application after the deadline passes.</p>
        <br />
        <p>
            <b>To protect your personal information, close this browser window.</b></p>
        <br />
        <br />
        <div style="border: 1px solid black; padding: 10px 10px 10px 10px; width: 250px; text-align:center; margin-left:auto; margin-right:auto;">
            <a href="frmLogin.aspx">Click here to log back in</a></div>
        <br />
        <br />
        <asp:Button ID="btnClose" runat="server" Text="Close Window" onmousedown="javascript: WindowClose();" />
        <br />
        <br />
    </div>
</asp:Content>
