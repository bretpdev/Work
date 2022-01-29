Imports System.Globalization
Imports System.Threading

Public Class NullableDateTimePicker
    Private _isNull As Boolean = True 'True when no date should be displayed.
    Private _nullValue As String = "" 'Value displayed when _isNull is True.
    Private _format As DateTimePickerFormat = DateTimePickerFormat.Short 'Display format.
    Private _customFormat As String = ""
    Private _formatAsString As String = ""

#Region "Properties"
    Public Overloads Property CustomFormat() As String
        Get
            Return _customFormat
        End Get
        Set(ByVal value As String)
            _customFormat = value
        End Set
    End Property

    Public Overloads Property Format() As DateTimePickerFormat
        Get
            Return _format
        End Get
        Set(ByVal value As DateTimePickerFormat)
            _format = value
            SetFormat()
            OnFormatChanged(EventArgs.Empty)
        End Set
    End Property

    Public Property FormatAsString() As String
        Get
            Return _formatAsString
        End Get
        Set(ByVal value As String)
            _formatAsString = value
            MyBase.CustomFormat = value
        End Set
    End Property

    Public Property NullValue() As String
        Get
            Return _nullValue
        End Get
        Set(ByVal value As String)
            _nullValue = value
        End Set
    End Property

    Public Overloads Property Value() As Object
        Get
            If _isNull Then
                Return Nothing
            Else
                Return MyBase.Value
            End If
        End Get
        Set(ByVal value As Object)
            If value Is Nothing OrElse value Is DBNull.Value Then
                SetToNullValue()
            Else
                SetToDateTimeValue()
                MyBase.Value = CType(value, DateTime)
            End If
        End Set
    End Property
#End Region 'Properties

#Region "Events"
    Protected Overrides Sub OnCloseUp(ByVal eventargs As EventArgs)
        If Control.MouseButtons = Windows.Forms.MouseButtons.None AndAlso _isNull Then
            SetToDateTimeValue()
            _isNull = False
        End If
        MyBase.OnCloseUp(eventargs)
    End Sub

    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Delete OrElse e.KeyCode = Keys.Back Then
            Me.Value = Nothing
            OnValueChanged(EventArgs.Empty)
        End If
        MyBase.OnKeyUp(e)
    End Sub
#End Region 'Events

    Public Sub New()
        MyBase.New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MyBase.Format = DateTimePickerFormat.Custom
        NullValue = " "
        Me.Format = DateTimePickerFormat.Short
    End Sub

    Private Sub SetFormat()
        Dim dtf As DateTimeFormatInfo = Thread.CurrentThread.CurrentCulture.DateTimeFormat
        Select Case _format
            Case DateTimePickerFormat.Long
                FormatAsString = dtf.LongDatePattern
            Case DateTimePickerFormat.Short
                FormatAsString = dtf.ShortDatePattern
            Case DateTimePickerFormat.Time
                FormatAsString = dtf.ShortTimePattern
            Case DateTimePickerFormat.Custom
                FormatAsString = Me.CustomFormat
        End Select
    End Sub

    Private Sub SetToDateTimeValue()
        If _isNull Then
            SetFormat()
            _isNull = False
            MyBase.OnValueChanged(New EventArgs())
        End If
    End Sub

    Private Sub SetToNullValue()
        _isNull = True
        MyBase.CustomFormat = If(String.IsNullOrEmpty(_nullValue), " ", String.Format("'{0}'", Me.NullValue))
    End Sub
End Class
