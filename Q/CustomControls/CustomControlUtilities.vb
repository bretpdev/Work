Public Class CustomControlUtilities
    ''this class hass common code that may be used by more than one custom control


    ''' <summary>
    ''' Takes a list of controls and returns true if IsDirty
    ''' </summary>
    ''' <param name="cntrls">List of controls.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckForIsDirty(ByVal cntrls As List(Of Control)) As Boolean
        For Each cntrl As Control In cntrls
            If TypeOf cntrl Is IIsDirtyExtender Then
                If CType(cntrl, IIsDirtyExtender).IsDirty = True Then
                    Return True
                End If
            Else
                If cntrl.Controls.Count > 0 Then
                    CheckForIsDirty = CustomControlUtilities.CheckForIsDirty(cntrl.Controls.Cast(Of Control).ToList())
                    If CheckForIsDirty Then
                        Return True
                    End If
                End If
            End If
        Next
        Return False
    End Function

End Class
