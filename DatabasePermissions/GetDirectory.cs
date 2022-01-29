using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabasePermissions
{
    class GetDirectory
    {

        Public Shared Function GetAuthenticatedUser(ByVal userName As String, ByVal password As String) As ActiveDirectoryUser
        Dim user As ActiveDirectoryUser = Nothing
        Dim domainQualifiedUserName As String = "UHEAA\" + userName
        Dim authenticationEntry As New DirectoryEntry("", domainQualifiedUserName, password)

        Try
            'Bind to the native ADSI object to force authentication.
            Dim obj As Object = authenticationEntry.NativeObject

            'Search for the user and get their scalar properties.
            Dim account As New NTAccount(domainQualifiedUserName)
            Dim securityId As SecurityIdentifier = DirectCast(account.Translate(GetType(SecurityIdentifier)), SecurityIdentifier)
            Dim sid As String = securityId.Value
            'Use the SID to query Active Directory for the full name and email address.
            Dim searcher As New DirectorySearcher()
            Dim searchEntry As New DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local")
            searcher.SearchRoot = searchEntry
            searcher.Filter = String.Format("(&(objectClass=user)(objectSid={0}))", sid)
            searcher.PropertiesToLoad.Add("cn")
            searcher.PropertiesToLoad.Add("mail")
            Dim result As SearchResult = searcher.FindOne()
            Dim dentry As DirectoryEntry = result.GetDirectoryEntry()
            Dim legalName As String = dentry.Properties("cn")(0).ToString()
            Dim emailAddress As String = dentry.Properties("mail")(0).ToString()

            'Get the groups the user belongs to.
            Dim groupMemberships As IEnumerable(Of String) = GetActiveDirectoryGroups(searchEntry, sid)

            'Instantiate the return object with the values retrieved from Active Directory.
            user = New ActiveDirectoryUser(sid, emailAddress, legalName, True, groupMemberships)
        Catch ex As Exception
            'Unable to authenticate. Leave the return value null.
        End Try

        Return user

        Private Shared Function GetActiveDirectoryGroups(ByVal searchEntry As DirectoryEntry, ByVal securityId As String) As IEnumerable(Of String)
        Dim searcher As New DirectorySearcher()
        searcher.SearchRoot = searchEntry
        searcher.Filter = String.Format("objectSid={0}", securityId)
        searcher.PropertiesToLoad.Add("memberOf")
        Dim result As SearchResult = searcher.FindOne()

        Dim groupMemberships As New List(Of String)()
        If (result IsNot Nothing) Then
            Const CONTAINER_NAME_INDICATOR As String = "CN="
            For Each fullyQualifiedGroupObject As Object In result.Properties("memberOf")
                Dim fullyQualifiedGroupName As String = fullyQualifiedGroupObject.ToString()
                Dim containerNameIndex As Integer = fullyQualifiedGroupName.IndexOf(CONTAINER_NAME_INDICATOR, StringComparison.CurrentCultureIgnoreCase)
                Dim commaIndex As Integer = fullyQualifiedGroupName.IndexOf(",", containerNameIndex)
                Dim group As String = fullyQualifiedGroupName.Substring(containerNameIndex + CONTAINER_NAME_INDICATOR.Length, commaIndex - containerNameIndex - CONTAINER_NAME_INDICATOR.Length)
                groupMemberships.Add(group)
            Next fullyQualifiedGroupObject
        End If
        Return groupMemberships
    }
}
