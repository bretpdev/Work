Imports System.IO
Imports Microsoft.Win32

Public Class frmVault
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
    Friend WithEvents txtvault As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmVault))
        Me.txtvault = New System.Windows.Forms.TextBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtvault
        '
        Me.txtvault.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtvault.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(64, Byte))
        Me.txtvault.Font = New System.Drawing.Font("Futura Lt BT", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtvault.ForeColor = System.Drawing.Color.Aqua
        Me.txtvault.Location = New System.Drawing.Point(24, 16)
        Me.txtvault.Multiline = True
        Me.txtvault.Name = "txtvault"
        Me.txtvault.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtvault.Size = New System.Drawing.Size(552, 224)
        Me.txtvault.TabIndex = 1
        Me.txtvault.Text = ""
        '
        'btnSave
        '
        Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(64, Byte), CType(64, Byte))
        Me.btnSave.Font = New System.Drawing.Font("FuturaBlack BT", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.Teal
        Me.btnSave.Location = New System.Drawing.Point(230, 248)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(144, 38)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Close Vault"
        '
        'frmVault
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(600, 300)
        Me.Controls.Add(Me.txtvault)
        Me.Controls.Add(Me.btnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmVault"
        Me.Text = "Vault"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private _encryptedFile As String
    Private _isMoving As Boolean = False
    Private _mouseDownX As Integer
    Private _mouseDownY As Integer

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        'Get the location of My Documents from the registry.
        Dim myDocuments As String = String.Empty
        Try
            myDocuments = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders").GetValue("Personal").ToString()
        Catch ex As Exception
            MessageBox.Show("Vault was unable to find the My Documents folder. Your encrypted data file is not accessible at this time.", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try

        'Define the encrypted file name and open it up.
        _encryptedFile = String.Format("{0}\Vault{1}.txt", myDocuments, Environment.UserName)
        If Not File.Exists(_encryptedFile) Then
            'Create the file.
            File.Create(_encryptedFile)
        Else
            Dim encryptedText As String = String.Empty
            Using vaultReader As New StreamReader(_encryptedFile)
                encryptedText = vaultReader.ReadLine().Replace("""", "")
                vaultReader.Close()
            End Using
            txtvault.Text = New IDEncryption().DecryptString(encryptedText, System.Environment.UserName)
        End If
        btnSave.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Using vaultWriter As New StreamWriter(_encryptedFile)
            vaultWriter.WriteLine(New IDEncryption().EncryptString(txtvault.Text, System.Environment.UserName))
            vaultWriter.Close()
        End Using
        Application.Exit()
    End Sub

    Private Sub frmVault_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        If e.Button = MouseButtons.Left Then
            _isMoving = True
            _mouseDownX = e.X
            _mouseDownY = e.Y
        End If
    End Sub

    Private Sub frmVault_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        If _isMoving Then
            Dim temp As Point = New Point
            temp.X = Me.Location.X + (e.X - _mouseDownX)
            temp.Y = Me.Location.Y + (e.Y - _mouseDownY)
            Me.Location = temp
        End If
    End Sub

    Private Sub frmVault_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp
        If e.Button = MouseButtons.Left Then
            _isMoving = False
        End If
    End Sub
End Class
