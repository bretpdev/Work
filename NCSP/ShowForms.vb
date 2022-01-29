Module ShowForms
    Public frmNCSPForm As New frmNCSP
    Public frmStudentsForm As frmStudents

    Sub NCSP()
        frmNCSPForm.Show()
    End Sub

    Sub Students(ByVal dataSet As DataSet)
        frmStudentsForm = New frmStudents

        frmStudentsForm.MdiParent = frmNCSPForm
        frmStudentsForm.Show(dataSet)
    End Sub

    Sub Schedules()
        Dim frmSchedulesForm As New frmSchedules

        frmSchedulesForm.MdiParent = frmNCSPForm
        frmSchedulesForm.Show()
    End Sub

    Sub TransHistory()
        Dim frmTransactionHistoryForm As New frmTransactionHistory

        frmTransactionHistoryForm.MdiParent = frmNCSPForm
        frmTransactionHistoryForm.Show()
    End Sub

    Sub Communications(ByVal FilterStr As String)
        Dim frmCommunicationsForm As New frmCommunications

        frmCommunicationsForm.MdiParent = frmNCSPForm
        frmCommunicationsForm.Show(FilterStr)
    End Sub

    Sub ChangePassword(ByVal ExitonCancel As Boolean)
        Dim frmPasswordForm As New frmPassword

        frmPasswordForm.Show(ExitonCancel)
    End Sub

    Sub CloseForms()
        Dim i As Integer = 0
        While i <= UBound(frmNCSPForm.MdiChildren)
            If frmNCSPForm.MdiChildren(i).Name = "frmMain" Or ShowForms.frmNCSPForm.MdiChildren(i).Name = "frmStudents" Then
                i = i + 1
            Else
                frmNCSPForm.MdiChildren(i).Close()
            End If

        End While
    End Sub


End Module
