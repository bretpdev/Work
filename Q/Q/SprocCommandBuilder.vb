Imports System.Collections.Generic
Imports System.Linq

Public Class SprocCommandBuilder
    Private _parameterCount As Integer
    Private _parameters As Dictionary(Of String, Object)
    Private _sprocName As String

    ''' <summary>
    ''' Returns the SQL statement to execute the stored procedure using the parameters that have been added.
    ''' The statement is in the form of a format string, with a placeholder for each non-null parameter.
    ''' This should be used as the first argument in a call to DataContext.ExecutsQuery() or DataContext.ExecuteCommand(),
    ''' and the Values property should be used as the second argument to provide values for the placeholders.
    ''' </summary>
    Public ReadOnly Property Command() As String
        Get
            Dim assignments As String = String.Join(", ", _parameters.Keys.ToArray())
            Dim cmd As String = String.Format("EXEC {0} {1}", _sprocName, assignments)
            Return cmd
        End Get
    End Property

    ''' <summary>
    ''' Returns a array of the objects that have been added as parameter values.
    ''' This should be used as the second argument in a call to DataContext.ExecuteQuery() or
    ''' DataContext.ExecuteCommand() when using the Command property as the first argument.
    ''' </summary>
    Public ReadOnly Property ParameterValues() As Object()
        Get
            Return _parameters.Values.ToArray()
        End Get
    End Property

    ''' <summary>
    ''' Creates a new SprocCommandBuilder object.
    ''' </summary>
    ''' <param name="sprocName">The name of the stored procedure for which you are building a command.</param>
    Public Sub New(ByVal sprocName As String)
        _parameterCount = 0
        _parameters = New Dictionary(Of String, Object)()
        _sprocName = sprocName
    End Sub

    ''' <summary>
    ''' Adds a parameter name and its value to the list, provided the value is not null.
    ''' If the value is null, the parameter won't be added to the list, so your sproc had better have a default value set.
    ''' </summary>
    ''' <param name="name">The name of the parameter as specified by the sproc.</param>
    ''' <param name="value">The value to be passed in to the sproc for this parameter.</param>
    Public Sub AddParameter(ByVal name As String, ByVal value As Object)
        If (value Is Nothing) Then Return
        name = name.TrimStart("@")
        Dim assignmentStatement As String = String.Format("@{0} = {1}{2}{3}", name, "{", _parameterCount, "}")
        _parameters.Add(assignmentStatement, value)
        _parameterCount += 1
    End Sub
End Class
