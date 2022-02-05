<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowStepControlReadOnly.ascx.cs" Inherits="UHEAAOperationsTrackingSystems.FlowStepControlReadOnly" %>
<table style="width: 100%;">
    <tr>
        <td width="50%">
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="width: 210px">
                        <asp:Label ID="Label11" runat="server" Text="Business Unit Access Only:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text='<%# AccessAlsoBasedOffBusinessUnit %>' CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label20" runat="server" Text="Notification Key:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text='<%# NotificationKey %>' CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label21" runat="server" Text="Control Display Text:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text='<%# ControlDisplayText %>' CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label27" runat="server" Text="Data Validation ID:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label28" runat="server" Text='<%# DataValidationID %>' 
                            CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td width="50%">
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="width: 210px">
                        <asp:Label ID="Label24" runat="server" Text="Access Key:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text='<%# AccessKey %>' CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label26" runat="server" Text="Staff Assignment:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text='<%# StaffAssignment %>' CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Staff Assignment Calculation</td>
                    <td>
                        <asp:Label ID="Label29" runat="server" Text='<%# StaffAssignmentCalculationID %>' CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        
                        <asp:Label ID="Label7" runat="server" Text="Status:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text='<%# Status %>' CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding: 0px">
            <table width="100%" style="border-width: 0px; text-align: left">
                <tr>
                    <td width="210px">
                        <asp:Label ID="Label14" runat="server" Text="Description:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text='<%# Description %>' CssClass="FlowSequenceNumberLabel"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>