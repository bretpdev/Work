Imports System.Threading
Imports System.IO
Imports System.Diagnostics.FileVersionInfo
Public Class frmMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMain))
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(104, 5)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "Form1"

    End Sub

#End Region

    Private fileNum As Integer
    Private cnt As Integer

    'This application will copy all files from the 
    'Live folder of an application, and replace the
    'current version. Then it will restart the calling application. 

    'The calling application should have a the NetStartClient module included in this source code.
    'It should then call the function "CheckForNewVersion("C:\Live Folder\")"

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim source As String
        Dim destination As String
        Dim name As String
        Dim lfv As FileVersionInfo
        Dim t As New Thread(AddressOf updateBar)

        'Thread.Sleep(2000)
        t.Start()
        FileOpen(1, "T:\NETstart.txt", OpenMode.Input)
        Input(1, source)
        Input(1, destination)
        Input(1, name)
        FileClose(1)

        'delete all files
        Dim farr As IO.FileInfo() = New IO.DirectoryInfo(source).GetFiles()
        Dim fi As IO.FileInfo
        fileNum = farr.GetLength(0)

        'replace files with live versions
        For Each fi In farr
            If Dir(destination & fi.Name) <> "" Then File.Delete(destination & fi.Name)
            File.Copy(source & fi.Name, destination & fi.Name)
            'Thread.Sleep(100)
            cnt += 1
        Next

        'start original process
        Process.Start(destination & name & ".exe")
        lfv = FileVersionInfo.GetVersionInfo(destination & name & ".exe")
        MsgBox("Upgraded " & name & " to version: " & lfv.FileVersion)
        End
    End Sub

    Sub updateBar()
        Dim pr As prog
        Dim g As Graphics
        Dim sb As New SolidBrush(Me.BackColor)
        pr = New prog
        pr.StartPosition = FormStartPosition.CenterScreen
        pr.Show()
        Do While cnt <> fileNum
            pr.Refresh()
            g = pr.lblbar.CreateGraphics()
            g.FillRectangle(sb, 0, 0, pr.lblbar.Width, pr.lblbar.Height)
            g.FillRectangle(Brushes.Blue, 0, 0, CInt(pr.lblbar.Width * (cnt / fileNum)), pr.lblbar.Height)
            Thread.Sleep(50)
        Loop
        pr.Hide()
    End Sub

End Class
