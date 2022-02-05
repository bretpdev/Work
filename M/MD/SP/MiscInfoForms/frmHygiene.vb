Imports System.Drawing
Public Class frmHygiene
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtrules As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtsecondary As System.Windows.Forms.TextBox
    Friend WithEvents txtrules2 As System.Windows.Forms.TextBox
    Friend WithEvents txtsecondary2 As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmHygiene))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtrules = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtsecondary = New System.Windows.Forms.TextBox
        Me.txtrules2 = New System.Windows.Forms.TextBox
        Me.txtsecondary2 = New System.Windows.Forms.TextBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(360, 23)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Good Address Hygiene Guide"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtrules
        '
        Me.txtrules.BackColor = System.Drawing.SystemColors.Window
        Me.txtrules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtrules.Location = New System.Drawing.Point(8, 88)
        Me.txtrules.Multiline = True
        Me.txtrules.Name = "txtrules"
        Me.txtrules.ReadOnly = True
        Me.txtrules.Size = New System.Drawing.Size(64, 296)
        Me.txtrules.TabIndex = 2
        Me.txtrules.TabStop = False
        Me.txtrules.Text = ""
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(176, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(192, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Secondary Unit Abbreviations"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(168, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Street Suffix Abbreviations"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 32)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(360, 32)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "*We should use these abbreviations instead of spelling out the words."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtsecondary
        '
        Me.txtsecondary.BackColor = System.Drawing.SystemColors.Window
        Me.txtsecondary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtsecondary.Location = New System.Drawing.Point(256, 88)
        Me.txtsecondary.Multiline = True
        Me.txtsecondary.Name = "txtsecondary"
        Me.txtsecondary.ReadOnly = True
        Me.txtsecondary.Size = New System.Drawing.Size(64, 160)
        Me.txtsecondary.TabIndex = 6
        Me.txtsecondary.TabStop = False
        Me.txtsecondary.Text = ""
        '
        'txtrules2
        '
        Me.txtrules2.BackColor = System.Drawing.SystemColors.Window
        Me.txtrules2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtrules2.Location = New System.Drawing.Point(80, 88)
        Me.txtrules2.Multiline = True
        Me.txtrules2.Name = "txtrules2"
        Me.txtrules2.ReadOnly = True
        Me.txtrules2.Size = New System.Drawing.Size(40, 296)
        Me.txtrules2.TabIndex = 7
        Me.txtrules2.TabStop = False
        Me.txtrules2.Text = ""
        '
        'txtsecondary2
        '
        Me.txtsecondary2.BackColor = System.Drawing.SystemColors.Window
        Me.txtsecondary2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtsecondary2.Location = New System.Drawing.Point(328, 88)
        Me.txtsecondary2.Multiline = True
        Me.txtsecondary2.Name = "txtsecondary2"
        Me.txtsecondary2.ReadOnly = True
        Me.txtsecondary2.Size = New System.Drawing.Size(40, 160)
        Me.txtsecondary2.TabIndex = 8
        Me.txtsecondary2.TabStop = False
        Me.txtsecondary2.Text = ""
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(128, 392)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(112, 23)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "Close"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(128, 256)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(232, 16)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Eliminate all punctuation and/or symbols."
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(128, 280)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(240, 56)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "If the apartment, unit or suite number won’t fit on the first line, move everythi" & _
        "ng else to the second line and keep the apt, unit or suite on the first line."
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(128, 344)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(232, 32)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "c/o ""John Doe"" needs to be handled the same way as Apartment numbers."
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(136, 88)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(104, 160)
        Me.PictureBox1.TabIndex = 13
        Me.PictureBox1.TabStop = False
        '
        'frmHygiene
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(376, 422)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtsecondary2)
        Me.Controls.Add(Me.txtrules2)
        Me.Controls.Add(Me.txtsecondary)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtrules)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmHygiene"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmHygiene"
        Me.ResumeLayout(False)

    End Sub

#End Region

    'this function overloads the frmDemographics show function
    Public Shadows Function Show() As Boolean
        'initalize the form componenets (blank all data holding text boxes and labels)
        'InitFormComponents()

        'set opacity level
        '****Me.Opacity = TransPar
        'set backcolor
        '****Me.BackColor = BackColor
        'set forecolor
        '****Me.ForeColor = ForeColor

        txtrules.BackColor = BackColor
        txtrules.ForeColor = ForeColor
        txtrules2.BackColor = BackColor
        txtrules2.ForeColor = ForeColor
        txtsecondary.BackColor = BackColor
        txtsecondary.ForeColor = ForeColor
        txtsecondary2.BackColor = BackColor
        txtsecondary2.ForeColor = ForeColor


        txtrules.Text = "Alley" & vbCrLf & _
                        "Avenue" & vbCrLf & _
                        "Boulevard" & vbCrLf & _
                        "Center" & vbCrLf & _
                        "Circle" & vbCrLf & _
                        "Cove" & vbCrLf & _
                        "Court" & vbCrLf & _
                        "Drive" & vbCrLf & _
                        "Lane" & vbCrLf & _
                        "Park" & vbCrLf & _
                        "Parkway" & vbCrLf & _
                        "Place" & vbCrLf & _
                        "Ridge" & vbCrLf & _
                        "Road" & vbCrLf & _
                        "Street" & vbCrLf & _
                        "Summit" & vbCrLf & _
                        "Terrace" & vbCrLf & _
                        "Trailer" & vbCrLf & _
                        "Valley" & vbCrLf & _
                        "Village" & vbCrLf & _
                        "Vista" & vbCrLf & _
                        "Way"
        txtrules2.Text = "ALY" & vbCrLf & _
                        "AVE" & vbCrLf & _
                        "BLVD" & vbCrLf & _
                        "CTR" & vbCrLf & _
                        "CIR" & vbCrLf & _
                        "CV" & vbCrLf & _
                        "CT" & vbCrLf & _
                        "DR" & vbCrLf & _
                        "LN" & vbCrLf & _
                        "PARK" & vbCrLf & _
                        "PKY" & vbCrLf & _
                        "PL" & vbCrLf & _
                        "RDG" & vbCrLf & _
                        "RD" & vbCrLf & _
                        "ST" & vbCrLf & _
                        "SMT" & vbCrLf & _
                        "TER" & vbCrLf & _
                        "TRL" & vbCrLf & _
                        "VLY" & vbCrLf & _
                        "VLG" & vbCrLf & _
                        "VIS" & vbCrLf & _
                        "WAY"
        txtsecondary.Text = "Apartment" & vbCrLf & _
                            "Basement" & vbCrLf & _
                            "Building" & vbCrLf & _
                            "Lot" & vbCrLf & _
                            "Lower" & vbCrLf & _
                            "Office" & vbCrLf & _
                            "Penthouse" & vbCrLf & _
                            "Suite" & vbCrLf & _
                            "Trailer" & vbCrLf & _
                            "Unit" & vbCrLf & _
                            "Upper"
        txtsecondary2.Text = "APT" & vbCrLf & _
                            "BSMT" & vbCrLf & _
                            "BLDG" & vbCrLf & _
                            "LOT" & vbCrLf & _
                            "LOWR" & vbCrLf & _
                            "OFC" & vbCrLf & _
                            "PH" & vbCrLf & _
                            "STE" & vbCrLf & _
                            "TRLR" & vbCrLf & _
                            "UNIT" & vbCrLf & _
                            "UPPR"

        Me.Showdialog()
        Return True
        'End If
        'Return False
    End Function

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Hide()
    End Sub

End Class
