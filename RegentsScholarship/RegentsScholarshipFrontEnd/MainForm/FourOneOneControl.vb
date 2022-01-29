Imports RegentsScholarshipBackEnd

Public Class FourOneOneControl
    ''' <summary>
    ''' DO NOT USE!!!
    ''' The parameterless constructor is needed for Visual Studio's Form Designer, but does nothing for our program.
    ''' </summary>
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    Public Sub New(ByVal comm As Communication)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        'Bind the Communication object and set the ToolTip text to the subject.
        CommunicationBindingSource.DataSource = comm
        ttpSubject.SetToolTip(lblSubject, comm.Subject)
    End Sub
End Class
