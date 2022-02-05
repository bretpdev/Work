<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalAppOption.ascx.cs"
    Inherits="UHEAAOperationsTrackingSystems.PortalAppOption" %>
<asp:LinkButton  ID="LinkButtonForAppImage" runat="server" CssClass="AppControl">
    <div style="width: 100%; text-align: center;">
        <div class="AppImage"><img src='<%= ApplicationImageFileNameAndPath %>' alt='<%= ApplicationNameText %>' /></div>
        <div class="AppText"><%= ApplicationNameText %></div>
    </div>
</asp:LinkButton>
