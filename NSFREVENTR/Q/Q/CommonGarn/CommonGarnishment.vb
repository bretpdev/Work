Imports System.Windows.Forms

Public Class CommonGarnishment

    ''' <summary>
    ''' get the judge name from the most recent DJFMN activity record
    ''' </summary>
    ''' <param name="ri">Reflection Interface</param>
    ''' <param name="ssn">Borrower's SSN</param>
    ''' <remarks></remarks>
    Public Shared Function GetJudgesNameFromDJGNMActivityRecord(ByVal ri As ReflectionInterface, ByVal ssn As String) As String
        ri.FastPath("LP50I" & ssn)
        ri.PutText(9, 20, "DJGNM", ReflectionInterface.Key.Enter)
        If ri.Check4Text(3, 2, "SEL") Then
            ri.ReflectionSession.TransmitANSI("X")
            ri.Hit(ReflectionInterface.Key.Enter)
            Return Trim(Mid(ri.ReflectionSession.GetDisplayText(13, 2, 35), 1, InStr(1, ri.ReflectionSession.GetDisplayText(13, 2, 35), " ", 1) - 1))
        ElseIf ri.Check4Text(4, 2, "TYPE") Then
            Return Trim(Mid(ri.ReflectionSession.GetDisplayText(13, 2, 35), 1, InStr(1, ri.ReflectionSession.GetDisplayText(13, 2, 35), " ", 1) - 1))
        Else
            MessageBox.Show("The judge's name was not found.  Add the name to the garnishment document and to OneLINK after the script ends.", "Judge's Name Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return ""
        End If
    End Function

End Class
