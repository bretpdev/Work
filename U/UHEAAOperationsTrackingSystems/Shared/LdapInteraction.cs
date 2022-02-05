using System;
using System.Text;
using System.Collections;
using System.DirectoryServices;

namespace UHEAAOperationsTrackingSystems
{
	public class LdapInteraction
	{
		/// <summary>
		/// Authenticates user against Active Directory.
		/// </summary>
		public bool IsAuthenticated(string username, string pwd)
		{
			try
			{
				//Bind to the native AdsObject to force authentication.
				object obj = new DirectoryEntry(string.Empty, @"UHEAA\" + username, pwd).NativeObject;
				return true; //user has been authenticated
			}
			catch (Exception)
			{
				return false; //user couldn't be authenticated
			}
		}
	}
}
