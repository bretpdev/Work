Imports System.Drawing

Public Class UserInfo

    Public Userid As String
    Public FColor As Color
    Public BColor As Color
    Public Opacity As Double
    Public Conn As System.Data.SqlClient.SqlConnection
    Public BusinessUnit As String
    Public BUHasHomePage As Boolean
    Public MDSeqID As String

    Public Enum WhatSettingToUpdate
        BackColor = 0
        ForeColor = 1
        Opacity = 2
    End Enum

    Public Sub New()
        BUHasHomePage = False
    End Sub

    Public Sub FigureOutDB()
        'figure out DB connection
        Conn = New SqlClient.SqlConnection
        If SP.TestMode() Then
            Conn.ConnectionString = "Data Source=OPSDEV;Initial Catalog=MauiDUDE;Integrated Security=SSPI;"
        Else
            Conn.ConnectionString = "Data Source=NOCHOUSE;Initial Catalog=MauiDUDE;Integrated Security=SSPI;"
        End If
    End Sub

    Public Sub GetUserIDFromLP40I()
        SP.Q.FastPath("LP40")
        SP.Q.Hit("ENTER")
        Userid = SP.Q.GetText(1, 9, 7)
    End Sub

    'changes the color and opacity of the form passed in
    Public Sub ChangeFormSettingsColorsOnly(ByVal Frm As System.Windows.Forms.Form)
        Frm.BackColor = BColor
        Frm.ForeColor = FColor
        If MDSeqID <> "" Then Frm.Text = Frm.Text + " - #" + MDSeqID
    End Sub

    Public Sub UpdateSettingsFile(ByVal SettingToUpdate As WhatSettingToUpdate, ByVal SettingValue As Object)
        'give vars current values
        Dim LOpacity As Double = Opacity
        Dim LBColor As Int32 = BColor.ToArgb
        Dim LFColor As Int32 = FColor.ToArgb
        'change applicable value
        If SettingToUpdate = WhatSettingToUpdate.BackColor Then
            LBColor = CType(SettingValue, Color).ToArgb()
            BColor = Color.FromArgb(LBColor)
        ElseIf SettingToUpdate = WhatSettingToUpdate.ForeColor Then
            LFColor = CType(SettingValue, Color).ToArgb()
            FColor = Color.FromArgb(LFColor)
        ElseIf SettingToUpdate = WhatSettingToUpdate.Opacity Then
            LOpacity = CType(SettingValue, Double)
            Opacity = LOpacity
        End If
        'write updated info out to the pref file
        FileOpen(1, "T:\MauiDUDEPref.dat", OpenMode.Output)
        Write(1, LOpacity, LBColor, LFColor)
        FileClose(1)
    End Sub

    Public Sub ReturnToFavoriteScreen()
        Try
            Dim Line As String
            Dim UserI() As String
            If Dir$("C:\Windows\Temp\userinfo.txt") <> "" Or Dir$("T:\userinfo.txt") <> "" Then
                If Dir$("C:\Windows\Temp\userinfo.txt") <> "" Then
                    FileOpen(1, "C:\Windows\Temp\UserInfo.txt", OpenMode.Input, OpenAccess.Read)
                ElseIf Dir$("T:\userinfo.txt") <> "" Then
                    FileOpen(1, "T:\UserInfo.txt", OpenMode.Input, OpenAccess.Read)
                End If
                Line = LineInput(1)
                UserI = Line.Split(CChar(", "))
                If UserI(5) <> "" Then
                    SP.Q.FastPath(Replace(UserI(5), """", "")) 'remove quotes
                End If
                FileClose(1)
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
