<CLSCompliant(True)> _
Public MustInherit Class Demographics
    Public Addr1 As String
    Public Addr2 As String
    ''' <summary>
    ''' Address Line 3
    ''' </summary>
    ''' <remarks>This was added for COMPASS compatibility.  Staff have been instructed to not use this line so, PA should have to code for it..</remarks>
    Public Addr3 As String
    Public City As String
    Public State As String
    Public Zip As String
    Public ForeignState As String
    Public Country As String
    Public Phone As String
    Public AltPhone As String
    Public Email As String

    ''' <summary>
    ''' Applies the following abbreviations to the address lines 1, 2 and 3
    ''' Replaces "STREET" with "ST"
    ''' Replace "AVENUE" with "AVE"
    ''' Replace "ROAD" with "RD"
    ''' Replace "LANE" with "LN"
    ''' Replace "DRIVE" with "DR"
    ''' Replace "HIGHWAY" with "HWY"
    ''' Replace "FLOOR" with "FL"
    ''' Replace "P O BOX" with "PO BOX"
    ''' Replace "P O BX" with "PO BOX"
    ''' Replace "-" with " "
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ApplyStandardAbbreviationsToAllAddressLines()
        If Not (Addr1 Is Nothing) Then
            Addr1 = ApplyStandardAbbreviations(Addr1)
        End If
        If Not (Addr2 Is Nothing) Then
            Addr2 = ApplyStandardAbbreviations(Addr2)
        End If
        If Not (Addr3 Is Nothing) Then
            Addr3 = ApplyStandardAbbreviations(Addr3)
        End If
    End Sub

    ''' <summary>
    ''' Applies the following abbreviations to the address part
    ''' Replaces "STREET" with "ST"
    ''' Replace "AVENUE" with "AVE"
    ''' Replace "ROAD" with "RD"
    ''' Replace "LANE" with "LN"
    ''' Replace "DRIVE" with "DR"
    ''' Replace "HIGHWAY" with "HWY"
    ''' Replace "FLOOR" with "FL"
    ''' Replace "P O BOX" with "PO BOX"
    ''' Replace "P O BX" with "PO BOX"
    ''' Replace "-" with " "
    ''' </summary>
    ''' <param name="addressPart">String to apply standard abbreviations to.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ApplyStandardAbbreviations(ByVal addressPart As String) As String
        addressPart = addressPart.Replace("STREET", "ST")
        addressPart = addressPart.Replace("AVENUE", "AVE")
        addressPart = addressPart.Replace("ROAD", "RD")
        addressPart = addressPart.Replace("LANE", "LN")
        addressPart = addressPart.Replace("DRIVE", "DR")
        addressPart = addressPart.Replace("HIGHWAY", "HWY")
        addressPart = addressPart.Replace("FLOOR", "FL")
        addressPart = addressPart.Replace("P O BOX", "PO BOX")
        addressPart = addressPart.Replace("P O BX", "PO BOX")
        addressPart = addressPart.Replace("-", " ")
        Return addressPart
    End Function

End Class
