<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="RegentsApp.Default" Title="Regents' Scholarship Application :: Home" %>

<asp:Content ID="defaultContent" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_Default" runat="server">
        <ContentTemplate>
            <style type="text/css">
                .showHide
                {
                    display: none;
                    font-weight: normal;
                    color: Black;
                    top: 300px;
                    left: 400px;
                    width: 300px;
                    position: fixed;
                    padding: 1em;
                    background: #FFC;
                    border: 1px solid black;
                }
            </style>

            <script type="text/javascript">

                function showItem(item) {
                    document.getElementById(item).style.display = "block";
                    return false;
                }
                function hideItem(item) {
                    document.getElementById(item).style.display = "none";
                    return false;
                }

                function OpenPDF() {
                    window.open("College_Trans_Sample.pdf");
                }
            
            </script>

            <div class="showHide" id="bodyplan">
                <p>
                    A State Student Identification Number is assigned to students attending public schools,
                    and should be found on your high school transcript or may be obtained from your
                    high school guidance counselor. For students attending a private school use the
                    first four letters of your last name and your six digit birthday. For example, if
                    your last name is "Brown" and your birthday is on January 15, 1992, the number you
                    would use is brow011592. If your last name is less than four letters, use your full
                    last name and your six digit birthday. Failure to enter the correct identification
                    number could result in a delay of processing your application.</p>
                <h4>
                    <a href="#" onclick='return hideItem("bodyplan");'>Close</a></h4>
            </div>
            <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
                <img id="Img1" src="Resources/2012-Header-No-Back.jpg" alt="Regents' Scholarship"
                    runat="server" />
                <h3 style="color: #853535;">
                    Welcome to the Regents' Scholarship application for high school students graduating
                    in 2012.
                </h3>
                <p>
                    In order to apply for the Regents’ Scholarship you must submit a complete scholarship
                    application file which includes: the <b>online application</b> and <b>supporting documentation</b>
                    used for verification of meeting scholarship requirements. A list of the required
                    supporting documents will be provided on the next page of instructions. Once you
                    have submitted the online portion of the application, you will receive a confirmation
                    email which will contain a detailed summary of what you still need to do to complete
                    your scholarship application. <b>As the applicant, it is your responsibility to submit
                        all required documentation by the deadline. <i>Your consideration for the scholarship
                            will be forfeited if you do not submit <u>all</u> required documentation by the
                            application deadline.</i></b>
                </p>
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
                <h3 style="text-align: center; color: #4967a4;">
                    INFORMATION NEEDED TO COMPLETE THE ONLINE PORTION OF THE APPLCIATION</h3>
                <p>
                    You will need the following information available from your high school and college
                    transcript(s) (if applicable) to complete the online portion of your application:</p>
                <div style="text-align: left;">
                    <ol>
                        <li style="margin-bottom: 1em;">Basic personal information.</li>
                        <li style="margin-bottom: 1em;">Titles of all the English, Math, Social Science, 
                            Science and World/Classical Language courses that you have completed in grades 
                            9-11, and that you will complete during grade 12.</li>
                        <li style="margin-bottom: 1em;">Grades earned in each course.</li>
                        <li style="margin-bottom: 1em;">Number of credits earned for each course.</li>
                        <li style="margin-bottom: 1em;">Grade level in which you completed the course - i.e.:
                            grade 9, 10, 11, or 12.</li>
                        <li style="margin-bottom: 1em;">High School Cumulative Grade Point Average (GPA).</li>
                        <li style="margin-bottom: 1em;">ACT score(s).</li>
                        <li style="margin-bottom: 1em;">State Student ID Number; <a href="#" onclick='return showItem("bodyplan");'
                            title="State Student ID">click here</a> for more information on where to obtain
                            your SSID. Note: Submitting the wrong SSID number could impact or delay the review
                            of your application.</li>
                        <li style="margin-bottom: 1em;">If you have or will earn college credit for a course,
                            this includes concurrent enrollment, you will need to identify the college from
                            which you have or will earn the credit.</li>
                    </ol>
                    <p>
                        <b>For help reading a high school sample transcript, <a target="_default" href="Transcript.aspx">
                            click here.</a>
                            <br />
                            <div style="display: inline;">
                                For help reading a college sample transcript,
                            </div>
                            <div onclick="javascript: OpenPDF();" style="text-decoration: underline; display: inline;
                                color: Blue;" onmouseover="this.style.cursor='pointer'">
                                click here.</div>
                        </b>
                    </p>
                </div>
                <br />
                <br />
                <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" />
                <br />
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
