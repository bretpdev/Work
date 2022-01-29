<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmCreditInstructions.aspx.cs"
    Inherits="RegentsApp.CreditInstructions" Title="Regents' Scholarship :: Class Credit Instructions" %>

<asp:Content ID="ClassCreditInfo" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_ClassCreditInfo" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <h1 style="font-family: Verdana;">
                Class Credit Instructions
            </h1>
            <div id="top" style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
                <p>
                    On the following pages, you will enter information about the courses you have completed,
                    are currently taking and will complete during <b>grades 9 - 12</b> that fulfill
                    the requirements for the Regents' Scholarship. You must enter enough information
                    to fulfill the minimum requirement for each category. The application will not let
                    you continue until you have entered the required credits for each category. If you
                    have completed additional credits beyond what is required, you should include this
                    information as part of your application. The minimum requirements are as follows:</p>
                <ul style="text-align: left;">
                    <li style="margin-bottom: 1em;">4 credits of English</li>
                    <li style="margin-bottom: 1em;">4 progressive credits of Mathematics (including Algebra
                        1, Geometry, Algebra 2, and one class beyond Algebra 2)</li>
                    <li style="margin-bottom: 1em;">3.5 credits of Social Science</li>
                    <li style="margin-bottom: 1em;">3 credits of lab-based Science (this must including 
                        one Biology, one Chemistry, and one Physics course.)</li>
                    <li style="margin-bottom: 1em;">2 progressive credits of the same world or classical
                        Language, other than English.</li>
                </ul>
                <h4 style="text-align: left;">
                    Selecting the Right Classes:</h4>
                <ul style="text-align: left;">
                    <li style="margin-bottom: 1em;">You will be choosing courses titles from a drop-down
                        menu. Select the course that most closely matches your transcript. If there is not
                        a course title similar to what is on your transcript use the "other" option to report
                        the course.</li>
                    <li style="margin-bottom: 1em;">If the course is an Advanced Placement course you must
                        select the "Advanced Placement" button. This will ensure the correct course names
                        are displayed for you to select.</li>
                    <li style="margin-bottom: 1em;">If the course is a concurrent enrollment or college
                        course, you must select the "Concurrent Enrollment/College" button. This will ensure
                        the correct course names are displayed for you to select. By selecting this button
                        you are indicating that you have earned college credit for the course.</li>
                    <ul>
                        <br />
                        <li style="margin-bottom: 1em;">If you completed or will complete a college or concurrent
                            enrollment course worth 3 or more college credits record the credit earned as 1
                            high school credit. If your high school transcript identifies the class as being
                            worth more than 1 credit, report the credit value as stated on your high school
                            transcript.</li>
                        <li style="margin-bottom: 1em;"><b>NOTE: If you have earned college credit for any 
                            course completed in grades 9, 10 or 11</b> you are required to submit a college transcript
                            at this time. </li>
                    </ul>
                    <li style="margin-bottom: 1em;">It is imperative that you indicate the appropriate grade
                        level in which you completed each course.</li>
                </ul>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnInstructionNextPage" runat="server" Text="Next Page" OnClick="btnInfoNextPage_Click"
                                Style="height: 26px" Width="80px" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Width="80px"
                                CausesValidation="False" />
                        </td>
                        <td style="width: 85px;">
                        </td>
                        <td>
                            <asp:Button ID="btnInstructionSaveReturn" runat="server" Text="Save and Return Later"
                                OnClick="btnInfoSaveReturn_Click" CausesValidation="false" />
                        </td>
                        <td style="width: 75px;">
                        </td>
                        <td>
                            <asp:Button ID="btnInstructionTop" runat="server" Text="Back to Top" onmousedown="javascript:scroll(0,0);"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
