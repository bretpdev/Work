Module General
    Public TestMode As Boolean = True
    Public Conn As SqlClient.SqlConnection
    Public Sub initialize()
        If TestMode Then
            Conn = New SqlClient.SqlConnection("workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""OPSDEV"";persist security info=False;initial catalog=MauiDUDE")
        Else
            Conn = New SqlClient.SqlConnection("workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""NOCHOUSE"";persist security info=False;initial catalog=MauiDUDE")
        End If
    End Sub
End Module
