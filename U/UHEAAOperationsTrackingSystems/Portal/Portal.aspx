<%@ Page Language="C#" MasterPageFile="~/UHEAAOperationsTrackingSystems.Master" AutoEventWireup="true"
    CodeBehind="Portal.aspx.cs" Inherits="UHEAAOperationsTrackingSystems.Portal"
    Title="UHEAA Operations Portal" %>

<%@ Register Assembly="UHEAAOperationsTrackingSystems" Namespace="UHEAAOperationsTrackingSystems.Shared.CustomControls"
    TagPrefix="cc2" %>
<%@ Register Src="PortalAppOption.ascx" TagName="PortalAppOption" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <title>UHEAA Operations Portal</title>
    <link rel="Stylesheet" href="/Default.css" type="text/css" />
    <link rel="shortcut icon" href="../Shared/Images/PortalFavicon.ico" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="ImageDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <uc1:PortalAppOption ID="PortalAppOption2" runat="server" UHEAASystem="ACDC Flows"
                        UHEAAAccessKey="Portal Access" ApplicationNameText="Flows" ApplicationImageFileNameAndPath="../Shared/Images/FlowControl.gif"
                        ApplicationMainPageNameAndPath="/FlowControl/FlowControl.aspx" />
                    <uc1:PortalAppOption ID="PortalAppOption5" runat="server" ApplicationNameText="Uheaa University"
                        ApplicationImageFileNameAndPath="../Shared/Images/UheaaUniversityIcon.gif" ApplicationMainPageNameAndPath="http://10.10.10.88" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
