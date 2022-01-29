Imports System.Windows.Forms
Imports System.Drawing
Public Class frmEmailComments
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal tIsGoodIdea As Boolean)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        IsGoodIdea = tIsGoodIdea
        If IsGoodIdea Then
            lblDirections.Text = "So, you think your smarter than The Big Kahuna hu!  Well, we'll see about that. Place your good idea in the bottle."
        Else
            lblDirections.Text = "Place your unexpected results in the bottle. A copy of your Reflection Session's Screen will be sent with your message."
        End If
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
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents lblDirections As System.Windows.Forms.Label
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents lblClose As System.Windows.Forms.Label
    Friend WithEvents lblMinimize As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmEmailComments))
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.btnSend = New System.Windows.Forms.Button
        Me.lblDirections = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.pnlMain = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblMinimize = New System.Windows.Forms.Label
        Me.lblClose = New System.Windows.Forms.Label
        Me.pnlMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(24, 328)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.Size = New System.Drawing.Size(168, 224)
        Me.txtComments.TabIndex = 0
        Me.txtComments.Text = ""
        '
        'btnSend
        '
        Me.btnSend.BackColor = System.Drawing.Color.Transparent
        Me.btnSend.Location = New System.Drawing.Point(24, 560)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.TabIndex = 1
        Me.btnSend.Text = "Send"
        '
        'lblDirections
        '
        Me.lblDirections.BackColor = System.Drawing.Color.Transparent
        Me.lblDirections.Location = New System.Drawing.Point(24, 272)
        Me.lblDirections.Name = "lblDirections"
        Me.lblDirections.Size = New System.Drawing.Size(176, 56)
        Me.lblDirections.TabIndex = 2
        Me.lblDirections.Text = "Directions"
        Me.lblDirections.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Transparent
        Me.btnCancel.Location = New System.Drawing.Point(120, 560)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        '
        'pnlMain
        '
        Me.pnlMain.BackColor = System.Drawing.Color.Transparent
        Me.pnlMain.BackgroundImage = CType(resources.GetObject("pnlMain.BackgroundImage"), System.Drawing.Image)
        Me.pnlMain.Controls.Add(Me.Label1)
        Me.pnlMain.Controls.Add(Me.lblMinimize)
        Me.pnlMain.Controls.Add(Me.lblClose)
        Me.pnlMain.Controls.Add(Me.txtComments)
        Me.pnlMain.Controls.Add(Me.lblDirections)
        Me.pnlMain.Controls.Add(Me.btnSend)
        Me.pnlMain.Controls.Add(Me.btnCancel)
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(216, 600)
        Me.pnlMain.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Vivaldi", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 224)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 48)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Message in a Bottle"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMinimize
        '
        Me.lblMinimize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMinimize.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinimize.Location = New System.Drawing.Point(96, 8)
        Me.lblMinimize.Name = "lblMinimize"
        Me.lblMinimize.Size = New System.Drawing.Size(24, 23)
        Me.lblMinimize.TabIndex = 5
        Me.lblMinimize.Text = "_"
        '
        'lblClose
        '
        Me.lblClose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblClose.Location = New System.Drawing.Point(120, 8)
        Me.lblClose.Name = "lblClose"
        Me.lblClose.Size = New System.Drawing.Size(24, 23)
        Me.lblClose.TabIndex = 4
        Me.lblClose.Text = "X"
        '
        'frmEmailComments
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.OldLace
        Me.ClientSize = New System.Drawing.Size(216, 600)
        Me.Controls.Add(Me.pnlMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmEmailComments"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maui DUDE Message in a Bottle"
        Me.TransparencyKey = System.Drawing.Color.OldLace
        Me.pnlMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private blnMoving As Boolean = False
    Private MouseDownX As Integer
    Private MouseDownY As Integer
    Private IsGoodIdea As Boolean
    Public Canceled As Boolean

    Private Sub pnlMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlMain.MouseDown
        If e.Button = MouseButtons.Left Then
            blnMoving = True
            MouseDownX = e.X
            MouseDownY = e.Y
        End If
    End Sub

    Private Sub pnlMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlMain.MouseMove
        If blnMoving Then
            Dim temp As Point = New Point
            temp.X = Me.Location.X + (e.X - MouseDownX)
            temp.Y = Me.Location.Y + (e.Y - MouseDownY)
            Me.Location = temp
        End If
    End Sub

    Private Sub pnlMain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlMain.MouseUp
        If e.Button = MouseButtons.Left Then
            blnMoving = False
        End If
    End Sub

    Private Sub lblClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblClose.Click
        Canceled = True
        Me.Hide()
    End Sub

    Private Sub lblMinimize_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Canceled = True
        Me.Hide()
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        Canceled = False
        Dim subjectStr As String
        If txtComments.Text.Replace(" ", "") = "" Then
            SP.frmWhoaDUDE.WhoaDUDE("Hey Dude, sending a bottle with no message is just littering. Please enter a comment and try again.", "Don't Litter", True)
            Exit Sub
        End If
        If IsGoodIdea Then
            subjectStr = "Maui DUDE - Good Idea"
            If TestMode() Then
                subjectStr = "TEST EMAIL PLEASE IGNORE -- " & subjectStr
            End If
            SendMail(Environment.UserName & "@utahsbr.edu", "MauiDUDE@utahsbr.edu", subjectStr, txtComments.Text)
        Else
            RIBM.PrintFileName = "T:\MD Reflection Unexpected.txt"

            RIBM.PrintFileExistsAction = Reflection.Constants.rcOverwrite
            RIBM.PrintToFile = True

            RIBM.PrintScreen(Reflection.Constants.rcPrintScreen, 1) 'create print screen in file
            RIBM.PrintToFile = False
            subjectStr = "Maui DUDE - Unexpected Result"
            If TestMode() Then
                subjectStr = "TEST EMAIL PLEASE IGNORE -- " & subjectStr
            End If
            SendMail(Environment.UserName & "@utahsbr.edu", "MauiDUDE@utahsbr.edu", subjectStr, txtComments.Text, , , "T:\MD Reflection Unexpected.txt")

        End If
        txtComments.Text = ""
        Me.Hide()
    End Sub

    Public Shared Function BrightIdea() As Boolean
        Dim frm As New frmEmailComments(True)
        frm.ShowDialog()
        Return frm.Canceled
    End Function

    Public Shared Function UnexpectedResults() As Boolean
        Dim frm As New frmEmailComments(False)
        SP.frmKnarlyDUDE.KnarlyDude("Make sure that your Reflection Session is on the correct screen and sp.q.hit OK to send a Screen Shot and Message to the Big Kahuna.", "MauiDUDE")
        frm.ShowDialog()
        Return frm.Canceled
    End Function

End Class
