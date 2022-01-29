<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transcript.aspx.cs" Inherits="RegentsApp.Transcript1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sample High School Transcript</title>
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
    </script>

</head>
<body style="background-color: #8b6800; text-align: center;">
    <div style="background-color: White; text-align: center; width: 800px; padding: 35px;">
        <div class="showHide" id="SSID">
            <p>
                A State Student Identification Number is assigned to students attending public schools,
                and should be found on your high school transcript or may be obtained from your
                high school guidance counselor. For students attending a private school use the
                first four letters of your last name and your six digit birthday. For example, if
                your last name is "Brown" and your birthday is on January 15, 1992, the number you
                would use is brow011592. If your last name is less than four letters, use your full
                last name and your six digit birthday.
            </p>
        </div>
        <div class="showHide" id="gpa">
            <p>
                This is an example of a cumulative GPA. This reflects the GPA for all course work
                that a student has completed.</p>
        </div>
        <div class="showHide" id="grade9">
            <p>
                This is an example of a grade level. This is the year in which the student completed
                the course.</p>
        </div>
        <div class="showHide" id="ACT">
            <p>
                This is an example of an ACT score, which is generally listed on the transcript.
                Students will be required to input the composite score and the scores earned for
                English, Math, Science, and Reading sections of the test.</p>
        </div>
        <div class="showHide" id="className">
            <p>
                This is an example of a course name. When entering the course name, choose whichever
                title is closest to what is reflected on the student's transcript.</p>
        </div>
        <div class="showHide" id="classGrade">
            <p>
                This is an example of a course grade. This is the letter grade that was earned for
                each term/trimester.</p>
        </div>
        <div class="showHide" id="classCredit">
            <p>
                This is an example of course credit. This reflects the number of credits earned
                for the course completed.</p>
        </div>
        <div style="font-family: Verdana; width: 700px; margin-left: auto; margin-right: auto;
            font-size: x-small;">
            <h1 style="text-align: center;">
                High School Sample Transcript</h1>
            <div style="text-align: left; border: solid 1px black; width: 680px;">
                <table>
                    <tr>
                        <td>
                            UTAH SCHOOL DISTRICT
                        </td>
                        <td style="width: 115px;">
                        </td>
                        <td>
                            UTAH HIGH SCHOOL
                        </td>
                        <td style="width: 90px;">
                        </td>
                        <td>
                            DATE:
                        </td>
                        <td style="width: 40px;">
                        </td>
                        <td>
                            02/01/2012
                        </td>
                    </tr>
                    <tr>
                        <td>
                            STUDENT TRANSCRIPT
                        </td>
                        <td>
                        </td>
                        <td>
                            123 MAIN STREET
                        </td>
                        <td>
                        </td>
                        <td>
                            PHONE:
                        </td>
                        <td>
                        </td>
                        <td>
                            801-555-1234
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            HIGHLAND, UT
                        </td>
                        <td>
                        </td>
                        <td>
                            FAX:
                        </td>
                        <td>
                        </td>
                        <td>
                            801-555-4321
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            JOHN Q. STUDENT
                        </td>
                        <td>
                        </td>
                        <td>
                            1234 ANYWHERE ST.
                        </td>
                        <td>
                        </td>
                        <td>
                            BIRTHDAY:
                        </td>
                        <td>
                        </td>
                        <td>
                            10/01/1992
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            HIGHLAND, UT
                        </td>
                        <td>
                        </td>
                        <td>
                            SSN:
                        </td>
                        <td>
                        </td>
                        <td>
                            123-45-6789
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            GUARDIAN: SUZIE PARENT
                        </td>
                        <td style="width: 125px;">
                        </td>
                        <td style="width: 90px;">
                        </td>
                        <td style="width: 100px;">
                        </td>
                        <td>
                            <a href="#" onclick="javascript: return false;" onmouseover='return showItem("SSID");'
                                onmouseout='return hideItem("SSID");'>
                                <table>
                                    <tr>
                                        <td style="width: 75px;">
                                            SSID:
                                        </td>
                                        <td style="width: 30px;">
                                        </td>
                                        <td style="padding-right: 30px;">
                                            9876543
                                        </td>
                                    </tr>
                                </table>
                            </a>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        GRADE
                                    </td>
                                    <td style="width: 70px;">
                                    </td>
                                    <td>
                                        CREDIT
                                    </td>
                                    <td style="width: 70px;">
                                    </td>
                                    <td>
                                        GPA
                                    </td>
                                    <td style="width: 60px;">
                                    </td>
                                    <td>
                                        OVERALL
                                    </td>
                                    <td style="width: 70px;">
                                    </td>
                                    <td>
                                        CREDIT
                                    </td>
                                    <td style="width: 50px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        09
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        7.00
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        4.00
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        10
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        7.75
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        4.00
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        09-12
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        30.75
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        11
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        7.5
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        4.00
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        12
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        8.5
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        4.00
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        CLASS RANK: 14/660
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <a href="#" onclick="javascript: return false;" onmouseover='return showItem("gpa");'
                                onmouseout='return hideItem("gpa");'>
                                <table>
                                    <tr>
                                        <td>
                                            GPA
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 15px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            4.00
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 30px;">
                                        </td>
                                    </tr>
                                </table>
                            </a>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table>
                    <tr>
                        <td style="width: 96px;">
                            <a href="#" onclick="javascript: return false;" onmouseover='return showItem("grade9");'
                                onmouseout='return hideItem("grade9");'>9TH GRADE</a>
                        </td>
                        <td style="width: 35px;">
                        </td>
                        <td>
                            Q1
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            Q2
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            Q3
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            Q4
                        </td>
                        <td style="width: 20px;">
                        </td>
                        <td>
                            CREDIT
                        </td>
                        <td style="width: 30px;">
                        </td>
                        <td style="width: 96px;">
                            12TH GRADE
                        </td>
                        <td style="width: 35px;">
                        </td>
                        <td>
                            Q1
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            Q2
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            Q3
                        </td>
                        <td style="width: 10px;">
                        </td>
                        <td>
                            Q4
                        </td>
                        <td style="width: 20px;">
                        </td>
                        <td>
                            CREDIT
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            BAND
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                        <td>
                        </td>
                        <td>
                            PHYSICS
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                    </tr>
                    <tr>
                        <td>
                            BIOLOGY
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                        <td>
                        </td>
                        <td>
                            MARCHING BND
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            .25
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ENGLISH 9 H
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                        <td>
                        </td>
                        <td>
                            CONCURRENT A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            .25
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ALGEBRA 1
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                        <td>
                        </td>
                        <td>
                            CONCURRENT B
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            .25
                        </td>
                    </tr>
                    <tr>
                        <td>
                            GEOGRAPHY
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            .50
                        </td>
                        <td>
                        </td>
                        <td>
                            SYMPHONY
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                    </tr>
                    <tr>
                        <td>
                            WORLD CIV
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            .50
                        </td>
                        <td>
                        </td>
                        <td>
                            ENGLISH 12
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                    </tr>
                    <tr>
                        <td>
                            PHYS ED
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            .50
                        </td>
                        <td>
                        </td>
                        <td>
                            JAPANESE 2
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                    </tr>
                    <tr>
                        <td>
                            COMPUTERS
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            .50
                        </td>
                        <td>
                        </td>
                        <td>
                            PRECALCULUS
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                    </tr>
                    <tr>
                        <td>
                            FOODS
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            .50
                        </td>
                        <td>
                        </td>
                        <td>
                            CHILD DEVELOP
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            .50
                        </td>
                    </tr>
                    <tr>
                        <td>
                            OFFICE AIDE
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            .50
                        </td>
                        <td>
                        </td>
                        <td>
                            PHOTOGRAPHY2
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            A
                        </td>
                        <td>
                        </td>
                        <td>
                            1.00
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table>
                    <tr>
                        <td style="text-align: left;">
                            <table>
                                <tr>
                                    <td style="width: 96px;">
                                        10TH GRADE
                                    </td>
                                    <td style="width: 25px;">
                                    </td>
                                    <td>
                                        Q1
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        Q2
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        Q3
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        Q4
                                    </td>
                                    <td style="width: 20px;">
                                    </td>
                                    <td>
                                        CREDIT
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        GOVERNMENT
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        .50
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        DRIVERS ED
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        .25
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        BAND
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        CHEMISTRY
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        ENGLISH 10 H
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        GEOMETRY
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        US HISTORY H
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        HEALTH
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        .50
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        FITNESS
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        .50
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        ARTS
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        .50
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        FOODS
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        .50
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 20px;">
                        </td>
                        <td>
                            <a href="#" onclick="javascript: return false;" onmouseover='return showItem("ACT");'
                                onmouseout='return hideItem("ACT");'>
                                <table>
                                    <tr>
                                        <td>
                                            ACT COMPOSITE
                                        </td>
                                        <td style="width: 15px;">
                                        </td>
                                        <td>
                                            29
                                        </td>
                                        <td style="width: 15px;">
                                        </td>
                                        <td>
                                            DATE:
                                        </td>
                                        <td style="width: 20px;">
                                        </td>
                                        <td style="text-align: right; padding-right: 20px;">
                                            05/09
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            ENGLISH ACT
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            30
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            READING ACT
                                        </td>
                                        <td>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px;">
                                            26
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            MATH ACT
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            33
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            SCIENCE ACT
                                        </td>
                                        <td>
                                        </td>
                                        <td style="text-align: right; padding-right: 20px;">
                                            26
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 90px;">
                                        </td>
                                    </tr>
                                </table>
                            </a>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table>
                    <tr>
                        <td style="text-align: left;">
                            <table>
                                <tr>
                                    <td>
                                        10TH GRADE
                                    </td>
                                    <td style="width: 25px;">
                                    </td>
                                    <td>
                                        Q1
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        Q2
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        Q3
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        Q4
                                    </td>
                                    <td style="width: 20px;">
                                    </td>
                                    <td>
                                        CREDIT
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        FIN LIT
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        .50
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SYMPHONY
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        AP CHEMISTRY
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        ENGLISH 11
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        JAPANESE 1
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#" onclick="javascript: return false;" onmouseover='return showItem("className");'
                                            onmouseout='return hideItem("className");'>ALGEBRA 2</a>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <a href="#" onclick="javascript: return false;" onmouseover='return showItem("classGrade");'
                                            onmouseout='return hideItem("classGrade");'>A</a>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <a href="#" onclick="javascript: return false;" onmouseover='return showItem("classGrade");'
                                            onmouseout='return hideItem("classGrade");'>A</a>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <a href="#" onclick="javascript: return false;" onmouseover='return showItem("classGrade");'
                                            onmouseout='return hideItem("classGrade");'>A</a>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <a href="#" onclick="javascript: return false;" onmouseover='return showItem("classGrade");'
                                            onmouseout='return hideItem("classGrade");'>A</a>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <a href="#" onclick="javascript: return false;" onmouseover='return showItem("classCredit");'
                                            onmouseout='return hideItem("classCredit");'>1.00</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        PSYCHOLOGY
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        PHOTOGRAPHY 1
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        A
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        1.00
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 70px;">
                        </td>
                        <td style="width: 270px;">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <table>
                    <tr>
                        <td style="text-align: left;">
                            _______________________________________________
                        </td>
                        <td style="width: 80px;">
                        </td>
                        <td style="text-align: left;">
                            _______________________________________________
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            AUTHORIZED SIGNATURE
                        </td>
                        <td>
                        </td>
                        <td style="text-align: center;">
                            TITLE
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</body>
</html>
