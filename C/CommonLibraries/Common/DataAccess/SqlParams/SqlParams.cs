using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Uheaa.Common.DataAccess
{
    public static class SqlParams
    {
        /// <summary>
        /// Generates SqlParameters based on the properties of the given object.
        /// Update parameters will not include [InsertOnly] properties.
        /// </summary>
        public static SqlParameter[] Update(object source)
        {
            return Generate(source, ParameterFilter.Update);
        }
        /// <summary>
        /// Generates SqlParameters based on the properties of the given object.
        /// Insert parameters will not include [UpdateOnly] or [PrimaryKey] properties.
        /// </summary>
        public static SqlParameter[] Insert(object source)
        {
            return Generate(source, ParameterFilter.Insert);
        }
        
        /// <summary>
        /// Genereates SqlParameters based on the properties of the given object.
        /// Delete parameters include only [PrimaryKey] properties.
        /// </summary>
        public static SqlParameter[] Delete(object source)
        {
            return Generate(source, ParameterFilter.Delete);
        }
        
        /// <summary>
        /// Genereates SqlParameters based on the properties of the given object.
        /// Only returns parameters based on [PrimaryKey] properties
        /// </summary>
        public static SqlParameter[] PK(object source)
        {
            return Generate(source, ParameterFilter.PrimaryKeyOnly);
        }

        /// <summary>
        /// Generates SqlParameters based on the properties of the given object.
        /// Only includes properties specified by the fields parameter.
        /// </summary>
        public static SqlParameter[] Specifics<T>(T source, params Expression<Func<T, object>>[] fields)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (var expression in fields)
            {
                var body = (MemberExpression)(expression.Body is UnaryExpression ? (expression.Body as UnaryExpression).Operand : expression.Body);
                var prop = (PropertyInfo)body.Member;
                string dbName = prop.GetAttributeValue<DbNameAttribute, string>(o => o.Name, prop.Name);
                parameters.Add(new SqlParameter(dbName, prop.GetValue(source, null)));
            }
            return parameters.ToArray();
        }

        public static SqlParameter[] Except<T>(T source, params Expression<Func<T, object>>[] fields)
        {
             List<SqlParameter> parameters = new List<SqlParameter>(Generate(source));
            foreach (var expression in fields)
            {
                MemberInfo prop = null;
                if (expression.Body is MemberExpression)
                    prop = ((MemberExpression)expression.Body).Member;
                else
                {
                    var op = ((UnaryExpression)expression.Body).Operand;
                    prop = ((MemberExpression)op).Member;
                }
                string dbName = prop.GetAttributeValue<DbNameAttribute, string>(o => o.Name, prop.Name);
                var param = parameters.Single(o => o.ParameterName == dbName);
                parameters.Remove(param);
            }
            return parameters.ToArray();
        }

        private enum ParameterFilter
        {
            All,
            Update,
            Insert,
            Delete,
            PrimaryKeyOnly
        }
        public static SqlParameter[] Generate(object source)
        {
            return Generate(source, ParameterFilter.All);
        }
        private static SqlParameter[] Generate(object source, ParameterFilter filter = ParameterFilter.All)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            Type type = source.GetType();
            var properties = type.GetProperties();
            bool anonymousType = Attribute.GetCustomAttributes(source.GetType()).Any(o => o is CompilerGeneratedAttribute); //means this is an anonymous type
            foreach (PropertyInfo pi in source.GetType().GetProperties())
            {
                if (pi.HasAttribute<DbIgnoreAttribute>()) continue; //ignore this property
                if (pi.HasAttribute<DbReadOnlyAttribute>()) continue; //read-only, won't be used for parameters
                if (anonymousType || (pi.CanRead && pi.CanWrite && !pi.GetGetMethod().IsStatic))
                {
                    string dbName = pi.GetAttributeValue<DbNameAttribute, string>(o => o.Name, pi.Name);
                    bool updateOnly = pi.HasAttribute<UpdateOnlyAttribute>();
                    bool insertOnly = pi.HasAttribute<InsertOnlyAttribute>();
                    bool primaryKey = pi.HasAttribute<PrimaryKeyAttribute>();
                    bool insertValid = !updateOnly && !primaryKey;
                    bool updateValid = !insertOnly;
                    bool deleteValid = primaryKey;
                    bool valid = (filter == ParameterFilter.All)
                              || (filter == ParameterFilter.Insert && insertValid)
                              || (filter == ParameterFilter.Update && updateValid)
                              || (filter == ParameterFilter.Delete && deleteValid)
                              || (filter == ParameterFilter.PrimaryKeyOnly && primaryKey);
                    if (valid)
                        parameters.Add(new SqlParameter(dbName, pi.GetValue(source, null)));
                }
            }
            return parameters.ToArray();
        }

        public static SqlParameter Single(string parameterName, object parameterValue)
        {
            return new SqlParameter(parameterName, parameterValue);
        }
    }
}
