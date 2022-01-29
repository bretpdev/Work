using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BatchLoginDatabase
{
	public class LoginCredentais
	{
		public string UserId { get; set; }
		public string Password { get; set; }
	}

	public class UserIdsAndPasswords
	{
		public string UserNameId { get; set; }
		public string DecryptedPassword { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
		public string Notes { get; set; }
	}

	public class UserEnteredValidData
	{
		public bool CurrPasswordMatch { get; set; }
		public bool NewPasswordMatch { get; set; }
		public bool CurrPasswordMatchesNewPassword { get; set; }
		public string Ids { get; set; }

		public UserEnteredValidData()
		{
			CurrPasswordMatch = true;
			NewPasswordMatch = true;
			CurrPasswordMatchesNewPassword = false;
		}
	}

	public class Results
	{
		public int Action { get; set; }
	}
}
