using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMPFAFSA.DataObjects
{
    public class ResetPasswordResponse
    {
        public string HashedResetPassword { get; set; }
        public bool Expired { get; set; }
    }
}