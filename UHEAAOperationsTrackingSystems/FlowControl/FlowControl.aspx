<%@ Page Language="C#" MasterPageFile="~/UHEAAOperationsTrackingSystems.Master" AutoEventWireup="true"
    CodeBehind="FlowControl.aspx.cs" Inherits="UHEAAOperationsTrackingSystems.FlowControl"
    Title="ACDC Flows" %>

<%@ Register Src="UserControls/FlowStepControl.ascx" TagName="FlowStepControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/FlowControlReadOnly.ascx" TagName="FlowControlReadOnly"
    TagPrefix="uc2" %>
<%@ Register Src="UserControls/FlowStepControlWithFlowInfoReadOnly.ascx" TagName="FlowStepControlWithFlowInfoReadOnly"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="Stylesheet" href="FlowControlSpecific.css" type="text/css" />
    <link rel="shortcut icon" href="../Shared/Images/FlowControlFavicon.ico" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin: 25px; text-align: left">
                <asp:Button ID="btnFlowManagement" runat="server" Text="Flow Management" CssClass="FlowManagementTabButton"
                    OnClick="btnFlowManagement_Click" />
                <asp:Button ID="btnStepManagement" runat="server" Text="Step Management" CssClass="StepManagementTabButton"
                    OnClick="btnStepManagement_Click" />
                <asp:Button ID="btnResearchFlowSteps" runat="server" Text="System Flow Steps" CssClass="ResearchFlowStepsTabButton"
                    OnClick="btnResearchFlowSteps_Click" />
                <asp:Button ID="btnStepStaffAssignment" runat="server" Text="Staff Assignments" CssClass="StepStaffAssignmentTabButton"
                    OnClick="btnStepStaffAssignment_Click" />
                <asp:MultiView ID="FlowMultiView" runat="server" ActiveViewIndex="0">
                    <asp:View ID="FlowManagementTab" runat="server">
                        <div class="FlowManagementTab">
                            <div class="TabPartHeader">
                                <asp:Label ID="Label15" runat="server" Text="Add Flow:"></asp:Label>
                            </div>
                            <br />
                            <table style="width: 100%; text-align: left;">
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label16" runat="server" Text="Flow ID:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddFlow_FlowID" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAddFlow_FlowID"
                                            ErrorMessage="* Required Field" ValidationGroup="AddFlow">* Required Field</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label1" runat="server" Text="System:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbAddFlow_System" runat="server" Height="22px" Width="155px">
                                            <asp:ListItem>Please Select...</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="cmbAddFlow_System"
                                            ErrorMessage="* Required Field" ValidationGroup="AddFlow" InitialValue="Please Select...">* Required Field</asp:RequiredFieldValidator>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label17" runat="server" Text="Control Display Text:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddFlow_ControlTextDisplay" runat="server" MaxLength="200"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label18" runat="server" Text="User Interface Display ID:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddFlow_UserInterfaceDisplayId" runat="server" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label19" runat="server" Text="Description:"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAddFlow_Description"
                                            ErrorMessage="* Required Field" ValidationGroup="AddFlow">* Required Field</asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddFlow_Description" runat="server" CssClass="DescriptionTextBoxes"
                                            MaxLength="8000" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <asp:Button ID="btnAddFlow" runat="server" Text="Add Flow" ValidationGroup="AddFlow"
                                            OnClick="btnAddFlow_Click" />
                                        <br />
                                        <asp:Label ID="lblAddFlowResult" runat="server" Text="###Error Goes Here###" ForeColor="Red"
                                            Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="TabPartHeader">
                                <asp:Label ID="Label2" runat="server" Text="Change Flow:"></asp:Label>
                            </div>
                            <br />
                            <div style="text-align: center">
                                <table width="100%">
                                    <tr>
                                        <td width="50%" style="text-align: right; margin-right: 5px">
                                            <asp:Label ID="lblChangeFlow_FlowID" runat="server" Text="Flow ID To Change:"></asp:Label>
                                        </td>
                                        <td width="50%" style="text-align: left; margin-left: 5px">
                                            <asp:DropDownList ID="cmbChangeFlow_FlowID" Height="22px" Width="155px" runat="server"
                                                AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="ObjectDataSourceFlowIDs"
                                                DataTextField="DisplayText" DataValueField="BackgroundValue" OnSelectedIndexChanged="cmbChangeFlow_FlowID_SelectedIndexChanged">
                                                <asp:ListItem>Please Select...</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="ObjectDataSourceFlowIDs" runat="server" SelectMethod="GetFlowIDs"
                                                TypeName="UHEAAOperationsTrackingSystems.FlowControlDataAccess"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <table style="width: 100%; text-align: left;">
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label4" runat="server" Text="System:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbChangeFlow_System" runat="server" Height="22px" 
                                            Width="155px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label5" runat="server" Text="Control Display Text:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChangeFlow_ControlDisplayText" runat="server" MaxLength="200"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label6" runat="server" Text="User Interface Display ID:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChangeFlow_UIDisplayID" runat="server" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 210px">
                                        <asp:Label ID="Label7" runat="server" Text="Description:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChangeFlow_Description" runat="server" CssClass="DescriptionTextBoxes"
                                            MaxLength="8000" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <asp:Button ID="btnChangeFlow" runat="server" Text="Change Flow" OnClick="btnChangeFlow_Click"
                                            Enabled="False" />
                                        <br />
                                        <asp:Label ID="lblChangeFlowResponse" runat="server" Text="###Error Goes Here###"
                                            ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>
                    <asp:View ID="StepManagementTab" runat="server">
                        <div class="StepManagementTab">
                            <div class="TabPartHeader">
                                <asp:Label ID="Label3" runat="server" Text="Manage Flow Steps:"></asp:Label>
                            </div>
                            <br />
                            <div style="text-align: center">
                                <table width="100%">
                                    <tr>
                                        <td width="50%" style="text-align: right; margin-right: 5px">
                                            <asp:Label ID="Label8" runat="server" Text="Flow ID To Change:"></asp:Label>
                                        </td>
                                        <td width="50%" style="text-align: left; margin-left: 5px">
                                            <asp:DropDownList ID="cmbStepManage_FlowID" Height="22px" Width="155px" runat="server"
                                                AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="ObjectDataSourceFlowIDs"
                                                DataTextField="DisplayText" DataValueField="BackgroundValue" OnSelectedIndexChanged="cmbStepManage_FlowID_SelectedIndexChanged"
                                                ValidationGroup="AddStep">
                                                <asp:ListItem>Please Select...</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="cmbStepManage_FlowID"
                                                ErrorMessage="* Required" InitialValue="Please Select..." ValidationGroup="AddStep">* Required</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; margin-right: 5px">
                                            <asp:Label ID="Label12" runat="server" Text="System:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; margin-left: 5px">
                                            <asp:Label ID="lblStepManage_System" runat="server" CssClass="FlowSequenceNumberLabel"
                                                BorderStyle="Inset" BorderWidth="2px" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; margin-right: 5px">
                                            <asp:Label ID="Label13" runat="server" Text="Description:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; margin-left: 5px">
                                            <asp:Label ID="lblStepManage_Description" runat="server" CssClass="FlowSequenceNumberLabel"
                                                BorderStyle="Inset" BorderWidth="2px" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="TabPartHeader">
                                <asp:Label ID="Label9" runat="server" Text="Add Step:"></asp:Label>
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                    <td width="50%">
                                        <table style="width: 100%; text-align: left;">
                                            <tr>
                                                <td style="width: 210px">
                                                    <asp:Label ID="Label11" runat="server" Text="Business Unit Access Only:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbStepManage_BusinessUnit" runat="server" Width="155" Height="22"
                                                        ValidationGroup="AddStep">
                                                        <asp:ListItem>Please Select...</asp:ListItem>
                                                        <asp:ListItem Value="True">True</asp:ListItem>
                                                        <asp:ListItem Value="False">False</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="cmbStepManage_BusinessUnit"
                                                        ErrorMessage="* Required" InitialValue="Please Select..." ValidationGroup="AddStep">* Required</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label20" runat="server" Text="Notification Key:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbStepManage_NotificationKey" runat="server" Height="22px"
                                                        Width="155px" DataSourceID="ObjectDataSourceNotificationKeys" DataTextField="Name"
                                                        DataValueField="Name" AppendDataBoundItems="True">
                                                        <asp:ListItem>Please Select...</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceNotificationKeys" runat="server" SelectMethod="GatherExistingKeysBasedOffFilterCriteria"
                                                        TypeName="UHEAAOperationsTrackingSystems.FlowControlDataAccess">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lblStepManage_System" Name="system" PropertyName="Text"
                                                                Type="String" />
                                                            <asp:Parameter DefaultValue="Notification" Name="type" Type="String" />
                                                            <asp:Parameter Name="keyWord" Type="String" DefaultValue="" />
                                                            <asp:Parameter Name="keyWordFieldFilter" Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label21" runat="server" Text="Control Display Text:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStepManage_ControlDisplayText" runat="server" MaxLength="200"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label28" runat="server" Text="Data Validation ID:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStepManage_DataValidationID" runat="server" MaxLength="50"></asp:TextBox>
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
                                                    <asp:DropDownList ID="cmbStepManage_AccessKey" runat="server" Height="22px" Width="155px"
                                                        DataSourceID="ObjectDataSourceAccessKeys" DataTextField="Name" DataValueField="Name"
                                                        AppendDataBoundItems="True">
                                                        <asp:ListItem>Please Select...</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceAccessKeys" runat="server" SelectMethod="GatherExistingKeysBasedOffFilterCriteria"
                                                        TypeName="UHEAAOperationsTrackingSystems.DataAccessBase">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lblStepManage_System" Name="system" PropertyName="Text"
                                                                Type="String" />
                                                            <asp:Parameter DefaultValue="Access" Name="type" Type="String" />
                                                            <asp:Parameter ConvertEmptyStringToNull="False" DefaultValue="" Name="keyWord" Type="String" />
                                                            <asp:Parameter ConvertEmptyStringToNull="False" Name="keyWordFieldFilter" Type="String" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label26" runat="server" Text="Staff Assignment:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbStepManage_StaffAssignment" runat="server" Height="22px"
                                                        Width="155px" DataSourceID="ObjectDataSourceUsers" 
                                                        DataTextField="LegalName" DataValueField="ID"
                                                        AppendDataBoundItems="True" AutoPostBack="True" 
                                                        OnSelectedIndexChanged="cmbStepManage_StaffAssignment_SelectedIndexChanged">
                                                        <asp:ListItem>Please Select...</asp:ListItem>
                                                        <asp:ListItem>Use Listed Calculation</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:ObjectDataSource ID="ObjectDataSourceUsers" runat="server" SelectMethod="GetSystemUsers"
                                                        TypeName="UHEAAOperationsTrackingSystems.DataAccessBase"></asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblStaffAssignmentCalcID" runat="server" Text="Staff Assignment Calculation ID:"
                                                        Enabled="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStepManage_StaffAssignmentCalculationID" runat="server" Enabled="False"
                                                        MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label29" runat="server" Text="Status:"
                                                        Enabled="True"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStepManage_Status" runat="server" Enabled="True"
                                                        MaxLength="50"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtStepManage_Description" runat="server" CssClass="DescriptionTextBoxes"
                                                        MaxLength="8000" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center">
                                        <asp:Button ID="btnAddStep" runat="server" Text="Add Step To Flow" OnClick="btnAddStep_Click"
                                            ValidationGroup="AddStep" />
                                        <br />
                                        <asp:Label ID="lblAddStepResponse" runat="server" Text="###Error Goes Here###" ForeColor="Red"
                                            Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <div class="TabPartHeader">
                                <asp:Label ID="Label10" runat="server" Text="Existing Steps:"></asp:Label>
                            </div>
                            <asp:Repeater ID="rptSteps" runat="server">
                                <ItemTemplate>
                                    <div style="background-color: #666666">
                                        <uc1:FlowStepControl ID="FlowStepControl1" runat="server" FlowID='<%# DataBinder.Eval (Container.DataItem, "FlowID").ToString() %>'
                                            AccessAlsoBasedOffBusinessUnit='<%# DataBinder.Eval (Container.DataItem, "AccessAlsoBasedOffBusinessUnit").ToString() %>'
                                            NotificationKey='<%# DataBinder.Eval (Container.DataItem, "NotificationKey") %>'
                                            AccessKey='<%# DataBinder.Eval (Container.DataItem, "AccessKey") %>' 
                                            StaffAssignment='<%# DataBinder.Eval (Container.DataItem, "StaffAssignment") %>'
                                            FlowStepSequenceNumber='<%# DataBinder.Eval (Container.DataItem, "FlowStepSequenceNumber") %>'
                                            ControlDisplayText='<%# DataBinder.Eval (Container.DataItem, "ControlDisplayText") %>'
                                            Description='<%# DataBinder.Eval (Container.DataItem, "Description") %>' 
                                            StaffAssignmentCalculationID='<%# DataBinder.Eval (Container.DataItem, "StaffAssignmentCalculationID") %>'
                                            Status='<%# DataBinder.Eval (Container.DataItem, "Status") %>'
                                            DataValidationID='<%# DataBinder.Eval (Container.DataItem, "DataValidationID") %>' />
                                    </div>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <div style="background-color: #999999">
                                        <uc1:FlowStepControl ID="FlowStepControl2" runat="server" FlowID='<%# DataBinder.Eval (Container.DataItem, "FlowID").ToString() %>'
                                            AccessAlsoBasedOffBusinessUnit='<%# DataBinder.Eval (Container.DataItem, "AccessAlsoBasedOffBusinessUnit").ToString() %>'
                                            NotificationKey='<%# DataBinder.Eval (Container.DataItem, "NotificationKey") %>'
                                            AccessKey='<%# DataBinder.Eval (Container.DataItem, "AccessKey") %>' 
                                            StaffAssignment='<%# DataBinder.Eval (Container.DataItem, "StaffAssignment") %>'
                                            FlowStepSequenceNumber='<%# DataBinder.Eval (Container.DataItem, "FlowStepSequenceNumber") %>'
                                            ControlDisplayText='<%# DataBinder.Eval (Container.DataItem, "ControlDisplayText") %>'
                                            Description='<%# DataBinder.Eval (Container.DataItem, "Description") %>' 
                                            StaffAssignmentCalculationID='<%# DataBinder.Eval (Container.DataItem, "StaffAssignmentCalculationID") %>'
                                            Status='<%# DataBinder.Eval (Container.DataItem, "Status") %>'
                                            DataValidationID='<%# DataBinder.Eval (Container.DataItem, "DataValidationID") %>' />
                                    </div>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </div>
                    </asp:View>
                    <asp:View ID="ResearchFlowTab" runat="server">
                        <div class="TabPartHeader">
                            <asp:Label ID="Label23" runat="server" Text="Research System Flows:"></asp:Label>
                        </div>
                        <div class="ResearchFlowStepsTab">
                            <table width="100%">
                                <tr>
                                    <td width="50%" style="text-align: right">
                                        <asp:Label ID="Label22" runat="server" Text="System:"></asp:Label>
                                    </td>
                                    <td width="50%" style="text-align: left">
                                        <asp:DropDownList ID="cmbSystemSteps_System" runat="server" Height="22px" Width="155px"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Repeater ID="RepeaterSystemFlows" runat="server" DataSourceID="ObjectDataSourceSystemFlows">
                                            <ItemTemplate>
                                                <uc2:FlowControlReadOnly ID="FlowControlReadOnly1" runat="server" 
                                                    FlowID='<%# DataBinder.Eval (Container.DataItem, "FlowID") %>'
                                                    TheSystem='<%# DataBinder.Eval (Container.DataItem, "System") %>' 
                                                    Description='<%# DataBinder.Eval (Container.DataItem, "Description") %>'
                                                    ControlDisplayText='<%# DataBinder.Eval (Container.DataItem, "ControlDisplayText") %>'
                                                    UserInterfaceDisplayIndicator='<%# DataBinder.Eval (Container.DataItem, "UserInterfaceDisplayIndicator") %>'
                                                     />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:ObjectDataSource ID="ObjectDataSourceSystemFlows" runat="server" SelectMethod="GetFlowsForSystem"
                                            TypeName="UHEAAOperationsTrackingSystems.FlowControlDataAccess">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="cmbSystemSteps_System" Name="system" PropertyName="SelectedValue"
                                                    Type="String" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>
                    <asp:View ID="StepStaffAssignmentTab" runat="server">
                        <div class="TabPartHeader">
                            <asp:Label ID="Label27" runat="server" Text="Research Staff Assignments To Flows:"></asp:Label>
                        </div>
                        <div class="StepStaffAssignmentTab">
                            <table width="100%">
                                <tr>
                                    <td width="50%" style="text-align: right">
                                        <asp:Label ID="Label25" runat="server" Text="User :"></asp:Label>
                                    </td>
                                    <td width="50%" style="text-align: left">
                                        <asp:DropDownList ID="cmbUserSearch_User" runat="server" DataSourceID="ObjectDataSourceUsers"
                                            DataTextField="LegalName" DataValueField="ID" Height="22px" Width="155px"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Repeater ID="RepeaterUserFlowInfo" runat="server" DataSourceID="ObjectDataSourceForUserFlows">
                                            <ItemTemplate>
                                                <hr />
                                                <br />
                                                <uc3:FlowStepControlWithFlowInfoReadOnly ID="FlowStepControlWithFlowInfoReadOnly1"
                                                    runat="server" FlowID='<%# DataBinder.Eval (Container.DataItem, "FlowID") %>'
                                                    FlowStepSequenceNumber='<%# DataBinder.Eval (Container.DataItem, "FlowStepSequenceNumber") %>'
                                                    AccessAlsoBasedOffBusinessUnit='<%# DataBinder.Eval (Container.DataItem, "AccessAlsoBasedOffBusinessUnit").ToString() %>'
                                                    AccessKey='<%# DataBinder.Eval (Container.DataItem, "AccessKey") %>' 
                                                    NotificationKey='<%# DataBinder.Eval (Container.DataItem, "NotificationKey") %>'
                                                    StaffAssignment='<%# DataBinder.Eval (Container.DataItem, "StaffAssignmentLegalName") %>'
                                                    ControlDisplayText='<%# DataBinder.Eval (Container.DataItem, "ControlDisplayText") %>'
                                                    StepDescription='<%# DataBinder.Eval (Container.DataItem, "StepDescription") %>'
                                                    FlowDescription='<%# DataBinder.Eval (Container.DataItem, "FlowDescription") %>'
                                                    TheSystem='<%# DataBinder.Eval (Container.DataItem, "TheSystem") %>'
                                                    DataValidationID='<%# DataBinder.Eval (Container.DataItem, "DataValidationID") %>'
                                                    Status='<%# DataBinder.Eval (Container.DataItem, "Status") %>' />
                                                <br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:ObjectDataSource ID="ObjectDataSourceForUserFlows" runat="server" SelectMethod="GetUsersFlowInfo"
                                            TypeName="UHEAAOperationsTrackingSystems.FlowControlDataAccess">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="cmbUserSearch_User" Name="user" PropertyName="SelectedValue"
                                                    Type="String" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="50">
                <ProgressTemplate>
                    <div class="Processing">
                        <br />
                        Processing . . .<br />
                        <br />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
