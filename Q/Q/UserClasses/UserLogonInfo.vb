Public Class UserLogonInfo

    Private _favoriteScreen As String
    ''' <summary>
    ''' Users favorite screen.  The screen to which the Auto Logon script takes the user.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FavoriteScreen() As String
        Get
            Return _favoriteScreen
        End Get
        Set(ByVal value As String)
            _favoriteScreen = value
        End Set
    End Property

    Private _logonRegion As String
    ''' <summary>
    ''' Logon region or mode.  The region (test or live) or mode (development modes) used by the Auto Logon script to log on to the system.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LogonRegion() As String
        Get
            Return _logonRegion
        End Get
        Set(ByVal value As String)
            _logonRegion = value
        End Set
    End Property

End Class
