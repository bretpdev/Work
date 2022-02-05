Imports SP
Imports System.Windows.Forms

Public Class frmCSQueueTask
    Dim cs As New frmContactScript()
    Dim script As String
    Dim newQueue As Boolean
    Dim first As Integer = 0
    Private bor As BorrowerLM
    Private ref As New Reference

    Public Sub New(ByVal _script As String, ByVal tbor As BorrowerLM, Optional ByVal _queue As Boolean = False)
        InitializeComponent()
        script = _script
        bor = tbor
        newQueue = _queue
        btnSaveRef1.Text = "Update Reference"
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmCSQueueTask_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadInfo()
    End Sub

    Private Sub LoadInfo()
        If newQueue Then
            gbxBor.Visible = False
            gbxRef1.Visible = False
            lblScript.Text = script
            Me.Height = 250
            btnCancel.Location = New System.Drawing.Point(208, 180)
            btnQueue.Location = New System.Drawing.Point(425, 180)
        Else
            tbxAmt.Visible = False
            mtbxDate.Visible = False
            lblAmt.Visible = False
            lblDate.Visible = False
            Dim i As Integer = 7
            Dim count As Integer = 1
            Dim counter As Integer = 1
            Dim temp As String
            FastPath("LP2CI" & bor.LMBorCnt.SSN)
            If SP.Q.Check4Text(1, 65, "REFERENCE SELECT") Then
                Do While Not SP.Q.Check4Text(22, 3, "46004")
                    If SP.Q.Check4Text(i, 42, "Y") Then
                        SP.Q.PutText(21, 13, count)
                        SP.Q.Hit("Enter")
                        If counter = 1 Then
                            bor.LMBorCnt.Ref1fname = Trim(SP.Q.GetText(4, 44, 12))
                            bor.LMBorCnt.Ref1lname = Trim(SP.Q.GetText(4, 5, 25))
                            bor.LMBorCnt.Ref1Addy1 = SP.Q.GetText(8, 9, 35)
                            bor.LMBorCnt.Ref1Addy2 = SP.Q.GetText(9, 9, 35)
                            bor.LMBorCnt.Ref1city = SP.Q.GetText(10, 9, 35)
                            bor.LMBorCnt.Ref1state = SP.Q.GetText(10, 52, 2)
                            bor.LMBorCnt.Ref1zip = SP.Q.GetText(10, 60, 9)
                            bor.LMBorCnt.Ref1phone = SP.Q.GetText(13, 17, 10)
                            bor.LMBorCnt.Ref1rel = SP.Q.GetText(6, 20, 8)
                            i = i + 3
                            SP.Q.Hit("F12")
                        Else
                            bor.LMBorCnt.Ref2fname = Trim(SP.Q.GetText(4, 44, 12))
                            bor.LMBorCnt.Ref2lname = Trim(SP.Q.GetText(4, 5, 25))
                            bor.LMBorCnt.Ref2addy1 = SP.Q.GetText(8, 9, 35)
                            bor.LMBorCnt.Ref2Addy2 = SP.Q.GetText(9, 9, 35)
                            bor.LMBorCnt.Ref2city = SP.Q.GetText(10, 9, 35)
                            bor.LMBorCnt.Ref2state = SP.Q.GetText(10, 52, 2)
                            bor.LMBorCnt.Ref2zip = SP.Q.GetText(10, 60, 9)
                            bor.LMBorCnt.Ref2phone = SP.Q.GetText(13, 17, 10)
                            bor.LMBorCnt.Ref2rel = SP.Q.GetText(6, 20, 8)
                            Exit Do
                        End If
                        counter = counter + 1
                    ElseIf i = 22 Then
                        SP.Q.Hit("F8")
                        count = 1
                        i = 7
                    Else
                        i = i + 3
                        count = count + 1
                    End If
                Loop
            End If
            lblScript.Text = script
            tbxBorAddy.Text = bor.UserProvidedDemos.Addr1
            temp = bor.UserProvidedDemos.HomePhoneNum
            If temp <> Nothing Then
                temp = temp.Insert(6, "-")
                temp = temp.Insert(3, "-")
            End If
            tbxBorPhone.Text = temp
            tbxDOB.Text = bor.UserProvidedDemos.DOB
            tbxDrvLic.Text = bor.LMBorCnt.DrvLic
            cbxRef1Rel.Text = bor.LMBorCnt.Ref1rel
            tbxRef1FName.Text = bor.LMBorCnt.Ref1fname
            tbxRef1LName.Text = bor.LMBorCnt.Ref1lname
            temp = bor.LMBorCnt.Ref1phone
            temp = temp.Insert(6, "-")
            temp = temp.Insert(3, "-")
            tbxRef1Phone.Text = temp
            tbxRef1Addy1.Text = bor.LMBorCnt.Ref1Addy1
            tbxRef1Addy2.Text = bor.LMBorCnt.Ref1Addy2
            tbxRef1City.Text = bor.LMBorCnt.Ref1city
            tbxRef1State.Text = bor.LMBorCnt.Ref1state
            tbxRef1Zip.Text = bor.LMBorCnt.Ref1zip
            cbxRef2Rel.Text = bor.LMBorCnt.Ref2rel
            tbxRef2FName.Text = bor.LMBorCnt.Ref2fname
            tbxRef2LName.Text = bor.LMBorCnt.Ref2lname
            temp = bor.LMBorCnt.Ref2phone
            temp = temp.Insert(6, "-")
            temp = temp.Insert(3, "-")
            tbxRef2Phone.Text = temp
            tbxRef2Addy1.Text = bor.LMBorCnt.Ref2addy1
            tbxRef2Addy2.Text = bor.LMBorCnt.Ref2Addy2
            tbxRef2City.Text = bor.LMBorCnt.Ref2city
            tbxRef2State.Text = bor.LMBorCnt.Ref2state
            tbxRef2Zip.Text = bor.LMBorCnt.Ref2zip
        End If
        first = first + 1
        bor.LMBorCnt.MadeChanges = False
    End Sub

    Private Sub btnQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueue.Click
        Dim com As String
        Dim q As String

        If tbxAmt.Text <> "" And mtbxDate.Text <> "" Then
            If DateAdd("d", 0, CDate(mtbxDate.Text)) >= Date.Today Then
                If CheckNum(tbxAmt.Text) Then
                    If newQueue Then
                        com = "Borrower promised to send $" & Trim(tbxAmt.Text) & " on " & mtbxDate.Text & "."
                        bor.LMBorCnt.PtpAmt = tbxAmt.Text
                        bor.LMBorCnt.PtpDate = mtbxDate.Text
                        q = "DPROMPAY"
                    Else
                        com = "Borrower requested to consolidate with Fed Direct, " & bor.UserProvidedDemos.Addr1 & " " & _
                        bor.UserProvidedDemos.City & ", " & bor.UserProvidedDemos.HomePhoneNum & ", " & tbxRef1FName.Text & " " & tbxRef1Addy1.Text & " " & _
                        tbxRef1Phone.Text & ", " & tbxRef2FName.Text & " " & tbxRef2Addy1.Text & " " & tbxRef2Phone.Text & ""
                        q = "DCONSTRT"
                    End If
                    Dim i As Integer = 1
                    Dim j As Integer = 8

                    FastPath("LP9OA" & bor.SSN)
                    Do While Not SP.Q.Check4Text(23, 3, "46004")
                        If SP.Q.Check4Text(j, 11, q) Then
                            SP.Q.PutText(21, 12, i)
                            SP.Q.Hit("Enter")
                            SP.Q.PutText(16, 12, com)
                            SP.Q.Hit("Enter")
                            SP.Q.Hit("F6")
                            Exit Do
                        End If
                        j = j + 1
                        i = i + 1
                        If i = 13 Then
                            SP.Q.Hit("F8")
                            j = 8
                            i = 1
                        End If
                    Loop

                    bor.LMBorCnt.Ref1rel = cbxRef1Rel.Text
                    bor.LMBorCnt.Ref1fname = tbxRef1FName.Text
                    bor.LMBorCnt.Ref1lname = tbxRef1LName.Text
                    bor.LMBorCnt.Ref1phone = tbxRef1Phone.Text
                    bor.LMBorCnt.Ref1Addy1 = tbxRef1Addy1.Text
                    bor.LMBorCnt.Ref1Addy2 = tbxRef1Addy2.Text
                    bor.LMBorCnt.Ref1city = tbxRef1City.Text
                    bor.LMBorCnt.Ref1state = tbxRef1State.Text
                    bor.LMBorCnt.Ref1zip = tbxRef1Zip.Text
                    bor.LMBorCnt.Ref2rel = cbxRef2Rel.Text
                    bor.LMBorCnt.Ref2fname = tbxRef2FName.Text
                    bor.LMBorCnt.Ref2lname = tbxRef2LName.Text
                    bor.LMBorCnt.Ref2phone = tbxRef2Phone.Text
                    bor.LMBorCnt.Ref2addy1 = tbxRef2Addy1.Text
                    bor.LMBorCnt.Ref2Addy2 = tbxRef2Addy2.Text
                    bor.LMBorCnt.Ref2city = tbxRef2City.Text
                    bor.LMBorCnt.Ref2state = tbxRef2State.Text
                    bor.LMBorCnt.Ref2zip = tbxRef2Zip.Text

                    Me.Close()
                Else
                    MsgBox("The Amount must be greater than 0 and in a numeric format", MsgBoxStyle.Exclamation, "Incorrect Format")
                End If
            Else
                MsgBox("The date must be today or in the future. Please fix and try again", MsgBoxStyle.Exclamation, "Incorrect Date")
            End If
        Else
            MsgBox("Both the Amount and Date must be filled in", MsgBoxStyle.Exclamation, "Fields Missing")
        End If
    End Sub

    Private Function CheckNum(ByVal num As String) As Boolean
        Dim i As Double
        Dim s As String = Trim(num)
        If Double.TryParse(s, i) Then
            If i > 0 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Private Sub cbxRef1Rel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxRef1Rel.SelectedIndexChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1rel = cbxRef1Rel.Text
        End If
    End Sub

    Private Sub tbxRef1FName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef1FName.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1fname = tbxRef1FName.Text
        End If
    End Sub

    Private Sub tbxRef1LName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef1LName.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1lname = tbxRef1LName.Text
        End If
    End Sub

    Private Sub tbxRef1Addy1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef1Addy1.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1Addy1 = tbxRef1Addy1.Text
        End If
    End Sub

    Private Sub tbxRed1Addy2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef1Addy2.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1Addy2 = tbxRef1Addy2.Text
        End If
    End Sub

    Private Sub tbxRef1City_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef1City.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1city = tbxRef1City.Text
        End If
    End Sub

    Private Sub tbxRef1State_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef1State.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1state = tbxRef1State.Text
        End If
    End Sub

    Private Sub tbxRef1Zip_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef1Zip.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1zip = tbxRef1Zip.Text
        End If
    End Sub

    Private Sub tbxRef1Phone_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef1Phone.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref1"
            bor.LMBorCnt.Ref1phone = tbxRef1Phone.Text
        End If
    End Sub

    Private Sub cbxRef2Rel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxRef2Rel.SelectedIndexChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2rel = cbxRef2Rel.Text
        End If
    End Sub

    Private Sub tbxRef2FName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef2FName.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2fname = tbxRef2FName.Text
        End If
    End Sub

    Private Sub tbxRef2LName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef2LName.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2lname = tbxRef2LName.Text
        End If
    End Sub

    Private Sub tbxRef2Addy1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef2Addy1.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2addy1 = tbxRef2Addy1.Text
        End If
    End Sub

    Private Sub tbxRef2Phone_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef2Phone.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2phone = tbxRef2Phone.Text
        End If
    End Sub

    Private Sub tbxRef2Addy2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef2Addy2.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2Addy2 = tbxRef2Addy2.Text
        End If
    End Sub

    Private Sub tbxRef2City_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef2City.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2city = tbxRef2City.Text
        End If
    End Sub

    Private Sub tbxRef2State_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef2State.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2state = tbxRef2State.Text
        End If
    End Sub

    Private Sub tbxRef2Zip_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbxRef2Zip.TextChanged
        If first > 0 Then
            bor.LMBorCnt.MadeChanges = True
            bor.LMBorCnt.RefChanged = "Ref2"
            bor.LMBorCnt.Ref2zip = tbxRef2Zip.Text
        End If
    End Sub

    Private Sub btnClear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear1.Click
        cbxRef1Rel.Text = ""
        tbxRef1Addy1.Text = ""
        tbxRef1Addy2.Text = ""
        tbxRef1City.Text = ""
        tbxRef1FName.Text = ""
        tbxRef1LName.Text = ""
        tbxRef1Phone.Text = ""
        tbxRef1State.Text = ""
        tbxRef1Zip.Text = ""
        cbxRef1Rel.Text = ""
        btnSaveRef1.Text = "Add Reference"
    End Sub

    Private Sub btnSaveRef1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveRef1.Click

        Dim i As Integer = 7
        Dim count As Integer = 1
        bor.LMBorCnt.ChangeRef(cbxRef1Rel.Text)
        If btnSaveRef1.Text = "Update Reference" Then
            SP.FastPath("LP2CI")
            If SP.Check4Text(1, 65, "REFERENCE SELECT") Then
                While SP.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                    If SP.Check4Text(i, 27, "A") And SP.Q.Check4Text(i - 1, 7, tbxRef1FName.Text) And SP.Q.Check4Text(i, 42, "Y") Then
                        UpdateRef1(count)
                        Exit While
                    End If
                    i = i + 3
                    count = count + 1
                    If SP.Check4Text(i - 1, 3, " ") Or i > 18 Then
                        i = 7
                        count = 1
                        SP.Hit("F8")
                    End If
                End While
            ElseIf SP.Check4Text(1, 59, "REFERENCE DEMOGRAPHICS") Then
                UpdateRef1(count)
            End If
        ElseIf btnSaveRef1.Text = "Add Reference" Then
            FastPath("LP2CA" & bor.SSN)
            SP.Q.PutText(4, 5, bor.LMBorCnt.Ref1lname)
            SP.Q.PutText(4, 44, bor.LMBorCnt.Ref1fname)
            SP.Q.PutText(5, 9, "O")
            bor.LMBorCnt.ChangeRef(bor.LMBorCnt.Ref1rel)
            SP.Q.PutText(6, 15, bor.LMBorCnt.Relationship)
            SP.Q.PutText(8, 9, bor.LMBorCnt.Ref1Addy1)
            SP.Q.PutText(8, 53, "Y")
            SP.Q.PutText(9, 9, bor.LMBorCnt.Ref1Addy2)
            SP.Q.PutText(10, 9, bor.LMBorCnt.Ref1city)
            SP.Q.PutText(10, 52, bor.LMBorCnt.Ref1state)
            SP.Q.PutText(10, 60, bor.LMBorCnt.Ref1zip)
            bor.LMBorCnt.Ref1phone = bor.LMBorCnt.Ref1phone.Replace("-", "")
            SP.Q.PutText(13, 17, bor.LMBorCnt.Ref1phone)
            SP.Q.PutText(13, 42, "Y")
            SP.Q.Hit("Enter")
            SP.Q.Hit("F6")
            btnSaveRef1.Text = "Update Reference"
            If SP.Q.Check4Text(22, 3, "48003") Then
                MsgBox("Reference Added Successfully", MsgBoxStyle.OkOnly, "Reference Added")
            End If
        End If
    End Sub

    Private Sub btnRef2Update_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRef2Update.Click
        Dim i As Integer = 7
        Dim count As Integer = 1
        SP.FastPath("LP2CI")
        If SP.Check4Text(1, 65, "REFERENCE SELECT") Then
            While SP.Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") = False
                If SP.Check4Text(i, 27, "A") And SP.Q.Check4Text(i - 1, 7, tbxRef2FName.Text) Then
                    UpdateRef2(count)
                    Exit While
                End If
                i = i + 3
                count = count + 1
                If SP.Check4Text(i - 1, 3, " ") Or i > 18 Then
                    i = 7
                    count = 1
                    SP.Hit("F8")
                End If
            End While
        ElseIf SP.Check4Text(1, 59, "REFERENCE DEMOGRAPHICS") Then
            UpdateRef2(count)
        End If
    End Sub

    Private Sub UpdateRef1(ByVal count As Integer)
        SP.PutText(21, 13, CStr(count))
        SP.Hit("Enter")
        SP.Q.PutText(1, 7, "C")
        SP.Q.Hit("Enter")
        bor.LMBorCnt.ChangeRef(bor.LMBorCnt.Ref1rel)
        SP.Q.PutText(6, 15, bor.LMBorCnt.Relationship)
        SP.Q.PutText(8, 9, bor.LMBorCnt.Ref1Addy1)
        SP.Q.PutText(8, 53, "Y")
        SP.Q.PutText(9, 9, bor.LMBorCnt.Ref1Addy2)
        SP.Q.PutText(10, 9, bor.LMBorCnt.Ref1city)
        SP.Q.PutText(10, 52, bor.LMBorCnt.Ref1state)
        SP.Q.PutText(10, 60, bor.LMBorCnt.Ref1zip)
        SP.Q.PutText(13, 17, bor.LMBorCnt.Ref1phone)
        SP.Q.PutText(13, 42, "Y")
        SP.Q.Hit("Enter")
        SP.Q.Hit("F6")
        If SP.Q.Check4Text(22, 3, "49000") Then
            MsgBox("Reference Updated Successfully", MsgBoxStyle.OkOnly, "Updated")
        Else
            MsgBox("There was a problem updated the reference. Please fix and try again", MsgBoxStyle.Critical, "Error Saving Reference")
        End If
    End Sub

    Private Sub UpdateRef2(ByVal count As Integer)
        SP.PutText(21, 13, CStr(count))
        SP.Hit("Enter")
        SP.Q.PutText(1, 7, "C")
        SP.Q.Hit("Enter")
        bor.LMBorCnt.ChangeRef(bor.LMBorCnt.Ref2rel)
        SP.Q.PutText(6, 15, bor.LMBorCnt.Relationship)
        SP.Q.PutText(8, 9, bor.LMBorCnt.Ref2addy1)
        SP.Q.PutText(8, 53, "Y")
        SP.Q.PutText(9, 9, bor.LMBorCnt.Ref2Addy2)
        SP.Q.PutText(10, 9, bor.LMBorCnt.Ref2city)
        SP.Q.PutText(10, 52, bor.LMBorCnt.Ref2state)
        SP.Q.PutText(10, 60, bor.LMBorCnt.Ref2zip)
        SP.Q.PutText(13, 17, bor.LMBorCnt.Ref2phone)
        SP.Q.PutText(13, 42, "Y")
        SP.Q.Hit("Enter")
        SP.Q.Hit("F6")
        If SP.Q.Check4Text(22, 3, "49000") Then
            MsgBox("Reference Updated Successfully", MsgBoxStyle.OkOnly, "Updated")
        Else
            MsgBox("There was a problem updated the reference. Please fix and try again", MsgBoxStyle.Critical, "Error Saving Reference")
        End If
    End Sub
End Class