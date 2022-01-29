<%@ Page Language="C#" MasterPageFile="~/Regents.Master" AutoEventWireup="true" CodeBehind="frmPersonalInfo.aspx.cs"
    Inherits="RegentsApp.PersonalInfo" Title="Regents Scholarship Application :: Personal Information"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="regentsPlaceHolder" runat="server">
    <asp:UpdatePanel ID="up_PersonalInfo" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timer" runat="server" OnTick="timer_Tick">
            </asp:Timer>

            <script runat="server">
                protected void timer_Tick(object sender, EventArgs e)
                {
                    SaveAndReturn();
                    Response.Redirect("frmTimeout.aspx");
                }
            </script>

            <br />

            <script language="javascript" type="text/javascript">
                window.onload=function() {
                    if(!NiftyCheck())
                        return;
                    Rounded("div#MainContainer","Transparent","White","large");
                    if (document.getElementById('<%= ddlHowHear.ClientID %>').value == "6") {
                        document.getElementById('<%= divHowHear.ClientID %>').style.display = "inline";
                    }
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
                
                function viewEthnic(sent) {
                    if (sent.toString() == "Other")
                        document.getElementById('<%= divEthnic.ClientID %>').style.display = "inline";
                    else
                    {
                        document.getElementById('<%= divEthnic.ClientID %>').style.display = "none";
                        ValidatorEnable(document.getElementById('<%= vaEthnic.ClientID %>'), false);
                    }
                }
                function viewHowHear(sent) {
                    var myVal = document.getElementById('<%= vaHowHearOther.ClientID %>');
                    if (sent.toString() == "Other") {
                        document.getElementById('<%= divHowHear.ClientID %>').style.display = "inline";
                    }
                    else {
                        document.getElementById('<%= divHowHear.ClientID %>').style.display = "none";
                        ValidatorEnable(document.getElementById('<%= vaHowHearOther.ClientID %>'), false);
                    }
                }
                function showItem(item) {
                    document.getElementById(item).style.display = "block";
                    return false;
                }
                function hideItem(item) {
                    document.getElementById(item).style.display = "none";
                    return false;
                }
                function CheckEligible(sent) {
                    if (sent.toString() == "No") {
                        document.getElementById('<%= divEligibleDDL.ClientID %>').style.display = "block";
                        document.getElementById('<%= divEligibleLabel.ClientID %>').style.display = "block";
                    }
                    else {
                        document.getElementById('<%= divEligibleDDL.ClientID %>').style.display = "none";
                        document.getElementById('<%= ddlEligible.ClientID %>').value = "";
                        document.getElementById('<%= divNonCitNotElig.ClientID %>').style.display = "none";
                        document.getElementById('<%= divEligibleLabel.ClientID %>').style.display = "none";
                        ValidatorEnable(document.getElementById('<%= vaEligible.ClientID %>'), false);
                    }
                }
            </script>

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
                .style1
                {
                    width: 291px;
                }
            </style>
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
            <br />
            <asp:Label ID="lblWelcome" runat="server" Font-Names="Verdana"></asp:Label>
            <br />
            <h1 style="font-family: Verdana;">
                Personal Information
            </h1>
            <asp:SqlDataSource ID="dsStateLookup" runat="server" ConnectionString="<%$ ConnectionStrings:RegentsAppConnectionString %>"
                SelectCommand="SELECT [Abbreviation], [Code] FROM [StateLookup]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="dsGenderLookup" runat="server" ConnectionString="<%$ ConnectionStrings:RegentsAppConnectionString %>"
                SelectCommand="SELECT * FROM [GenderLookup]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="dsEthnicLookup" runat="server" ConnectionString="<%$ ConnectionStrings:RegentsAppConnectionString %>"
                SelectCommand="SELECT [Description], [Code] FROM [EthnicityLookup] WHERE ([IsInDefaultList] = @IsInDefaultList) ORDER BY [Description]">
                <SelectParameters>
                    <asp:Parameter DefaultValue="True" Name="IsInDefaultList" Type="Boolean" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="dsHowHear" runat="server" ConnectionString="<%$ ConnectionStrings:RegentsAppConnectionString %>"
                SelectCommand="SELECT [Code], [Description] FROM [HowHearAboutLookup] WHERE ([IsInDefault] = @IsInDefault) ORDER BY [Description]">
                <SelectParameters>
                    <asp:Parameter DefaultValue="true" Name="IsInDefault" Type="Boolean" />
                </SelectParameters>
            </asp:SqlDataSource>
            <div style="font-family: Verdana; width: 600px; margin-left: auto; margin-right: auto;">
                <p style="color: Red;">
                    Please help us protect your personal information by logging out of the 
                    application by using the &quot;Save and Return Later&quot; button at the bottom of the 
                    page.</p>
                <p>
                    Enter the following information. It is the student's responsibility to ensure data
                    is accurate before proceeding to the next section.
                </p>
                <div style="border-width: thin; border-color: Black; border-style: solid;">
                    <h3>
                        Full Legal Name
                    </h3>
                </div>
                <br />
                <table>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblFirstName" runat="server" Text="First Name:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbFirstName" runat="server" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaFirstName" runat="server" ControlToValidate="tbFirstName"
                                ErrorMessage="First Name required" Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblMidName" runat="server" Text="Middle Name:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbMidName" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblLastName" runat="server" Text="Last Name:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="tbLastName" runat="server" MaxLength="25"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaLastName" runat="server" ControlToValidate="tbLastName"
                                ErrorMessage="Last Name required" Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="border-width: thin; border-color: Black; border-style: solid;">
                    <h3>
                        Current Address
                    </h3>
                </div>
                <br />
                <table>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblAddy1" runat="server" Text="Street 1:"></asp:Label>
                        </td>
                        <td style="text-align: left;" class="style1">
                            <asp:TextBox ID="tbAddy1" runat="server" Width="250px" MaxLength="35"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaAddress" runat="server" ControlToValidate="tbAddy1"
                                ErrorMessage="Address required" Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblAddy2" runat="server" Text="Street 2:"></asp:Label>
                        </td>
                        <td style="text-align: left;" class="style1">
                            <asp:TextBox ID="tbAddy2" runat="server" Width="250px" MaxLength="35"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label>
                        </td>
                        <td style="text-align: left;" class="style1">
                            <asp:TextBox ID="tbCity" runat="server" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaCity" runat="server" ControlToValidate="tbCity"
                                ErrorMessage="City required" Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblState" runat="server" Text="State:"></asp:Label>
                        </td>
                        <td style="text-align: left;" class="style1">
                            <asp:DropDownList ID="ddlState" runat="server" Width="80px" DataSourceID="dsStateLookup"
                                DataTextField="Abbreviation" DataValueField="Code">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblZip" runat="server" Text="ZIP:"></asp:Label>
                        </td>
                        <td style="text-align: left;" class="style1">
                            <asp:TextBox ID="tbZip1" runat="server" Width="75px" MaxLength="5"></asp:TextBox>
                            -
                            <asp:TextBox ID="tbZip2" runat="server" Width="60px" MaxLength="4"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaZip" runat="server" ControlToValidate="tbZip1"
                                ErrorMessage="Zip required" Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblPhone" runat="server" Text="Telephone:"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 325px;" class="style1">
                            <br />
                            <asp:TextBox ID="tbPhone" runat="server"></asp:TextBox>
                            <asp:Label ID="lblPhoneIndicator" runat="server" Text="###-###-####"></asp:Label>
                            <asp:RequiredFieldValidator ID="vaPhone" runat="server" ControlToValidate="tbPhone"
                                ErrorMessage="Phone Number required" Enabled="False">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="vaRegPhone" runat="server" ControlToValidate="tbPhone"
                                ErrorMessage="Phone Number not valid" ValidationExpression="(\d{3}-)\d{3}-\d{4}">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="border-width: thin; border-color: Black; border-style: solid;">
                    <h3>
                        Applicant Information
                    </h3>
                </div>
                <br />
                <table>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <asp:Label ID="lblDOB" runat="server" Text="Date of Birth:"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <asp:TextBox ID="tbDOB" runat="server" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaDob" runat="server" ControlToValidate="tbDOB" ErrorMessage="Date of Birth required"
                                Enabled="False">*</asp:RequiredFieldValidator>
                            <asp:Label ID="lblDOBFormat" runat="server" Text="MM/DD/YYYY" Enabled="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:RegularExpressionValidator ID="vaRegDob" runat="server" ControlToValidate="tbDOB"
                                ErrorMessage="You entered a birth date that is invalid. Please correct this and try again."
                                ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"
                                Font-Size="Smaller"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <br />
                            <asp:Label ID="lblGender" runat="server" Text="Gender:"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <br />
                            <asp:DropDownList ID="ddlGender" runat="server" DataSourceID="dsGenderLookup" DataTextField="Description"
                                DataValueField="Code">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaGender" runat="server" ControlToValidate="ddlGender"
                                ErrorMessage="Gender required" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <br />
                            <asp:Label ID="lblEthnic" runat="server" Text="Ethnic Origin (optional):"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <br />
                            <asp:DropDownList ID="ddlEthnic" runat="server" DataSourceID="dsEthnicLookup" DataTextField="Description"
                                CausesValidation="false" DataValueField="Code" onchange="javascript: viewEthnic(this.options[this.selectedIndex].text);">
                            </asp:DropDownList>
                            <div id="divEthnic" runat="server" style="display: none;">
                                <asp:TextBox ID="tbEthnic" runat="server" Width="245px" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vaEthnic" runat="server" ErrorMessage="Designate your ethnicity in the text box provided when selecting 'other'."
                                    Enabled="False" Text="*" ControlToValidate="tbEthnic"></asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblChooseBest" runat="server" Font-Size="Smaller" Text="Choose the best one that describes you."
                                ForeColor="#3399FF"></asp:Label><br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <asp:Label ID="lblSSID" runat="server" Text="State Student ID Number:"></asp:Label>
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <asp:TextBox ID="tbSSID" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="vaSsid" runat="server" ControlToValidate="tbSSID"
                                ErrorMessage="State Student ID required" Enabled="False">*</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="vaSSID6" runat="server" ControlToValidate="tbSSID"
                                Enabled="False" ErrorMessage="You have entered an invalid State Student ID.  Please &lt;a href=&quot;#&quot; onclick='return showItem(&quot;bodyplan&quot;);'&gt;Click here&lt;/a&gt; for more information.">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="vaSSIDAlpha" runat="server" ControlToValidate="tbSSID"
                                ErrorMessage="You have entered an invalid State Student ID.  Please &lt;a href=&quot;#&quot; onclick='return showItem(&quot;bodyplan&quot;);'&gt;Click here&lt;/a&gt; for more information."
                                ValidationExpression="^[a-zA-Z0-9]+">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblSSIDNum" runat="server" Font-Size="Smaller" Text="Can be obtained through your school's counseling office.  &lt;a href=&quot;#&quot; onclick='return showItem(&quot;bodyplan&quot;);'&gt;Click here&lt;/a&gt; for more information."
                                ForeColor="#3399FF"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <br />
                            <asp:Label ID="lblCriminal" runat="server">Do you have a criminal
                    record, excluding minor traffic citations?</asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <br />
                            <br />
                            <asp:DropDownList ID="ddlCriminal" runat="server" Width="60px" Height="22px">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaCriminal" runat="server" ControlToValidate="ddlCriminal"
                                ErrorMessage="An answer to the question regarding criminal record is required"
                                Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left; color: Red;">
                            <div id="divCrimNotElig" runat="server" style="display: none;">
                                * You are not eligible to receive the Regents' Scholarship.</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <br />
                            <asp:Label ID="lblCitizen" runat="server">Are you a United States citizen?</asp:Label>
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <br />
                            <asp:DropDownList ID="ddlCitizen" runat="server" Height="22px" Width="60px">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaCitizen" runat="server" ControlToValidate="ddlCitizen"
                                ErrorMessage="An answer to the question regarding citizenship is required" Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <div id="divEligibleLabel" runat="server" style="display: none;">
                                <br />
                                <asp:Label ID="lblNonCitizen" runat="server">Are you a non-citizen 
                    who is able to receive federal financial aid?</asp:Label>
                            </div>
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <div id="divEligibleDDL" runat="server" style="display: none;">
                                <br />
                                <br />
                                <asp:DropDownList ID="ddlEligible" runat="server" Height="22px">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>Yes</asp:ListItem>
                                    <asp:ListItem>No</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="vaEligible" runat="server" ErrorMessage="An answer to the question regarding Financial Aid is required"
                                    Enabled="false" ControlToValidate="ddlEligible">*</asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left; color: Red;">
                            <div id="divNonCitNotElig" runat="server" style="display: none;">
                                * You are not eligible to receive the Regents' Scholarship.
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <br />
                            <asp:Label ID="lblFinAid" runat="server">Do you intend to apply 
                    for federal financial aid?</asp:Label>
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <br />
                            <br />
                            <asp:DropDownList ID="ddlFinAid" runat="server" Height="22px">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaFinAid" runat="server" ControlToValidate="ddlFinAid"
                                ErrorMessage="An answer to the question regarding financial aid is required"
                                Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <br />
                            <asp:Label ID="lblHowHear" runat="server">How did you hear about
                    the Regents' Scholarship?</asp:Label>
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <br />
                            <br />
                            <asp:DropDownList ID="ddlHowHear" runat="server" CausesValidation="false" DataSourceID="dsHowHear"
                                DataTextField="Description" onchange="javascript: viewHowHear(this.options[this.selectedIndex].text);"
                                DataValueField="Code">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaHowHear" runat="server" ControlToValidate="ddlHowHear"
                                ErrorMessage="An answer to the question regarding how you heard about the Regents' Scholarship is required"
                                Enabled="False">*</asp:RequiredFieldValidator>
                            <br />
                            <div id="divHowHear" runat="server" style="display: none;">
                                <asp:TextBox ID="tbHowHear" runat="server" Width="245px" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="vaHowHearOther" runat="server" ControlToValidate="tbHowHear"
                                    ErrorMessage="Please enter how you heard about the scholarship." Visible="True"
                                    Enabled="False">*</asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 300px;">
                            <br />
                            <asp:Label ID="lblUESP" runat="server" Text="Are you or our parents saving for your
                             college with a Utah Educational Savings Plan (UESP) account?"></asp:Label>
                            <br />
                            <br />
                        </td>
                        <td style="text-align: left; width: 300px;">
                            <asp:DropDownList ID="ddlUESP" runat="server" Width="60px" CausesValidation="false">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="vaUESP" runat="server" ControlToValidate="ddlUESP"
                                ErrorMessage="An answer to the question regarding an UESP account is required."
                                Enabled="False">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:ValidationSummary ID="vsName" runat="server" />
                <div id="divNextPage" runat="server">
                    <table id="tableBtns" style="width: auto; margin-left: auto; margin-right: auto;">
                        <tr>
                            <td>
                                <asp:Button ID="btnInfoNextPage" runat="server" Text="Next Page" OnClick="btnInfoNextPage_Click"
                                    Style="height: 26px" Width="80px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <table style="width: auto; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td>
                            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Width="80px"
                                CausesValidation="False" />
                        </td>
                        <td style="width: 85px;">
                        </td>
                        <td>
                            <asp:Button ID="btnInfoSaveReturn" runat="server" Text="Save and Return Later" OnClick="btnInfoSaveReturn_Click"
                                CausesValidation="false" />
                        </td>
                        <td style="width: 75px;">
                        </td>
                        <td>
                            <asp:Button ID="btnTop" runat="server" Text="Back to Top" onmousedown="javascript:scroll(0,0);" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
