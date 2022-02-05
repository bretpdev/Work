Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class InternetExplorerAutomator


    'ATTENTION: DEBUGGING THIS CLASS SUCKS BECAUSE OF THE MULTI-THREADING!  SORRY!


    ''' <summary>
    ''' Different options for text entry
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum EnterTextTypes
        Text
        Password
    End Enum

    ''' <summary>
    ''' Different options for button clicks
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ClickButtonTypes
        Button
        Reset
        Submit
    End Enum


    Protected _browserWindow As GenericBrowser
    Protected _doc As HtmlDocument
    Protected _docCurrent As Boolean
    Protected _ie As WebBrowser
    Protected _uiThread As Thread


    'delegates for cross thread calls
    Private Delegate Sub BrowserVisible(ByVal makeVisible As Boolean)
    Private Delegate Sub BrowserNavigate(ByVal URL As String)
    Private Delegate Sub BrowserCloseWindow()

#Region "Implemented Delegates"

    Private _visible As New BrowserVisible(AddressOf XThreadingVisible)
    Private Sub XThreadingVisible(ByVal makeVisible As Boolean)
        _browserWindow.Visible = makeVisible
    End Sub
    ''' <summary>
    ''' Changes visiblity for generic browser window.
    ''' </summary>
    ''' <param name="makeVisible">Indicator as to whether the Window should be visible or not.</param>
    ''' <remarks></remarks>
    Public Sub Visible(ByVal makeVisible As Boolean)
        _browserWindow.Invoke(_visible, New Object() {makeVisible})
    End Sub

    Private _navigate As New BrowserNavigate(AddressOf XThreadingNavigate)
    Private Sub XThreadingNavigate(ByVal URL As String)
        _ie.Navigate(URL)
    End Sub
    ''' <summary>
    ''' Sends the browser to the specified URL.
    ''' </summary>
    ''' <param name="URL">URL for the browser to navigate to.</param>
    ''' <remarks></remarks>
    Public Sub Navigate(ByVal URL As String)
        _browserWindow.Invoke(_navigate, New Object() {URL})
    End Sub

    Private _closeBrowserWindow As New BrowserCloseWindow(AddressOf XThreadingCloseBrowserWindow)
    Private Sub XThreadingCloseBrowserWindow()
        _ie.Dispose()
        _browserWindow.Close()
    End Sub
    ''' <summary>
    ''' Closes Borwser window and releases browser resources (I guess the resourcses to the WebBrowser tool is really high. (When MS states that then you know it is bad).
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CloseBrowserWindow()
        _browserWindow.Invoke(_closeBrowserWindow)
    End Sub

#End Region

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        _browserWindow = New GenericBrowser
        _ie = _browserWindow.Browser
        AddHandler _ie.Navigating, AddressOf Navigating
        AddHandler _ie.DocumentCompleted, AddressOf Set_doc
        _uiThread = New Thread(AddressOf BrowserUIThread)
        _uiThread.SetApartmentState(ApartmentState.STA)
        _uiThread.IsBackground = True
        _uiThread.Start()
        'hold on current thread so browser can catch up
        Thread.Sleep(New TimeSpan(0, 0, 3))
        _docCurrent = False
    End Sub

    'Starts browser thread.  Two threads are needed to allow the browser browse while the application monitors it.
    Private Sub BrowserUIThread()
        _browserWindow.ShowDialog()
    End Sub

    'tracks when navigation process starts.
    Private Sub Navigating(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs)
        _docCurrent = False
    End Sub

    'sets the document when it has been loaded, also sets flag so other functionality works
    Private Sub Set_doc(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs)
        _doc = _ie.Document
        _docCurrent = True
    End Sub

    'just places everything on pause until the document is loaded
    Private Sub WaitAndSpinUntilDocumentIsCurrent()
        Dim ts As New TimeSpan(0, 0, 1)
        Dim SecondsWaited As Integer = 0
        While _docCurrent = False
            Threading.Thread.Sleep(ts)
            SecondsWaited = SecondsWaited + 1
            If SecondsWaited = 120 Then
                Throw New InternetAutomationException("Couldn't navigate to the desired web page.  Please contact a member of Systems Support.")
            End If
        End While
    End Sub

    ''' <summary>
    ''' Checks if the title text is the same as the string provided.
    ''' </summary>
    ''' <param name="titleTextToCheck">Text to check against.</param>
    ''' <returns>Ture or false whether the title matches the specified string sent to the method.</returns>
    ''' <remarks></remarks>
    Public Function IsTitleCorrect(ByVal titleTextToCheck As String) As Boolean
        WaitAndSpinUntilDocumentIsCurrent() 'make sure everything is ready to be interacted with
        Return (titleTextToCheck.ToUpper() = _doc.Title.ToUpper())
    End Function

    ''' <summary>
    ''' This function selects an option from a list or drop down list box. 
    ''' </summary>
    ''' <param name="FieldName">Field name to select option from.</param>
    ''' <param name="Selection">Selection option value.</param>
    ''' <remarks></remarks>
    Public Sub SelectOption(ByVal FieldName As String, ByVal Selection As String)
        Dim i As Integer
        Dim OptionCounter As Integer
        While _doc.All.Count > i
            If UCase(_doc.All.Item(i).TagName) = "SELECT" Then
                If UCase(_doc.All.Item(i).Name) = UCase(FieldName) Then
                    While OptionCounter < _doc.All.Item(i).Children.Count
                        If _doc.All.Item(i).Children(OptionCounter).GetAttribute("Value") = Selection Then
                            _doc.All.Item(i).SetAttribute("SelectedIndex", OptionCounter.ToString())
                            Exit Sub
                        End If
                        OptionCounter = OptionCounter + 1
                    End While
                    Throw New InternetAutomationException(String.Format("The select option ""{0}"" in field name ""{1}"" can't be found.  Please contact a member of Systems Support.", Selection, FieldName))
                End If
            End If
            i = i + 1
        End While
        Throw New InternetAutomationException(String.Format("The select option ""{0}"" in field name ""{1}"" can't be found.  Please contact a member of Systems Support.", Selection, FieldName))
    End Sub

    ''' <summary>
    ''' This method enters text into a text or password field. 
    ''' </summary>
    ''' <param name="Type">Type of field (text or password).</param>
    ''' <param name="FieldName">Field name.</param>
    ''' <param name="TextToBeEntered">Text to enter into the field.</param>
    ''' <remarks></remarks>
    Public Sub EnterText(ByVal Type As EnterTextTypes, ByVal FieldName As String, ByVal TextToBeEntered As String)
        Dim i As Integer
        Dim typeString As String
        If Type = EnterTextTypes.Text Then
            typeString = "TEXT"
        Else
            typeString = "PASSWORD"
        End If
        While _doc.All.Count > i
            If UCase(_doc.All.Item(i).TagName) = "INPUT" Then
                If typeString = "PASSWORD" Then 'if searching for a password text area
                    If UCase(_doc.All.Item(i).GetAttribute("Type")) = typeString Then
                        If UCase(_doc.All.Item(i).Name) = UCase(FieldName) Then
                            _doc.All.Item(i).SetAttribute("Value", TextToBeEntered)
                            Exit Sub
                        End If
                    End If
                ElseIf typeString = "TEXT" Then 'if searching for a regular text area
                    If UCase(_doc.All.Item(i).GetAttribute("Type")) = typeString Then
                        If UCase(_doc.All.Item(i).Name) = UCase(FieldName) Then
                            _doc.All.Item(i).SetAttribute("Value", TextToBeEntered)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            i = i + 1
        End While
        Throw New InternetAutomationException(String.Format("The text box defined in code as ""{0}"" can't be found.  Please contact a member of Systems Support.", FieldName))
    End Sub

    ''' <summary>
    ''' This method clicks a specified radio button.
    ''' </summary>
    ''' <param name="RadioText">The name of the radio button.</param>
    ''' <remarks></remarks>
    Public Sub ClickRadioButton(ByVal RadioText As String)
        Dim i As Integer
        While _doc.All.Count > i
            If UCase(_doc.All.Item(i).TagName) = "INPUT" Then
                If UCase(_doc.All.Item(i).GetAttribute("Type")) = "RADIO" Then
                    If UCase(_doc.All.Item(i).GetAttribute("Value")) = UCase(RadioText) Then
                        _doc.All.Item(i).InvokeMember("Click")
                        Exit Sub
                    End If
                End If
            End If
            i = i + 1
        End While
        Throw New InternetAutomationException(String.Format("The radio button defined in code as ""{0}"" can't be found.  Please contact a member of Systems Support.", RadioText))
    End Sub

    ''' <summary>
    ''' This method clicks on a check box.
    ''' </summary>
    ''' <param name="CheckBoxText">The check bos's name to click.</param>
    ''' <remarks></remarks>
    Public Sub ClickCheckBox(ByVal CheckBoxText As String)
        Dim i As Integer
        While _doc.All.Count > i
            If UCase(_doc.All.Item(i).TagName) = "INPUT" Then
                If UCase(_doc.All.Item(i).GetAttribute("Type")) = "CHECKBOX" Then
                    If UCase(_doc.All.Item(i).GetAttribute("Name")) = UCase(CheckBoxText) OrElse UCase(_doc.All.Item(i).GetAttribute("Value")) = UCase(CheckBoxText) Then
                        _doc.All.Item(i).InvokeMember("Click")
                        Exit Sub
                    End If
                End If
            End If
            i = i + 1
        End While
        Throw New InternetAutomationException(String.Format("The check box defined in code as ""{0}"" can't be found.  Please contact a member of Systems Support.", CheckBoxText))
    End Sub

    ''' <summary>
    ''' This method clicks a button.
    ''' </summary>
    ''' <param name="Type">Button type (Button, Reset or Submit)</param>
    ''' <param name="ButtonName">Button name.</param>
    ''' <remarks></remarks>
    Public Sub ClickButton(ByVal Type As ClickButtonTypes, ByVal ButtonName As String)
        Dim i As Integer
        Dim typeString As String
        If Type = ClickButtonTypes.Button Then
            typeString = "BUTTON"
        ElseIf Type = ClickButtonTypes.Reset Then
            typeString = "RESET"
        Else
            typeString = "SUBMIT"
        End If
        While _doc.All.Count > i
            If UCase(_doc.All.Item(i).TagName) = "INPUT" Then
                If typeString = "BUTTON" Then
                    If UCase(_doc.All.Item(i).GetAttribute("Type")) = typeString Then
                        If UCase(_doc.All.Item(i).GetAttribute("Value")) = UCase(ButtonName) OrElse UCase(_doc.All.Item(i).GetAttribute("Name")) = UCase(ButtonName) Then
                            _doc.All.Item(i).InvokeMember("Click")
                            'wait while the page loads
                            WaitAndSpinUntilDocumentIsCurrent()
                            Exit Sub
                        End If
                    End If
                ElseIf typeString = "RESET" Then
                    If UCase(_doc.All.Item(i).GetAttribute("Type")) = typeString Then
                        If UCase(_doc.All.Item(i).GetAttribute("Value")) = UCase(ButtonName) OrElse UCase(_doc.All.Item(i).GetAttribute("Name")) = UCase(ButtonName) Then
                            _doc.All.Item(i).InvokeMember("Click")
                            'wait while the page loads
                            WaitAndSpinUntilDocumentIsCurrent()
                            Exit Sub
                        End If
                    End If
                ElseIf typeString = "SUBMIT" Then
                    If UCase(_doc.All.Item(i).GetAttribute("Type")) = typeString Then
                        If UCase(_doc.All.Item(i).GetAttribute("Value")) = UCase(ButtonName) OrElse UCase(_doc.All.Item(i).GetAttribute("Name")) = UCase(ButtonName) Then
                            _doc.All.Item(i).InvokeMember("Click")
                            'wait while the page loads
                            WaitAndSpinUntilDocumentIsCurrent()
                            Exit Sub
                        End If
                    End If
                End If
            End If
            i = i + 1
        End While
        Throw New InternetAutomationException(String.Format("The button defined in code as ""{0}"" can't be found.  Please contact a member of Systems Support.", ButtonName))
    End Sub

    ''' <summary>
    ''' This method navigates the browser to the specified URL.
    ''' </summary>
    ''' <param name="URL">URL to navigate to.</param>
    ''' <param name="Title">The title to check against.</param>
    ''' <remarks></remarks>
    Public Sub NavigateToURL(ByVal URL As String, ByVal Title As String)
        'go to web site
        Navigate(URL)
        'wait until web page is up
        WaitAndSpinUntilDocumentIsCurrent()
        If IsTitleCorrect(Title) = False Then
            Throw New InternetAutomationException("The web page titles didn't match.  Please contact a member of Systems Support.")
        End If
    End Sub

    ''' <summary>
    ''' This method clicks on a link based off the outer HTML.  Good for clicking on image links.
    ''' </summary>
    ''' <param name="OuterHTML">Outer HTML text.</param>
    ''' <remarks></remarks>
    Public Sub ClickOnLinkThroughOuterHTML(ByVal OuterHTML As String)
        Dim i As Integer
        While _doc.Links.Count > i
            If InStr(UCase(_doc.Links.Item(i).OuterHtml), UCase(OuterHTML), vbTextCompare) Then
                _doc.Links.Item(i).InvokeMember("Click") 'click on applicable link
                'wait while the page loads
                WaitAndSpinUntilDocumentIsCurrent()
                Exit Sub
            End If
            i = i + 1
        End While
        Throw New InternetAutomationException(String.Format("The link defined in code as ""{0}"" can't be found.  Please contact a member of Systems Support.", OuterHTML))
    End Sub

    ''' <summary>
    ''' This method clicks on a link based off the text of the link. 
    ''' </summary>
    ''' <param name="LinkText"></param>
    ''' <remarks></remarks>
    Public Sub ClickOnLink(ByVal LinkText As String)
        Dim i As Integer
        While _doc.Links.Count > i
            If InStr(UCase(_doc.Links.Item(i).OuterText), UCase(LinkText), vbTextCompare) Then
                _doc.Links.Item(i).InvokeMember("Click") 'click on applicable link
                'wait while the page loads
                WaitAndSpinUntilDocumentIsCurrent()
                Exit Sub
            End If
            i = i + 1
        End While
        Throw New InternetAutomationException(String.Format("The link defined in code as ""{0}"" can't be found.  Please contact a member of Systems Support.", LinkText))
    End Sub


End Class

