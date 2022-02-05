<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlowControlReadOnly.ascx.cs"
    Inherits="UHEAAOperationsTrackingSystems.FlowControlReadOnly" %>
<%@ Register Src="FlowStepControlReadOnly.ascx" TagName="FlowStepControlReadOnly"
    TagPrefix="uc1" %>
<br />
<table style="width: 100%; text-align: left;">
    <tr>
        <td style="width: 210px">
            <asp:Label ID="Label16" runat="server" Text="Flow ID:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblFlowID" runat="server" Text='<%# FlowID %>'></asp:Label>
            <asp:HiddenField ID="hflSystem" runat="server" Value='<%# TheSystem %>' />
        </td>
    </tr>
    <tr>
        <td style="width: 210px">
            <asp:Label ID="Label17" runat="server" Text="Control Display Text:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblControlDisplayText" runat="server" Text='<%# ControlDisplayText %>'></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 210px">
            <asp:Label ID="Label18" runat="server" Text="User Interface Display ID:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblUserInterfaceDisplayID" runat="server" Text='<%# UserInterfaceDisplayIndicator %>'></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 210px">
            <asp:Label ID="Label19" runat="server" Text="Description:"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDescription" runat="server" Text='<%# Description %>'></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center">
            <asp:Button ID="btnHideViewSteps" runat="server" Text="View Steps" OnClick="btnViewSteps_Click" />
        </td>
    </tr>
</table>
<div style="border-style: inset;">
    <asp:Repeater ID="RepeaterSteps" runat="server">
        <ItemTemplate>
            <div style="background-color: #666666">
                <uc1:FlowStepControlReadOnly ID="FlowStepControlReadOnly1" runat="server" FlowID='<%# DataBinder.Eval (Container.DataItem, "FlowID").ToString() %>'
                    AccessAlsoBasedOffBusinessUnit='<%# DataBinder.Eval (Container.DataItem, "AccessAlsoBasedOffBusinessUnit").ToString() %>'
                    NotificationKey='<%# DataBinder.Eval (Container.DataItem, "NotificationKey") %>'
                    AccessKey='<%# DataBinder.Eval (Container.DataItem, "AccessKey") %>' 
                    StaffAssignment='<%# DataBinder.Eval (Container.DataItem, "StaffAssignmentLegalName") %>'
                    StaffAssignmentCalculationID='<%# DataBinder.Eval (Container.DataItem, "StaffAssignmentCalculationID") %>' 
                    FlowStepSequenceNumber='<%# DataBinder.Eval (Container.DataItem, "FlowStepSequenceNumber") %>'
                    ControlDisplayText='<%# DataBinder.Eval (Container.DataItem, "ControlDisplayText") %>'
                    Description='<%# DataBinder.Eval (Container.DataItem, "Description") %>' 
                    DataValidationID='<%# DataBinder.Eval (Container.DataItem, "DataValidationID") %>'
                    Status='<%# DataBinder.Eval (Container.DataItem, "Status") %>' />
            </div>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <div style="background-color: #999999">
                <uc1:FlowStepControlReadOnly ID="FlowStepControlReadOnly2" runat="server" FlowID='<%# DataBinder.Eval (Container.DataItem, "FlowID").ToString() %>'
                    AccessAlsoBasedOffBusinessUnit='<%# DataBinder.Eval (Container.DataItem, "AccessAlsoBasedOffBusinessUnit").ToString() %>'
                    NotificationKey='<%# DataBinder.Eval (Container.DataItem, "NotificationKey") %>'
                    AccessKey='<%# DataBinder.Eval (Container.DataItem, "AccessKey") %>' 
                    StaffAssignment='<%# DataBinder.Eval (Container.DataItem, "StaffAssignmentLegalName") %>'
                    StaffAssignmentCalculationID='<%# DataBinder.Eval (Container.DataItem, "StaffAssignmentCalculationID") %>' 
                    FlowStepSequenceNumber='<%# DataBinder.Eval (Container.DataItem, "FlowStepSequenceNumber") %>'
                    ControlDisplayText='<%# DataBinder.Eval (Container.DataItem, "ControlDisplayText") %>'
                    Description='<%# DataBinder.Eval (Container.DataItem, "Description") %>' 
                    DataValidationID='<%# DataBinder.Eval (Container.DataItem, "DataValidationID") %>'
                    Status='<%# DataBinder.Eval (Container.DataItem, "Status") %>' />
            </div>
        </AlternatingItemTemplate>
    </asp:Repeater>
</div>
