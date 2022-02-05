<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmPostSubmit.aspx.cs"
    Inherits="RegentsApp.PostSubmit" Title="Regents' Application :: Post Submission" %>

<asp:Content ID="contentPostSubmit" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_PostSubmit" runat="server">
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
            
                function OpenPDF() {
                    window.open("2012_Course_Form.pdf");
                }
            
            </script>

            <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
                <h1 style="font-family: Verdana;">
                    Post Submission Page
                </h1>
                <h4>
                    Thank you for submitting the online portion of the Regents’ Scholarship. <b style="color: Red;">
                        You still need to submit the supporting documents as outlined below.</b></h4>
                <h4>
                    As the applicant, it is your responsibility to submit all required documentation
                    by the deadline. <i>Your consideration for the scholarship will be forfeited if you
                        do not submit <u>all</u> required documentation by the application deadline.</i>
                </h4>
                <div style="border: solid 1px black; background-color: #f6d05e; padding: 0px 10px 0px 10px;">
                    <h4 style="text-align: left; color: #4967a4;">
                        DEADLINES:</h4>
                    <p>
                        All Regents' Scholarship application materials must be postmarked by the priority
                        deadline <b>December 21, 2011</b> or postmarked by the final application deadline
                        of <b>February 1, 2012. <u>Late documents are not accepted. Incomplete files will not
                            be reviewed.</u> Faxed or emailed documents are not accepted.</b> Processing
                        of documents takes 2-3 business days. During peak times additional processing time
                        may be needed.
                    </p>
                </div>
                <h4 style="text-align: left; color: #4967a4;">
                    LIST OF SUPPORTING DOCUMENTS</h4>
                <div style="text-align: left;">
                    <ol>
                        <li style="margin-bottom: 1em;"><b>The Regents&#39; Scholarship Course Form is available 
                            for download here. </b>
                            <button id="btnCourseForm" style="font-size: large; font-weight: bold" onclick="javascript: OpenPDF();" />
                            Course Form</button></li>
                        <ul>
                            <br />
                            <li><b>Substitute schedules or incomplete forms will not be accepted.</b></li>
                            <br />
                            <br />
                            <li>The form must be filled out <u>correctly and completely</u>, including both the
                                student and counselor signatures certifying the information provided is accurate
                                and complete. </li>
                            <br />
                            <br />
                            <li>If you change your schedule after you have submitted your documentation, you must
                                submit a new form by February 1, 2012. Schedule and course changes after February
                                1, may have a negative impact on your ability to qualify for the scholarship.</li>
                        </ul>
                        <li style="margin-bottom: 1em;"><b>An official high school transcript,</b> which includes
                            your cumulative grade point average (GPA).</li>
                        <li style="margin-bottom: 1em;"><b>An ACT score.</b> This can be a photo copy of the 
                            official score report, or it can be the ACT scores listed on the high school 
                            transcript. However; as the applicant, it is your responsibility to make sure 
                            that this documentation is provided.</li>
                        <li style="margin-bottom: 1em;"><b>An official college transcript(s). If you have 
                            earned college credit for any English, Math, Social Science, Science or Foreign 
                            Language course during grades 9, 10, and 11 you are required to submit an 
                            official college transcript at this time.</b>
                            <br />
                            <br />
                            <ul>
                                <li>The official college transcript is required even if the course is listed on the 
                                    official high school transcript. </li>
                                <br />
                                <br />
                                <li>The official college transcript is required regardless of how you earned the college
                                    credit, as you may have earned college credit through concurrent enrollment, early
                                    college, distance education, or if you completed the course on your own at a college
                                    or university.</li>
                                <br />
                            </ul>
                            <li style="margin-bottom: 1em;"><b>Other official transcripts.</b></li>
                            <ul>
                                <br />
                                <li>This is required if you completed any of the required courses through an approved
                                    private institution/business entity such as BYU Independent Study; or if you have
                                    transferred from a private or out of state school.</li>
                                <br />
                                <br />
                                <ul>
                                    <li>Some districts have subcontracted with private online institution/business 
                                        entities to offer online courses. If the course work you completed is offered in 
                                        such a manner, you are required to submit the transcript from the private entity 
                                        where the credit has originated.</li>
                                </ul>
                                <br />
                                <li>This is required even if the course is listed on the official high school 
                                    transcript.</li>
                                <br />
                                <br />
                                <li>Note: this is not applicable to course work completed through Electronic High School.</li>
                            </ul>
                        </li>
                    </ol>
                </div>
                <h4 style="text-align: left; color: #4967a4;">
                    SUBMITTING YOUR APPLICATION</h4>
                <p>
                    <b>It is highly recommended</b>, if possible, that the applicant collect all required
                    documents and submit them in one envelope to the address below as well as, <b>keep a
                        copy of all documents submitted.</b> Certified mail is strongly encouraged as
                    a way to track submitted documents. The mailing address is:</p>
                <p>
                    Utah System of Higher Education
                    <br />
                    Regents' Scholarship<br />
                    PO Box 145114<br />
                    Salt Lake City, Utah 84114-5114</p>
                <h4 style="text-align: left; color: #4967a4;">
                    TIMELINE OF REVIEW</h4>
                <p>
                    There are two phases to the review process for the Regents’ Scholarship, the 
                    initial review and the final review. Once your application has been submitted, 
                    it will be reviewed to see if you are “on-track” to qualify for the scholarship. 
                    You will be notified of the outcome of the initial review by <b>May 1.</b> If you have not received
                    notification by May 1, please contact our office by calling 801-321-7294. Instructions
                    regarding the final review will be included in the notification to those who are
                    found to be “on-track”.</p>
                <p>
                    <b>To protect your personal information, log out and close all browser screens.</b></p>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnLogout" runat="server" OnClick="btnLogout_Click" Text="Logout" />
                        </td>
                        <td style="border-style: none; width: 200px;">
                        </td>
                        <td>
                            <input id="btnPrint" onclick="window.open('Report.aspx');" type="button" value="Print Application" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
