<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FourOneOneControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.lblSubject = New System.Windows.Forms.Label
        Me.CommunicationBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.txtText = New System.Windows.Forms.TextBox
        Me.ttpSubject = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.CommunicationBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSubject
        '
        Me.lblSubject.AutoSize = True
        Me.lblSubject.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CommunicationBindingSource, "Subject", True))
        Me.lblSubject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubject.Location = New System.Drawing.Point(4, 4)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.Size = New System.Drawing.Size(45, 13)
        Me.lblSubject.TabIndex = 0
        Me.lblSubject.Text = "Label1"
        '
        'CommunicationBindingSource
        '
        Me.CommunicationBindingSource.DataSource = GetType(RegentsScholarshipBackEnd.Communication)
        '
        'txtText
        '
        Me.txtText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtText.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.CommunicationBindingSource, "Text", True))
        Me.txtText.Location = New System.Drawing.Point(22, 20)
        Me.txtText.Multiline = True
        Me.txtText.Name = "txtText"
        Me.txtText.ReadOnly = True
        Me.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtText.Size = New System.Drawing.Size(246, 65)
        Me.txtText.TabIndex = 1
        '
        'ttpSubject
        '
        Me.ttpSubject.AutoPopDelay = 0
        Me.ttpSubject.InitialDelay = 500
        Me.ttpSubject.IsBalloon = True
        Me.ttpSubject.ReshowDelay = 100
        '
        'FourOneOneControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.txtText)
        Me.Controls.Add(Me.lblSubject)
        Me.Name = "FourOneOneControl"
        Me.Size = New System.Drawing.Size(271, 88)
        CType(Me.CommunicationBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSubject As System.Windows.Forms.Label
    Friend WithEvents CommunicationBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents txtText As System.Windows.Forms.TextBox
    Friend WithEvents ttpSubject As System.Windows.Forms.ToolTip

End Class
