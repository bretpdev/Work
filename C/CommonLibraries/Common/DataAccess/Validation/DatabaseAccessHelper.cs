using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Uheaa.Common.DataAccess
{
    public static class DatabaseAccessHelper
    {
        /// <summary>
        /// Generates and automatically displays a Sproc Access Alert to the user.
        /// </summary>
        /// <returns>True if there are no errors.  False otherwise.</returns>
        public static bool StandardSprocAccessCheck(Assembly assembly)
        {
            string message = GenerateSprocAccessAlert(assembly);
            if (message != null)
            {
                Dialog.Error.Ok(message);
                return false;
            }
            return true;
        }
        public static List<SprocValidationResult> CheckSprocAccess(Assembly assembly)
        {
            List<SprocValidationResult> badSprocs = new List<SprocValidationResult>();
            foreach (Type type in assembly.GetTypes())
            {
                foreach (MethodInfo mi in type.GetMethods())
                {
                    var attr = mi.GetCustomAttributes(typeof(UsesSprocAttribute), true);
                    if (attr != null && attr.Length > 0)
                    {
                        foreach (var sproc in attr.Cast<UsesSprocAttribute>())
                        {
                            var result = CheckSpecificSprocAccess(sproc.SprocName, sproc.Database);
                            if (!result.HasAccess)
                            {
                                if (result.Exception != null)
                                    badSprocs.Add(SprocValidationResult.Error(sproc, result.Exception));
                                else
                                    badSprocs.Add(SprocValidationResult.Failure(sproc));
                            }
                        }
                    }
                }
            }
            return badSprocs;
        }
        public class SprocAccessResult
        {
            public bool HasAccess { get; set; }
            public Exception Exception { get; set; }
            public SprocAccessResult(bool hasAccess) { HasAccess = hasAccess; }
            public SprocAccessResult(Exception ex) { Exception = ex; HasAccess = false; }
        }
        public static SprocAccessResult CheckSpecificSprocAccess(string sprocName, DataAccessHelper.Database db)
        {
            string query = "";
            if (sprocName.Contains("."))
                query = string.Format("select * from fn_my_permissions('{0}.{1}', 'OBJECT') where permission_name='EXECUTE';", db.ToString().ToLower(), sprocName);
            else
                query = string.Format("select * from fn_my_permissions('{0}.dbo.{1}', 'OBJECT') where permission_name='EXECUTE';", db.ToString().ToLower(), sprocName);
            try
            {
                if (DataAccessHelper.GetContext(db).ExecuteQuery<object>(query).Count() == 0)
                    return new SprocAccessResult(false); //no execute permission
            }
            catch (Exception ex)
            {
                return new SprocAccessResult(ex);
            }
            return new SprocAccessResult(true);
        }

        public static string GenerateSprocAccessAlert(Assembly assembly)
        {
            Func<SprocValidationResult, string> genError = new Func<SprocValidationResult, string>((sproc) =>
            {
                var nl = Environment.NewLine;
                string except = "";
                if (sproc.Exception != null)
                    except = sproc.Exception.ToString() + nl;
                var db = sproc.Attribute.Database;
                string conn = DataAccessHelper.GetConnectionString(db);
                string nameAndSchema = sproc.Attribute.SprocName;
                if (!nameAndSchema.Contains("."))
                    nameAndSchema = "dbo." + nameAndSchema;
                return conn + nl +
                       db.ToString().ToLower() + "." + nameAndSchema + nl +
                       except + nl;
            });
            List<SprocValidationResult> badSprocs = CheckSprocAccess(assembly);
            if (badSprocs.Count == 0)
                return null;
            var failures = badSprocs.Where(o => o.Result == SprocResult.Failure);
            var errors = badSprocs.Where(o => o.Result == SprocResult.Error);
            string alert = null;
            if (failures.Any())
            {
                alert += "You don't have execute permission for the following stored procedures: \n";
                foreach (SprocValidationResult sproc in failures)
                    alert += genError(sproc);
            }
            if (errors.Any())
            {
                alert += "Encountered an exception verifying the following stored procedures: \n";
                foreach (SprocValidationResult sproc in errors)
                    alert += genError(sproc);
            }
            return alert;
        }
    }
}
