<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowStepControl.ascx.cs"
    Inherits="UHEAAOperationsTrackingSystems.FlowStepControl" %>
    
<table style="width: 100%;">
    <tr>
        <td width="50%">
            <table style="width: 100%; text-align: left;">
                <tr>
                    <td style="width: 210px">
                        <asp:Label ID="Label11" runat="server" Text="Business Unit Access Only:"></asp:Label>
                        <asp:HiddenField ID="hfFlowID" runat="server" Visible="False" Value='<%# FlowID %>' />
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbBusinessUnit" runat="server" Width="155" Height="22"
                            SelectedValue='<%# AccessAlsoBasedOffBusinessUnit %>'>
                            <asp:ListItem Value="True">True</asp:ListItem>
                            <asp:ListItem Value="False">False</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label20" runat="server" Text="Notification Key:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbNotificationKey" runat="server" Height="22px"
                            Width="155px" DataSourceID="ObjectDataSourceNotificationKeys" DataTextField="Name"
                            DataValueField="Name" SelectedValue='<%# NotificationKey %>' 
                            AppendDataBoundItems="True">
                            <asp:ListItem>Please Select...</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label21" runat="server" Text="Control Display Text:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtControlDisplayText" runat="server" 
                            Text='<%# ControlDisplayText %>' MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                                                    <asp:Label ID="Label28" runat="server" 
                            Text="Data Validation ID:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDataValidationID" runat="server" 
                            Text='<%# DataValidationID %>' MaxLength="50"></asp:TextBox>
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
                        <asp:DropDownList ID="cmbAccessKey" runat="server" Height="22px" Width="155px"
                            DataSourceID="ObjectDataSourceAccessKeys" DataTextField="Name" DataValueField="Name"
                            SelectedValue='<%# AccessKey %>' AppendDataBoundItems="True">
                            <asp:ListItem>Please Select...</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label26" runat="server" Text="Staff Assignment:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbStaffAssignment" runat="server" Height="22px"
                            Width="155px" DataSourceID="ObjectDataSourceUsers" DataTextField="LegalName" DataValueField="ID"
                            SelectedValue='<%# StaffAssignment %>' AppendDataBoundItems="True" 
                            AutoPostBack="True" 
                            onselectedindexchanged="cmbStaffAssignment_SelectedIndexChanged" 
                            ondatabound="cmbStaffAssignment_DataBound">
                            <asp:ListItem>Please Select...</asp:ListItem>
                            <asp:ListItem>Use Listed Calculation</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStaffAssignmentCalculationID" runat="server" 
                            Text="Staff Assignment Calculation ID:" Enabled="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStaffAssignmentCalculationID" runat="server" 
                            Text='<%# StaffAssignmentCalculationID %>' Enabled="False" 
                            MaxLength="100" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" 
                            Text="Status:" Enabled="True"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStatus" runat="server" 
                            Text='<%# Status %>' Enabled="True" MaxLength="50" ></asp:TextBox>
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
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="DescriptionTextBoxes"
                            MaxLength="8000" TextMode="MultiLine" Text='<%# Description %>'></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="text-align: Left">
            <asp:Button ID="btnDelete" runat="server" Text="Delete" 
                CssClass="WideLeftMarginButtons" onclick="btnDelete_Click" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" 
                CssClass="WideLeftMarginButtons" onclick="btnUpdate_Click" />
            <asp:Label ID="lblStepChangeUpdateResponse" runat="server" 
                Text="Step has been updated." Visible="False" CssClass="WideLeftMarginButtons" 
                ForeColor="Red"></asp:Label>
        </td>
        <td style="text-align: right;">
            <asp:Label ID="lblFlowStepSequenceNumber" runat="server" CssClass="FlowSequenceNumberLabel"
                Text='<%# FlowStepSequenceNumber %>'
                Font-Bold="True" Font-Size="Larger" BorderStyle="Inset" BorderWidth="2" BackColor="White"></asp:Label>
            <asp:ImageButton ID="btnMoveUp" runat="server" 
                ImageUrl="/Shared/Images/ARW03UP.ICO" onclick="btnMoveUp_Click" 
                Height="30px" Width="30px" />
            <asp:ImageButton ID="btnMoveDown" runat="server" 
                ImageUrl="/Shared/Images/ARW03DN.ICO" onclick="btnMoveDown_Click" 
                Height="30px" Width="30px" />
        </td>
    </tr>
</table>
 
