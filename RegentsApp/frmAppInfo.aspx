<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmAppInfo.aspx.cs"
    Inherits="RegentsApp.WebForm1" Title="Regents' Scholarship Application :: Application Information" %>

<asp:Content ID="Content1" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_AppInfo" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>
            <link href="RegentsStyle.css" rel="Stylesheet" type="text/css" />

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;
                text-align: center;">
                <img id="Img1" src="Resources/2012-Header-No-Back.jpg" alt="Regents' Scholarship"
                    runat="server" />
                <h3 style="color: #853535;">
                    Continue reading to learn what documents must be submitted to have a completed application
                    file.
                </h3>
                <p>
                    In order to have a complete Regents’ Scholarship application file you must submit
                    the online application <u>along with the supporting documents</u> as outlined below.
                    The purpose of the supporting documents is to provide evidence that what you have
                    reported on the online portion of the scholarship application is true and accurate.
                    Therefore, all requirements related to GPA, grades, course work you have and will
                    complete, and an ACT score must be accounted for on the supporting documents you
                    submit. <b>Your consideration for the scholarship will be forfeited if you do not submit
                        <u>all</u> required documentation by the application deadline.</b>
                </p>
                <h3 style="text-align: left; color: #4967a4;">
                    LIST OF SUPPORTING DOCUMENTS</h3>
                <div style="text-align: left;">
                    <ol>
                        <li style="margin-bottom: 1em;"><b>A Regents&#39; Scholarship Course Form.</b> As the
                            applicant, it is your responsibility to download, complete, and submit this form
                            as part of your scholarship application file by the deadline.
                            <br />
                            <br />
                            <ul>
                                <li style="margin-bottom: 1em;">A link to the <b>Regents’ Scholarship Course Form</b>
                                    is available on the <b>post-submission page</b> of the online-application.</li>
                                <li style="margin-bottom: 1em;"><b>Substitute schedules or incomplete forms will not
                                    be accepted.</b></li>
                                <li style="margin-bottom: 1em;">The form must be filled out <u>correctly and completely</u>,
                                    including both the student and counselor signatures certifying the information provided
                                    is accurate and complete.</li>
                                <li style="margin-bottom: 1em;">If you change your schedule after you have submitted
                                    your documentation, you must submit a new form by February 1, 2012. Schedule and
                                    course changes after February 1, may have a negative impact on your ability to qualify
                                    for the scholarship.</li>
                            </ul>
                        </li>
                        <li style="margin-bottom: 1em;"><b>An official high school transcript,</b> which 
                            includes your cumulative grade point average (GPA).</li>
                        <li style="margin-bottom: 1em;"><b>An ACT score.</b> This can be a photo copy of the 
                            student’s official score report, or it can be the ACT scores listed on the high 
                            school transcript. However, as the applicant, it is your responsibility to make 
                            sure that this documentation is submitted.</li>
                        <li style="margin-bottom: 1em;"><b>An official college transcript(s). If you have 
                            earned college credit for any English, Math, Social Science, Science or 
                            World/Classical Language course completed during grades 9, 10, and 11 you are 
                            required to submit an official college transcript at this time.</b>
                            <br />
                            <br />
                            <ul>
                                <li>The official college transcript is required even if the course is listed on the
                                    official high school transcript. </li>
                                <br />
                                <br />
                                <li>The official college transcript is required regardless of how you earned the college
                                    credit, whether through concurrent enrollment, distance education, or regular college
                                    or university enrollment.</b> </li>
                                <br />
                            </ul>
                        </li>
                        <li style="margin-bottom: 1em;"><b>Other official transcripts.</b>
                            <br />
                        <br />
                            <ul>
                        <li>This is required if you completed any of the required courses through an 
                            approved private institution/business entity such as BYU Independent Study; or 
                            if you have transferred from a private or out of state school.</li>
                        <br />
                        <br />
                        <li>This is required even if the course is listed on the official high school transcript.</li>
                        <br />
                        <br />
                                <li>Note: this is not applicable to course work completed through Electronic High School</li>
                            </ul>
                        </li>
                    </ol>
                </div>
                <p>
                    Please note that verification of your lawful presence in the United States will
                    be required if you are found "on track" for the Regents’ Scholarship. Your submittal
                    of this application certifies, under penalty of perjury, that you are a U.S. citizen
                    or a qualified alien that is lawfully present in the U.S.</p>
                <p>
                    If you have questions regarding the scholarship requirements <a target="_blank" href="http://www.higheredutah.org/scholarship_info/regents-scholarship/">
                        click here</a>.</p>
            </div>
            <br />
            <br />
            <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
