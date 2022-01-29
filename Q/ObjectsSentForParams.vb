Public Class ObjectsSentForParams

	Public Class Imaging

		Private _testMode As Boolean
		Public Property TestMode() As Boolean
			Get
				Return _testMode
			End Get
			Set(ByVal value As Boolean)
				_testMode = value
			End Set
		End Property

		Private _efs As EnterpriseFileSystem
		Public Property Efs() As EnterpriseFileSystem
			Get
				Return _efs
			End Get
			Set(ByVal value As EnterpriseFileSystem)
				_efs = value
			End Set
		End Property

		Private _scriptId As String
		Public Property ScriptId() As String
			Get
				Return _scriptId
			End Get
			Set(ByVal value As String)
				_scriptId = value
			End Set
		End Property

		Private _acctNumFieldIndex As String
		Public Property AcctNumFieldIndex() As String
			Get
				Return _acctNumFieldIndex
			End Get
			Set(ByVal value As String)
				_acctNumFieldIndex = value
			End Set
		End Property

		Private _imagingDocId As String
		Public Property ImagingDocId() As String
			Get
				Return _imagingDocId
			End Get
			Set(ByVal value As String)
				_imagingDocId = value
			End Set
		End Property

		Private _letterTrackingDoc As String
		Public Property LetterTrackingDoc() As String
			Get
				Return _letterTrackingDoc
			End Get
			Set(ByVal value As String)
				_letterTrackingDoc = value
			End Set
		End Property

		Private _dataFile As String
		Public Property DataFile() As String
			Get
				Return _dataFile
			End Get
			Set(ByVal value As String)
				_dataFile = value
			End Set
		End Property

		Private _region As ScriptSessionBase.Region
		Public Property Region() As ScriptSessionBase.Region
			Get
				Return _region
			End Get
			Set(ByVal value As ScriptSessionBase.Region)
				_region = value
			End Set
		End Property

		Private _processOffT As Boolean
		Public Property ProcessOfT() As Boolean
			Get
				Return _processOffT
			End Get
			Set(ByVal value As Boolean)
				_processOffT = value
			End Set
		End Property

		Public Sub New(ByVal testMode As Boolean, ByVal efs As EnterpriseFileSystem, ByVal scriptId As String, ByVal acctNumFieldIndex As String, ByVal imagingDocId As String, ByVal letterTrackingDoc As String, ByVal dataFile As String, ByVal region As ScriptSessionBase.Region, ByVal processOffT As Boolean)
			_testMode = testMode
			_efs = efs
			_scriptId = scriptId
			_acctNumFieldIndex = acctNumFieldIndex
			_imagingDocId = imagingDocId
			_letterTrackingDoc = letterTrackingDoc
			_dataFile = dataFile
			_region = region
			_processOffT = processOffT
		End Sub
	End Class
End Class
