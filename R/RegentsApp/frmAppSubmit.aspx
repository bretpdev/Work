<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmAppSubmit.aspx.cs"
    Inherits="RegentsApp.AppSubmit" Title="Regents' Application :: Submit Application" %>

<asp:Content ID="contentAppSubmit" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_AppSubmit" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <script type="text/javascript" language="javascript">
                function SubmitReady() {
                    var verifyTrue = document.getElementById('<%= chkVerifyTrue.ClientID %>');
                    var verifyPostmarked = document.getElementById('<%= chkVerifyPostmark.ClientID %>');
                    var verifyIdentity = document.getElementById('<%= chkVerifyIdentity.ClientID %>');
                    var verifyLawful = document.getElementById('<%= chkVerifyLawful.ClientID %>');
                    var verifyCitizen = document.getElementById('<%= chkVerifyCitizen.ClientID %>');
                    var verifyTranscript = document.getElementById('<%= chkVerifyTranscript.ClientID %>');
                    if (verifyTrue.checked && verifyPostmarked.checked && verifyIdentity.checked && verifyLawful.checked && verifyCitizen.checked && verifyTranscript.checked) {
                        document.getElementById('<%= btnSubmit.ClientID %>').disabled = false;
                    }
                    else {
                        document.getElementById('<%= btnSubmit.ClientID %>').disabled = true;
                    }
                }
            </script>

            <div id="Top" style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
                <h1>
                    Application Submission</h1>
                <p>
                    Acknowledge each of the statements below by checking the boxes provided.&nbsp;By
                    marking each statement, you are verifying that you have read and acknowledge the
                    statement. Then click on the &quot;Submit My Application&quot; button to officially
                    submit the online portion of your application.</p>
                <p>
                    Remember, you must also submit supporting documentation to have a complete scholarship
                    application file.</p>
                <br />
                <table>
                    <tr>
                        <td style="text-align: right;" valign="top">
                            <asp:CheckBox ID="chkVerifyTrue" runat="server" onclick="javascript:SubmitReady();" />
                        </td>
                        <td style="text-align: left;" valign="top">
                            <ul>
                                <li>I verify that all information submitted in this application is true and accurate.</li></ul>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" valign="top">
                            <asp:CheckBox ID="chkVerifyPostmark" runat="server" onclick="javascript:SubmitReady();" />
                        </td>
                        <td style="text-align: left;" valign="top">
                            <ul>
                                <li>I understand all required documentation must be postmarked by December 21, 2011
                                    to meet the priority deadline or by February 1, 2012 to meet the final deadline.</li></ul>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" valign="top">
                            <asp:CheckBox ID="chkVerifyIdentity" runat="server" onclick="javascript:SubmitReady();" />
                        </td>
                        <td style="text-align: left;" valign="top">
                            <ul>
                                <li>I verify that I have represented my identity honestly.</li></ul>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" valign="top">
                            <asp:CheckBox ID="chkVerifyLawful" runat="server" onclick="javascript:SubmitReady();" />
                        </td>
                        <td style="text-align: left;" valign="top">
                            <ul>
                                <li>I acknowledge that I will need to verify my lawful presence in the United States
                                    should I be found "on track" and accept the Regents&#39; Scholarship</li></ul>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" valign="top">
                            <asp:CheckBox ID="chkVerifyCitizen" runat="server" onclick="javascript:SubmitReady();" />
                        </td>
                        <td style="text-align: left;" valign="top">
                            <ul>
                                <li>I understand that by submitting this application I am certifying, under penalty
                                    of perjury, that I am a U.S. citizen or a qualified alien that is lawfully present
                                    in the United States.</li></ul>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;" valign="top">
                            <asp:CheckBox ID="chkVerifyTranscript" runat="server" onclick="javascript:SubmitReady();" />
                        </td>
                        <td style="text-align: left;" valign="top">
                            <ul>
                                <li>I understand that at this time I am required to submit an official college transcript
                                    if I have earned college credit for any English, Math, Social Science, Science or
                                    World/Classical Language complete during grades 9, 10 and 11 as part of my supporting
                                    document. This is required even if the course is on my high school transcript. This
                                    is required regardless of how I earned the college credit, whether through concurrent
                                    enrollment, distance education, or regular college or university enrollment.</li></ul>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <br />
                            <asp:Button ID="btnSubmit" runat="server" Enabled="false" Text="Submit My Application"
                                OnClick="btnSubmit_Click" Width="200px" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CausesValidation="False"
                                Text="Back" Width="100px" />
                        </td>
                        <td style="width: 125px;">
                        </td>
                        <td>
                            <asp:Button ID="btnSaveReturn" runat="server" CausesValidation="false" OnClick="btnSaveReturn_Click"
                                Text="Save and Return Later" Width="200px" />
                        </td>
                        <td style="width: 125px;">
                        </td>
                        <td>
                            <asp:Button ID="btnTop" runat="server" onmousedown="javascript:scroll(0,0);" CausesValidation="False"
                                Text="Back to Top" Width="100px" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
