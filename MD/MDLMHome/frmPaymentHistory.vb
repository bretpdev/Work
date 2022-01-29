Public Class frmPaymentHistory
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        SP.UsrInf.ChangeFormSettingsColorsOnly(Me)
        LVHist.BackColor = Me.BackColor
        LVHist.ForeColor = Me.ForeColor
        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Me.Hide()
        'If disposing Then
        '    If Not (components Is Nothing) Then
        '        components.Dispose()
        '    End If
        'End If
        'MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents LVHist As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lbl48Pay As System.Windows.Forms.Label
    Friend WithEvents IL As System.Windows.Forms.ImageList
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPaymentHistory))
        Me.LVHist = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.IL = New System.Windows.Forms.ImageList(Me.components)
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lbl48Pay = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'LVHist
        '
        Me.LVHist.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader8, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader9})
        Me.LVHist.FullRowSelect = True
        Me.LVHist.Location = New System.Drawing.Point(8, 32)
        Me.LVHist.Name = "LVHist"
        Me.LVHist.Size = New System.Drawing.Size(704, 136)
        Me.LVHist.SmallImageList = Me.IL
        Me.LVHist.TabIndex = 0
        Me.LVHist.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Reason Reversal"
        Me.ColumnHeader1.Width = 102
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Effective Date"
        Me.ColumnHeader2.Width = 84
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Date Posted"
        Me.ColumnHeader3.Width = 76
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Trans Type"
        Me.ColumnHeader4.Width = 71
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Principal"
        Me.ColumnHeader5.Width = 73
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Interest"
        Me.ColumnHeader6.Width = 51
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Late Fee"
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Trans Amt"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Principal Balance"
        Me.ColumnHeader9.Width = 106
        '
        'IL
        '
        Me.IL.ImageSize = New System.Drawing.Size(16, 16)
        Me.IL.ImageStream = CType(resources.GetObject("IL.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IL.TransparentColor = System.Drawing.Color.Transparent
        '
        'btnClose
        '
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(400, 240)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(848, 23)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Borrower Payment History"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(720, 24)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(152, 136)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'lbl48Pay
        '
        Me.lbl48Pay.Font = New System.Drawing.Font("PosterBodoni BT", 12.0!)
        Me.lbl48Pay.ForeColor = System.Drawing.Color.Red
        Me.lbl48Pay.Location = New System.Drawing.Point(8, 200)
        Me.lbl48Pay.Name = "lbl48Pay"
        Me.lbl48Pay.Size = New System.Drawing.Size(864, 32)
        Me.lbl48Pay.TabIndex = 6
        Me.lbl48Pay.Text = "Label2"
        Me.lbl48Pay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.Label2.ImageIndex = 0
        Me.Label2.ImageList = Me.IL
        Me.Label2.Location = New System.Drawing.Point(8, 168)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(704, 24)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "       = Bounced check or some other type of payment that wasn't collectible"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmPaymentHistory
        '
        Me.AcceptButton = Me.btnClose
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(880, 262)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lbl48Pay)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.LVHist)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(888, 296)
        Me.MinimumSize = New System.Drawing.Size(888, 296)
        Me.Name = "frmPaymentHistory"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Borrower Payment History"
        Me.ResumeLayout(False)

    End Sub

#End Region

	'TODO: See if these can be replaced with structs.
    Private Data(8, 0) As String
    Private FinalData(8, 0) As String

    Public Overloads Sub Show(ByVal NumOf48OnTime As String)
        Dim I As Integer
        Dim I1 As Integer
        Dim MatchIndex As Integer
        Dim LVIdx As Integer
        Dim OpenOnly As Boolean
        Dim OpenOrAll As New frmOpenOrAll
        ReDim Data(8, 0)
        ReDim FinalData(8, 0)
        LVHist.Items.Clear()
        'prompt user for all open loans or all loans
        OpenOrAll.Show(OpenOnly)
        SP.Processing.Show()
        SP.Processing.Refresh()
        'switch keys until right option is displayed
        If SP.Q.Check4Text(1, 74, "TCX0D") Or SP.Q.Check4Text(1, 72, "TCX06") Then
            While SP.Q.Check4Text(24, 52, "LOAN") = False
                SP.Q.Hit("F2")
            End While
        ElseIf SP.Q.Check4Text(1, 72, "TCX0I") Then
            While SP.Q.Check4Text(24, 58, "LOAN") = False
                SP.Q.Hit("F2")
            End While
        End If
        'access TS26
        SP.Q.Hit("F8")
        If OpenOnly Then
            OpenLoans()
        Else
            AllLoans()
        End If
        If NumOf48OnTime = "" Or NumOf48OnTime = "0" Then
            lbl48Pay.Visible = False
        Else
            lbl48Pay.Visible = True
            lbl48Pay.Text = NumOf48OnTime & " of 48 On-time Payments"
        End If
        If Data.GetUpperBound(1) <> 0 Then
            'combine values and enter into another array
            While I < Data.GetUpperBound(1)
                MatchIndex = 0
                If Matchfound(Data(3, I), Data(1, I), Data(2, I), MatchIndex) Then
                    FinalData(4, MatchIndex) = CDbl(FinalData(4, MatchIndex)) + CDbl(Data(4, I))
                    FinalData(5, MatchIndex) = CDbl(FinalData(5, MatchIndex)) + CDbl(Data(5, I))
                    FinalData(6, MatchIndex) = CDbl(FinalData(6, MatchIndex)) + CDbl(Data(6, I))
                    FinalData(7, MatchIndex) = CDbl(FinalData(7, MatchIndex)) + CDbl(Data(7, I))
                    'if principle bal is blank then don't try an perform addition
                    If Data(8, I) <> "" Then
                        FinalData(8, MatchIndex) = CDbl(FinalData(8, MatchIndex)) + CDbl(Data(8, I))
                    End If
                Else
                    FinalData(0, FinalData.GetUpperBound(1)) = Data(0, I)
                    FinalData(1, FinalData.GetUpperBound(1)) = Data(1, I)
                    FinalData(2, FinalData.GetUpperBound(1)) = Data(2, I)
                    FinalData(3, FinalData.GetUpperBound(1)) = Data(3, I)
                    FinalData(4, FinalData.GetUpperBound(1)) = Data(4, I)
                    FinalData(5, FinalData.GetUpperBound(1)) = Data(5, I)
                    FinalData(6, FinalData.GetUpperBound(1)) = Data(6, I)
                    FinalData(7, FinalData.GetUpperBound(1)) = Data(7, I)
                    FinalData(8, FinalData.GetUpperBound(1)) = Data(8, I)
                    ReDim Preserve FinalData(8, FinalData.GetUpperBound(1) + 1)
                End If
                I += 1
			End While
			'''''''''''''''''''''''''''''''''
			' for testing					'
			'''''''''''''''''''''''''''''''''
			'While I1 < Data.GetUpperBound(1)
			'    'transfer array information to List View
			'    LVIdx = LVHist.Items.Add(Data(0, I1)).Index
			'    LVHist.Items(LVIdx).SubItems.Add(Data(1, I1))
			'    LVHist.Items(LVIdx).SubItems.Add(Data(2, I1))
			'    LVHist.Items(LVIdx).SubItems.Add(Data(3, I1))
			'    LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(Data(4, I1), 2))
			'    LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(Data(5, I1), 2))
			'    LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(Data(6, I1), 2))
			'    LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(Data(7, I1), 2))
			'    LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(Data(8, I1), 2))
			'    I1 += 1
			'End While
			'LVHist.Items.Add(" ")
			'LVHist.Items.Add(" ")
			'I1 = 0
			'''''''''''''''''''''''''''''''''
			While I1 < FinalData.GetUpperBound(1)
				'transfer array information to List View
				LVIdx = LVHist.Items.Add(FinalData(0, I1)).Index
				LVHist.Items(LVIdx).SubItems.Add(FinalData(1, I1))
				LVHist.Items(LVIdx).SubItems.Add(FinalData(2, I1))
				LVHist.Items(LVIdx).SubItems.Add(FinalData(3, I1))
				LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(FinalData(7, I1), 2))
				LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(FinalData(4, I1), 2))
				LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(FinalData(5, I1), 2))
				LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(FinalData(6, I1), 2))
				'LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(FinalData(7, I1), 2))
				If FinalData(8, I1) <> "" Then
					LVHist.Items(LVIdx).SubItems.Add(FormatCurrency(FinalData(8, I1), 2))
				Else
					LVHist.Items(LVIdx).ImageIndex = 0
				End If
				I1 += 1
			End While

			'return to ACP
			While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
				SP.Q.Hit("F12")
			End While
			SP.Processing.Hide()
			MyBase.Show()
		Else
            'return to ACP
            While SP.Q.Check4Text(1, 74, "TCX0D") = False And SP.Q.Check4Text(1, 72, "TCX06") = False And SP.Q.Check4Text(1, 72, "TCX0I") = False
                SP.Q.Hit("F12")
            End While
            SP.Processing.Hide()
            MyBase.Show()
            SP.frmWhoaDUDE.WhoaDUDE("No results were found.", "Payment History")
        End If
	End Sub

	'Search for an array match.
	Private Function Matchfound(ByVal TranType As String, ByVal EffDt As String, ByVal DtPosted As String, ByRef MI As Integer) As Boolean
		While MI < FinalData.GetUpperBound(1)
			If FinalData(3, MI) = TranType And FinalData(1, MI) = EffDt And FinalData(2, MI) = DtPosted Then
				Return True
			End If
			MI += 1
		End While
		Return False
	End Function

	'Collect the information for all loans on TS26.
	Private Sub AllLoans()
		Dim Row As Integer
		Dim SubRow As Integer
		Dim I As Integer
		If SP.Q.Check4Text(1, 72, "TSX28") Then
			'selection screen
			Row = 8
			While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
				If SP.Q.GetText(Row, 2, 2).Length > 1 Then
					SP.Q.PutText(21, 12, CStr(SP.Q.GetText(Row, 2, 2)), True)
				ElseIf SP.Q.GetText(Row, 2, 2).Length = 1 Then
					SP.Q.PutText(21, 12, "0" & CStr(SP.Q.GetText(Row, 2, 2)), True)
				Else
					Exit While
				End If
				'target screen
				SP.Q.Hit("F6")			 'page to FIN screen
				'Blank Active Indicator
				SP.Q.RIBM.MoveCursor(8, 2)
				SP.Q.Hit("End")
				'blank All field
				SP.Q.RIBM.MoveCursor(8, 51)
				SP.Q.Hit("End")
				'Select Active/Rev option
				SP.Q.PutText(10, 2, "X")
				SP.Q.PutText(10, 51, "X")
				SP.Q.PutText(16, 51, "X")
				SP.Q.PutText(17, 51, "X")
				SP.Q.PutText(18, 51, "X")
				SP.Q.PutText(20, 51, "X", True)
				If SP.Q.Check4Text(1, 72, "TSX7S") = False Then
					SubRow = 11
					'cycle through all selection rows and get the REV REA
					While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
						If SP.Q.Check4Text(SubRow, 3, " ") = False Then
							'translate number to understandable code
							If SP.Q.GetText(SubRow, 8, 1) = "" Then
								Data(0, Data.GetUpperBound(1)) = ""
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "1" Or _
								   SP.Q.GetText(SubRow, 8, 1) = "6" Then
								Data(0, Data.GetUpperBound(1)) = "NSF"
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "2" Or _
								   SP.Q.GetText(SubRow, 8, 1) = "3" Or _
								   SP.Q.GetText(SubRow, 8, 1) = "4" Or _
								   SP.Q.GetText(SubRow, 8, 1) = "7" Then
								Data(0, Data.GetUpperBound(1)) = "Post Error"
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "5" Then
								Data(0, Data.GetUpperBound(1)) = "Refund"
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "8" Then
								Data(0, Data.GetUpperBound(1)) = "Bad Check"
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "9" Then
								Data(0, Data.GetUpperBound(1)) = "Rev/Apply"
							End If
							Data(7, Data.GetUpperBound(1)) = SP.Q.GetText(SubRow, 41, 10)					  'Trans Amt
							Data(8, Data.GetUpperBound(1)) = SP.Q.GetText(SubRow, 68, 10)					  'Princ Bal
							ReDim Preserve Data(8, (Data.GetUpperBound(1) + 1))
						End If
						SubRow += 1
						If SP.Q.Check4Text(SubRow, 3, " ") Then
							SubRow = 11
							SP.Q.Hit("F8")
						End If
					End While
					'back up and enter the target screen for each selection Row
					SP.Q.Hit("F12")
					SP.Q.Hit("Enter")
					'select for row
					SP.Q.PutText(22, 18, "1", True)
					'collect the rest of the information for each record
					While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
						Data(1, I) = SP.Q.GetText(16, 66, 8)				   'effct date
						Data(2, I) = SP.Q.GetText(18, 24, 8)				   'date posted
						If SP.Q.Check4Text(16, 24, "PAYMENT") Then
							Data(3, I) = SP.Q.GetText(17, 24, 8)					  'trans type
						Else
							Data(3, I) = SP.Q.GetText(16, 24, 8)					  'trans type
						End If
						Data(4, I) = SP.Q.GetText(13, 15, 10)				   'principal
						Data(5, I) = SP.Q.GetText(13, 28, 10)				   'Interest
						Data(6, I) = SP.Q.GetText(13, 42, 10)				   'Late Fees
						SP.Q.Hit("F8")
						I += 1				   'move to next row in array
					End While
					SP.Q.Hit("F12")
					SP.Q.Hit("F12")
					SP.Q.Hit("F12")
					SP.Q.Hit("F12")
				Else			 'if fin info isn't found
					SP.Q.Hit("F12")
					SP.Q.Hit("F12")
				End If
				Row += 1
				If Row = 20 Then
					Row = 8
					SP.Q.Hit("F8")
				End If
			End While
		Else
			'target screen
			SP.Q.Hit("F6")		  'page to FIN screen
			'Blank Active Indicator
			SP.Q.RIBM.MoveCursor(8, 2)
			SP.Q.Hit("End")
			'blank All field
			SP.Q.RIBM.MoveCursor(8, 51)
			SP.Q.Hit("End")
			'Select Active/Rev option
			SP.Q.PutText(10, 2, "X")
			SP.Q.PutText(10, 51, "X")
			SP.Q.PutText(16, 51, "X")
			SP.Q.PutText(17, 51, "X")
			SP.Q.PutText(18, 51, "X")
			SP.Q.PutText(20, 51, "X", True)
			If SP.Q.Check4Text(1, 72, "TSX7S") = False Then
				SubRow = 11
				'cycle through all selection rows and get the REV REA
				While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
					If SP.Q.Check4Text(SubRow, 3, " ") = False Then
						'translate number to understandable code
						If SP.Q.GetText(SubRow, 8, 1) = "" Then
							Data(0, Data.GetUpperBound(1)) = ""
						ElseIf SP.Q.GetText(SubRow, 8, 1) = "1" Or _
							   SP.Q.GetText(SubRow, 8, 1) = "6" Then
							Data(0, Data.GetUpperBound(1)) = "NSF"
						ElseIf SP.Q.GetText(SubRow, 8, 1) = "2" Or _
							   SP.Q.GetText(SubRow, 8, 1) = "3" Or _
							   SP.Q.GetText(SubRow, 8, 1) = "4" Or _
							   SP.Q.GetText(SubRow, 8, 1) = "7" Then
							Data(0, Data.GetUpperBound(1)) = "Post Error"
						ElseIf SP.Q.GetText(SubRow, 8, 1) = "5" Then
							Data(0, Data.GetUpperBound(1)) = "Refund"
						ElseIf SP.Q.GetText(SubRow, 8, 1) = "8" Then
							Data(0, Data.GetUpperBound(1)) = "Bad Check"
						ElseIf SP.Q.GetText(SubRow, 8, 1) = "9" Then
							Data(0, Data.GetUpperBound(1)) = "Rev/Apply"
						End If
						Data(7, Data.GetUpperBound(1)) = SP.Q.GetText(SubRow, 41, 10)				   'Trans Amt
						Data(8, Data.GetUpperBound(1)) = SP.Q.GetText(SubRow, 68, 10)				   'Princ Bal
						ReDim Preserve Data(8, (Data.GetUpperBound(1) + 1))
					End If
					SubRow += 1
					If SP.Q.Check4Text(SubRow, 3, " ") Then
						SubRow = 11
						SP.Q.Hit("F8")
					End If
				End While
				'back up and enter the target screen for each selection Row
				SP.Q.Hit("F12")
				SP.Q.Hit("Enter")
				'select for row
				SP.Q.PutText(22, 18, "1", True)
				'collect the rest of the information for each record
				While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
					Data(1, I) = SP.Q.GetText(16, 66, 8)				'effct date
					Data(2, I) = SP.Q.GetText(18, 24, 8)				'date posted
					Data(3, I) = SP.Q.GetText(16, 24, 8)				'trans type
					Data(4, I) = SP.Q.GetText(13, 15, 10)				'principal
					Data(5, I) = SP.Q.GetText(13, 28, 10)				'Interest
					Data(6, I) = SP.Q.GetText(13, 42, 10)				'Late Fees
					SP.Q.Hit("F8")
					I += 1				'move to next row in array
				End While
				SP.Q.Hit("F12")
				SP.Q.Hit("F12")
				SP.Q.Hit("F12")
			Else		  'if fin info isn't found
				SP.Q.Hit("F12")
			End If
		End If
	End Sub

	'Collect the information for all open loans on TS26.
	Private Sub OpenLoans()
		Dim Row As Integer
		Dim SubRow As Integer
		Dim I As Integer
		Dim FoundOpenLn As Boolean = False
		If SP.Q.Check4Text(1, 72, "TSX28") Then
			'selection screen
			Row = 8
			While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
				FoundOpenLn = False
				If (SP.Q.Check4Text(Row, 64, " 0.00") = False And SP.Q.Check4Text(Row, 69, "CR") = False) Or SP.Q.Check4Text(Row, 59, "          ") Then
					If SP.Q.GetText(Row, 2, 2).Length > 1 Then
						SP.Q.PutText(21, 12, CStr(SP.Q.GetText(Row, 2, 2)), True)
					ElseIf SP.Q.GetText(Row, 2, 2).Length = 1 Then
						SP.Q.PutText(21, 12, "0" & CStr(SP.Q.GetText(Row, 2, 2)), True)
					Else
						Exit While
					End If
					FoundOpenLn = True
				End If
				If FoundOpenLn Then
					'target screen
					SP.Q.Hit("F6")				'page to FIN screen
					'Blank Active Indicator
					SP.Q.RIBM.MoveCursor(8, 2)
					SP.Q.Hit("End")
					'blank All field
					SP.Q.RIBM.MoveCursor(8, 51)
					SP.Q.Hit("End")
					'Select Active/Rev option
					SP.Q.PutText(10, 2, "X")
					SP.Q.PutText(10, 51, "X")
					SP.Q.PutText(16, 51, "X")
					SP.Q.PutText(17, 51, "X")
					SP.Q.PutText(18, 51, "X")
					SP.Q.PutText(20, 51, "X", True)
					If SP.Q.Check4Text(1, 72, "TSX7S") = False Then
						SubRow = 11
						'cycle through all selection rows and get the REV REA
						While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
							If SP.Q.Check4Text(SubRow, 3, " ") = False Then
								'translate number to understandable code
								If SP.Q.GetText(SubRow, 8, 1) = "" Then
									Data(0, Data.GetUpperBound(1)) = ""
								ElseIf SP.Q.GetText(SubRow, 8, 1) = "1" Or _
									   SP.Q.GetText(SubRow, 8, 1) = "6" Then
									Data(0, Data.GetUpperBound(1)) = "NSF"
								ElseIf SP.Q.GetText(SubRow, 8, 1) = "2" Or _
									   SP.Q.GetText(SubRow, 8, 1) = "3" Or _
									   SP.Q.GetText(SubRow, 8, 1) = "4" Or _
									   SP.Q.GetText(SubRow, 8, 1) = "7" Then
									Data(0, Data.GetUpperBound(1)) = "Post Error"
								ElseIf SP.Q.GetText(SubRow, 8, 1) = "5" Then
									Data(0, Data.GetUpperBound(1)) = "Refund"
								ElseIf SP.Q.GetText(SubRow, 8, 1) = "8" Then
									Data(0, Data.GetUpperBound(1)) = "Bad Check"
								ElseIf SP.Q.GetText(SubRow, 8, 1) = "9" Then
									Data(0, Data.GetUpperBound(1)) = "Rev/Apply"
								End If
								Data(7, Data.GetUpperBound(1)) = SP.Q.GetText(SubRow, 41, 10)						 'Trans Amt
								Data(8, Data.GetUpperBound(1)) = SP.Q.GetText(SubRow, 68, 10)						 'Princ Bal
								ReDim Preserve Data(8, (Data.GetUpperBound(1) + 1))
							End If
							SubRow += 1
							If SP.Q.Check4Text(SubRow, 3, " ") Then
								SubRow = 11
								SP.Q.Hit("F8")
							End If
						End While
						'back up and enter the target screen for each selection Row
						SP.Q.Hit("F12")
						SP.Q.Hit("Enter")
						'select for row
						SP.Q.PutText(22, 18, "1", True)
						'collect the rest of the information for each record
						While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
							Data(1, I) = SP.Q.GetText(16, 66, 8)					  'effct date
							Data(2, I) = SP.Q.GetText(18, 24, 8)					  'date posted
							Data(3, I) = SP.Q.GetText(16, 24, 8)					  'trans type
							Data(4, I) = SP.Q.GetText(13, 15, 10)					  'principal
							Data(5, I) = SP.Q.GetText(13, 28, 10)					  'Interest
							Data(6, I) = SP.Q.GetText(13, 42, 10)					  'Late Fees
							SP.Q.Hit("F8")
							I += 1					  'move to next row in array
						End While
						SP.Q.Hit("F12")
						SP.Q.Hit("F12")
						SP.Q.Hit("F12")
						SP.Q.Hit("F12")
					Else				'if fin info isn't found
						SP.Q.Hit("F12")
						SP.Q.Hit("F12")
					End If
				End If
				Row += 1
				If Row = 20 Then
					Row = 8
					SP.Q.Hit("F8")
				End If
			End While
		Else
			If SP.Q.Check4Text(11, 12, "0.00") = False And SP.Q.Check4Text(11, 22, "CR") = False Then
				'target screen
				SP.Q.Hit("F6")			 'page to FIN screen
				'Blank Active Indicator
				SP.Q.RIBM.MoveCursor(8, 2)
				SP.Q.Hit("End")
				'blank All field
				SP.Q.RIBM.MoveCursor(8, 51)
				SP.Q.Hit("End")
				'Select Active/Rev option
				SP.Q.PutText(10, 2, "X")
				SP.Q.PutText(10, 51, "X")
				SP.Q.PutText(16, 51, "X")
				SP.Q.PutText(17, 51, "X")
				SP.Q.PutText(18, 51, "X")
				SP.Q.PutText(20, 51, "X", True)
				If SP.Q.Check4Text(1, 72, "TSX7S") = False Then
					SubRow = 11
					'cycle through all selection rows and get the REV REA
					While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
						If SP.Q.Check4Text(SubRow, 3, " ") = False Then
							'translate number to understandable code
							If SP.Q.GetText(SubRow, 8, 1) = "" Then
								Data(0, Data.GetUpperBound(1)) = ""
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "1" Or _
								   SP.Q.GetText(SubRow, 8, 1) = "6" Then
								Data(0, Data.GetUpperBound(1)) = "NSF"
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "2" Or _
								   SP.Q.GetText(SubRow, 8, 1) = "3" Or _
								   SP.Q.GetText(SubRow, 8, 1) = "4" Or _
								   SP.Q.GetText(SubRow, 8, 1) = "7" Then
								Data(0, Data.GetUpperBound(1)) = "Post Error"
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "5" Then
								Data(0, Data.GetUpperBound(1)) = "Refund"
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "8" Then
								Data(0, Data.GetUpperBound(1)) = "Bad Check"
							ElseIf SP.Q.GetText(SubRow, 8, 1) = "9" Then
								Data(0, Data.GetUpperBound(1)) = "Rev/Apply"
							End If
							Data(7, Data.GetUpperBound(1)) = SP.Q.GetText(SubRow, 41, 10)					  'Trans Amt
							Data(8, Data.GetUpperBound(1)) = SP.Q.GetText(SubRow, 68, 10)					  'Princ Bal
							ReDim Preserve Data(8, (Data.GetUpperBound(1) + 1))
						End If
						SubRow += 1
						If SP.Q.Check4Text(SubRow, 3, " ") Then
							SubRow = 11
							SP.Q.Hit("F8")
						End If
					End While
					'back up and enter the target screen for each selection Row
					SP.Q.Hit("F12")
					SP.Q.Hit("Enter")
					'select for row
					SP.Q.PutText(22, 18, "1", True)
					'collect the rest of the information for each record
					While SP.Q.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") = False
						Data(1, I) = SP.Q.GetText(16, 66, 8)				   'effct date
						Data(2, I) = SP.Q.GetText(18, 24, 8)				   'date posted
						Data(3, I) = SP.Q.GetText(16, 24, 8)				   'trans type
						Data(4, I) = SP.Q.GetText(13, 15, 10)				   'principal
						Data(5, I) = SP.Q.GetText(13, 28, 10)				   'Interest
						Data(6, I) = SP.Q.GetText(13, 42, 10)				   'Late Fees
						SP.Q.Hit("F8")
						I += 1				   'move to next row in array
					End While
					SP.Q.Hit("F12")
					SP.Q.Hit("F12")
					SP.Q.Hit("F12")
				Else			 'if fin info isn't found
					SP.Q.Hit("F12")
				End If
			End If
		End If
	End Sub

	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Me.Hide()
	End Sub
End Class
