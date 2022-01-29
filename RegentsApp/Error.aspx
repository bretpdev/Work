<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs"
    Inherits="RegentsApp.Error" Title="Regents' Application :: ERROR" %>

<asp:Content ID="ErrorContent" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
        <h2>
            We apologize but there was an error saving your application.<br />
            <br />
            Please log back into your application and try again.<br />
            <br />
            If you continue to have problems, please call customer service at 1(877) 336 -7378.
        </h2>
        <br />
        <br />
        <div style="border: 1px solid black; padding: 10px 10px 10px 10px; width: 250px;">
            <a href="frmLogin.aspx">Click here to log back in</a></div>
    </div>
</asp:Content>
