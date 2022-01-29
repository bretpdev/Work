Public Class BSNoteDude
    Inherits SP.frmNoteDUDE

    Public Enum ReflectionSystem
        Compass
        OneLink
    End Enum

    Private _system As ReflectionSystem

    Public Sub New(ByRef borrower As SP.Borrower, ByVal rSystem As ReflectionSystem)
        MyBase.New(borrower)
        _system = rSystem

        Dim systemString As String = "COMPASS"
        If rSystem = ReflectionSystem.OneLink Then systemString = "ONELINK"
        MyBase.Label1.Text = systemString
        MyBase.Label2.Text = "Note DUDE"
        MyBase.Text = systemString + " Note DUDE"
        MyBase.StartPosition = Windows.Forms.FormStartPosition.Manual
    End Sub

    Public Overloads Function Show(ByVal forCompass As Boolean, ByVal firstTimeDisplaying As Boolean) As Boolean
        If forCompass Then
            tbNoteText.MaxLength = 1400
        Else
            tbNoteText.MaxLength = 600
        End If
        If _system = ReflectionSystem.OneLink Then
            CType(MyBase.Bor, BorrowerBS).NoteDudeNotesForOneLink = tbNoteText.Text
        Else
            CType(MyBase.Bor, BorrowerBS).Notes = tbNoteText.Text
        End If
        If firstTimeDisplaying Then
            MyBase.Show()
        Else
            MyBase.ShowDialog()
        End If
    End Function

    Public Overrides Sub Cancel()
        If _system = ReflectionSystem.OneLink Then
            tbNoteText.Text = CType(MyBase.Bor, BorrowerBS).NoteDudeNotesForOneLink
        Else
            tbNoteText.Text = CType(MyBase.Bor, BorrowerBS).Notes
        End If
        Me.Hide()
    End Sub

    Public Overrides Sub OK()
        If _system = ReflectionSystem.OneLink Then
            CType(MyBase.Bor, BorrowerBS).NoteDudeNotesForOneLink = tbNoteText.Text
        Else
            CType(MyBase.Bor, BorrowerBS).Notes = tbNoteText.Text
        End If
        MyBase.Bor.SpillGuts()
        Me.Hide()
    End Sub
End Class
