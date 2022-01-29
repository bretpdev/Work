﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace COMPFAFSA.DataAccess
{
    public partial class DataAccessHelper : IDisposable
    {
        public enum Mode
        {
            Live,
            Dev
        }

        readonly string ConnectionString;
        public DataAccessHelper()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public SqlParameter UserId()
        {
            return Sp("UserId", HttpContext.Current.User.Identity.GetUserId());
        }

        public T ExecuteSingle<T>(string commandName, params SqlParameter[] sqlParameters)
        {
            var results = ExecuteList<T>(commandName, sqlParameters);
            return results.Single();
        }

        public T ExecuteSingleOrDefault<T>(string commandName, params SqlParameter[] sqlParameters)
        {
            var results = ExecuteList<T>(commandName, sqlParameters);
            return results.SingleOrDefault();
        }

        public List<T> ExecuteList<T>(string commandName, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection conn = GetConn())
            {
                using (SqlCommand comm = GetComm(commandName, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        foreach (SqlParameter param in sqlParameters)
                            comm.Parameters.Add(param);
                        List<T> results = new List<T>();
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                    results.Add(Populate<T>(reader)); //primitive type
                                }
                            } while (reader.NextResult());
                        }
                        return results;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        comm.Parameters.Clear();
                    }
                }
            }
        }

        public static T Populate<T>(DbDataReader reader, bool read = false)
        {
            if (read)
                reader.Read();
            if (!reader.HasRows)
                return default(T);
            Type type = typeof(T);
            object value = reader[0] is DBNull ? null : reader[0];
            if (type.GetConstructor(Type.EmptyTypes) == null) //no default constructor
                return (T)value; //primitive types
            if (typeof(T) == typeof(object)) //they just want a base object type, return it
                return (T)value;
            List<string> columns = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
                columns.Add(reader.GetName(i).ToLower());
            T obj = (T)type.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            foreach (PropertyInfo pi in type.GetProperties())
            {
                string dbName = pi.Name.ToLower();
                if (columns.Contains(dbName))
                    try
                    {
                        pi.SetValue(obj, ConvertCustom(reader[dbName], pi.PropertyType), new object[] { });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Couldn't populate field {0} of class {1}.  The value was {2}.  \nOriginal Exception: \n{3}", pi.Name, obj.ToString(), reader[dbName], ex.Message), ex);
                    }
            }
            return obj;
        }

        /// <summary>
        /// Convert specified value to a given type.  Special string parsing includes:
        /// Money with dollar signs converted to decimal ("$5.23" -> 5.23m)
        /// Y/N converted to bool ("y" -> true)
        /// 1/0 converted to bool ("0" -> false)
        /// String dates parsed to dates ("1/1/2015" -> new Date(1, 1, 2015))
        /// </summary>
        private static object ConvertCustom(object value, Type type)
        {
            if (value is DBNull)
                if (type == typeof(DateTime))
                    throw new Exception("Can't convert DBNull to non-nullable DateTime");
                else
                    return null;
            if (Nullable.GetUnderlyingType(type) != null)//working with a nullable)
            {
                if (value is string)
                    if (string.IsNullOrEmpty((string)value))
                        return null;
                type = Nullable.GetUnderlyingType(type);
            }
            if (value is int && type.IsEnum)
                return Enum.ToObject(type, value);
            if (type == typeof(bool))
                if (value is string)
                {
                    //returns 0/1 and Y/N as bools
                    string sval = (string)value;
                    sval = sval.ToLower();
                    if (sval == "y" || sval == "yes" || sval == "1") return true;
                    if (sval == "n" || sval == "no" || sval == "0") return false;
                }
            if (type == typeof(DateTime))
                if (value is string)
                {
                    DateTime parse = DateTime.Now;
                    if (DateTime.TryParse((string)value, out parse))
                        return parse;
                }
            if (type == typeof(decimal))
                if (value is string)
                {
                    string sval = (string)value;
                    sval = sval.Replace("$", "");
                    decimal parse = 0;
                    if (decimal.TryParse(sval, out parse))
                        return parse;
                }

            return Convert.ChangeType(value, type);
        }

        protected SqlParameter Sp(string name, object val)
        {
            return new SqlParameter(name, val);
        }


        private SqlConnection GetConn()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        private SqlCommand GetComm(string commandName, SqlConnection conn)
        {
            return new SqlCommand(commandName, conn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 10 * 60 * 60 }; //10 minutes tieout
        }

        /// <summary>
        /// Takes in a IEnumerable<object> and returns a DataTable with the data set
        /// </summary>
        public DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            DataTable dt = new DataTable();

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }

            return dt;
        }

        public virtual void Dispose()
        {

        }
    }
}