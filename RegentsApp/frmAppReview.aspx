<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmAppReview.aspx.cs"
    Inherits="RegentsApp.AppReview" Title="Regents' Scholarship :: Application Review" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="AppReviewContent" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_AppReview" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <br />

            <script language="javascript" type="text/javascript">
                window.onload=function() {
                    if(!NiftyCheck())
                        return;
                    Rounded("div#MainContainer","Transparent","White","large");
                    DisableBackButton();
                }
                
                document.onkeydown = showDown;
                
                function showDown(){
                    if (event.keycode == 116){
                        cancelKey(evt);
                    }
                }
                
                function DisableBackButton() {
                    window.history.forward();
                }
                setTimeout("DisableBackButton", 0);
                
                window.onunload=function() {
                    DisableBackButton();
                }
            </script>

            <div style="font-family: Verdana; width: 700px; text-align: center; margin-left: auto;
                margin-right: auto;">
                <h2 style="font-family: Verdana;">
                    Application Review
                </h2>
                <div style="text-align: left;">
                    <p>
                        <b>Carefully review the information below to ensure it is accurate.</b> It is important
                        that all information is correct upon initial submission. If all the information
                        is correct, you must click on the button below in order to finalize your application.
                    </p>
                    <p>
                        If you do not have all the information to complete the application, use the &quot;Save
                        and Return Later&quot; button on this page. This will allow you to collect the remaining
                        information and log back into your application to complete it.</p>
                    <p>
                        <b>Note:</b> <u>Once you submit your application, you will not be able to use this site
                            to make changes to your application information.</u> You may still log into 
                        your account to view and print a copy of the information you submitted until 
                        February 1, 2012; however, no changes to the application will be permitted.
                    </p>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Width="80px"
                                    CausesValidation="False" />
                            </td>
                            <td style="width: 85px;">
                            </td>
                            <td>
                                <asp:Button ID="btnSaveReturn" runat="server" Text="Save and Return Later" OnClick="btnSaveReturn_Click"
                                    CausesValidation="false" />
                            </td>
                            <td style="width: 75px;">
                            </td>
                            <td>
                                <asp:Button ID="btnTop" runat="server" Text="Scroll Down" onmousedown="javascript:scroll(0,2000);" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnNextPage" runat="server" Text="All information is correct. Continue to finalize my application."
                                    OnClick="btnNextPage_Click" Style="height: 45px; width: 500px; cursor: pointer;"
                                    Font-Bold="True" Font-Size="Medium" ForeColor="Black" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <div style="width: 600px; margin-left: auto; margin-right: auto; text-align: left;">
                        <CR:CrystalReportViewer ID="ApplicationViewer" runat="server" AutoDataBind="True"
                            DisplayGroupTree="False" EnableDatabaseLogonPrompt="False" EnableDrillDown="False"
                            EnableParameterPrompt="False" Height="7131px" ReportSourceID="CrystalReportSource"
                            SeparatePages="False" ShowAllPageIds="True" Width="634px" DisplayToolbar="False" />
                        <br />
                        <CR:CrystalReportSource ID="CrystalReportSource" runat="server">
                            <Report FileName="Reporting\CompletedApplication.rpt">
                            </Report>
                        </CR:CrystalReportSource>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
