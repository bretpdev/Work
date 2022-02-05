using COMPFAFSA.DataObjects;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using Uheaa.Common;

namespace COMPFAFSA.DataAccess
{
    public class UsersDataAccess : DataAccessHelper, IDisposable
    {
        public static UsersDataAccess Create(IdentityFactoryOptions<UsersDataAccess> options, IOwinContext context)
        {
            return new UsersDataAccess();
        }

        string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public RegistrationResult Register(string email, SecureString password, string fullName, bool admin)
        {
            return (RegistrationResult)ExecuteSingle<int>("compfafsa.CreateAccount",
                Sp("FULLNAME", fullName),
                Sp("EMAILADDRESS", email),
                Sp("HASHEDPW", Hash(password)),
                Sp("Admin", admin));
        }

        public enum RegistrationResult
        {
            Success = 1,
            BadEmail = 2
        }

        public enum ChangePasswordResult
        {
            Success = 1,
            EmailNotFound = 2,
            ConfirmPasswordDoesNotMatch = 3,
            OldPasswordDoesNotMatch = 4
        }

        public enum LoginResult
        {
            Success = 1,
            Failure = 2,
            Lockout = 3,
            SuccessNeedSetPassword = 4
        }

        public ChangePasswordResult ChangePassword(string email, SecureString newPassword, SecureString oldPassword, SecureString confirmPassword)
        {
            string hashed = GetUserHashedPassword(email);
            if (!BCrypt.Net.BCrypt.Verify(SecureStringToString(oldPassword), hashed))
            {
                return ChangePasswordResult.OldPasswordDoesNotMatch;
            }
            else if(SecureStringToString(newPassword) != SecureStringToString(confirmPassword))
            {
                return ChangePasswordResult.ConfirmPasswordDoesNotMatch;
            }
            else
            {
                return (ChangePasswordResult)ExecuteSingle<int>("compfafsa.ChangePassword",
                    Sp("EMAILADDRESS", email),
                    Sp("HASHEDPW", Hash(newPassword)));
            }
        }

        public int? GetUserId(string email)
        {
            int? ret = ExecuteList<int?>("compfafsa.GetUserId", Sp("Email", email)).FirstOrDefault();
            return ret;
        }

        public LoginResult AuthenticationFailure(string email)
        {
            //This will update the login attempts
            var result = ExecuteSingle<string>("compfafsa.AuthenticationFailure",
                Sp("EmailAddress", email));

            switch (result.ToUpper())
            {
                case "LOCKOUT":
                    return LoginResult.Lockout;
                case "FAILURE":
                    return LoginResult.Failure;
                default:
                    return LoginResult.Failure;
            }
        }

        public LoginResult Authenticate(string email, SecureString password)
        {
            string hashed = GetUserHashedPassword(email);
            string result = "FAILURE";

            if (hashed.IsNullOrEmpty() || !BCrypt.Net.BCrypt.Verify(SecureStringToString(password), hashed))
            {
                return AuthenticationFailure(email);
            }

            result = ExecuteSingle<string>("compfafsa.AuthenticateUser",
                Sp("EmailAddress", email),
                Sp("HashedPassword", hashed)).ToString();

            switch (result.ToUpper())
            {
                case "SUCCESS":
                    return LoginResult.Success;
                case "SUCCESSNEEDSETPASSWORD":
                    return LoginResult.SuccessNeedSetPassword;
                case "LOCKOUT":
                    return LoginResult.Lockout;
                case "FAILURE":
                    return LoginResult.Failure;
                default:
                    return LoginResult.Failure;
            }
        }

        public string CreateResetPassword(string email)
        {
            string code = GetRandomNumberHash();
            var result = ExecuteSingle<int>("compfafsa.CreateResetPassword",
                Sp("Email", email),
                Sp("HashedResetPassword", code));

            var boolResult = Convert.ToBoolean(result);
            if(boolResult)
            {
                return code;
            }
            else
            {
                return null;
            }
        }

        public ChangePasswordResult ResetPassword(int userId, SecureString newPassword, SecureString confirmPassword)
        {
            if (SecureStringToString(newPassword) != SecureStringToString(confirmPassword))
            {
                return ChangePasswordResult.ConfirmPasswordDoesNotMatch;
            }
            else
            {
                return (ChangePasswordResult)ExecuteSingle<int>("compfafsa.ResetPassword",
                    Sp("UserId", userId),
                    Sp("HASHEDPW", Hash(newPassword)));
            }
        }

        public bool GetAccountLockedOut(string email)
        {
            var ret = ExecuteSingle<bool>("compfafsa.GetUserLockedOut", Sp("Email", email));
            return ret;
        }

        private string GetUserHashedPassword(string email)
        {
            return ExecuteList<string>("compfafsa.GetUserPasswordHash", Sp("Email", email)).SingleOrDefault();
        }

        public ResetPasswordResponse GetUserHashedPasswordReset(string email)
        {
            return ExecuteList<ResetPasswordResponse>("compfafsa.GetUserPasswordResetHash", Sp("Email", email)).SingleOrDefault();
        }

        public bool GetUserIsAdmin(string email)
        {
            var ret = ExecuteSingle<bool>("compfafsa.GetUserIsAdmin", Sp("EmailAddress", email));
            return ret; //Convert.ToBoolean(ret);
        }

        protected string Hash(SecureString password)
        {
            return BCrypt.Net.BCrypt.HashPassword(SecureStringToString(password), BCrypt.Net.BCrypt.GenerateSalt());
        }

        protected string GetRandomNumberHash()
        {
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
            byte[] randBytes = new byte[16];
            rand.GetBytes(randBytes);
            var randString = Convert.ToBase64String(randBytes);
            return BCrypt.Net.BCrypt.HashPassword(randString, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public override void Dispose()
        {
            
        }

        
    }
}