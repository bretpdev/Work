using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.IO;
using System.Threading;
using Uheaa.Common.DataAccess;
using System.Threading.Tasks;
using System.Dynamic;
using System.Collections;


namespace SERF_File_Generator
{
    public delegate void RecordFormatter<T>(T obj, int seqNum = 0);
    public interface IRecordTypeInfo
    {
        void PopulateLists(object o);
    }
    public class RecordTypeInfo<T> : IRecordTypeInfo
    {
        public string ClassName { get; set; }
        public RecordTypeInfo()
        {
            ClassName = typeof(T).Name;
            foreach (PropertyInfo pi in typeof(T).GetProperties().Where(p => p.Name.StartsWith("ListOf")))
            {
                ListMethod lm = new ListMethod();
                lm.ListProperty = pi;
                lm.GenericArg = pi.PropertyType.GetGenericArguments()[0];
                MethodInfo executeList = typeof(DataAccessHelper).GetMethods().Where(m => m.Name == "ExecuteList" && m.GetParameters().Count() == 3).First()
                                         .MakeGenericMethod(lm.GenericArg);
                lm.ExecuteListMethod = executeList;

                lm.ListProcName = "Get" + pi.Name.Substring("ListOf".Length);

                lm.Occurs = pi.GetCustomAttribute<OccursAttribute>().Times;

                lm.IdField = ClassName + "Id";

                ListMethods.Add(lm);
            }
        }
        public List<ListMethod> ListMethods = new List<ListMethod>();
        public void PopulateLists(object record)
        {
            foreach (ListMethod lm in ListMethods)
                lm.ListProperty.SetValue(record, lm.GetList((T)record));
        }

        public class ListMethod
        {
            public Type GenericArg { get; set; }
            public PropertyInfo ListProperty { get; set; }
            public MethodInfo ExecuteListMethod { get; set; }
            public string ListProcName { get; set; }
            public int Occurs { get; set; }
            public string IdField { get; set; }
            public IList GetList(T record)
            {
                var list = (IList)ExecuteListMethod.Invoke(null, new object[] { 
                    ListProcName, DataAccessHelper.Database.AlignImport,
                    new SqlParameter[]{ new SqlParameter(IdField, typeof(T).GetProperty(IdField).GetValue(record)) }
                });
                if (list.Count < Occurs)
                {
                    var blank = Activator.CreateInstance(GenericArg);
                    for (int i = list.Count; i < Occurs; i++)
                        list.Add(blank);
                }
                return list;
            }
        }
    }
    public static class RecordPopulator
    {
        private static Dictionary<Type, IRecordTypeInfo> cachedInfo = new Dictionary<Type, IRecordTypeInfo>();
        public static IEnumerable<string> PopulateRecord<T>(string ssn, string recordType,string owner, RecordFormatter<T> formatter = null) where T : SerfFileBase
        {
            string className = typeof(T).Name;
            List<T> data = DataAccessHelper.ExecuteList<T>("Get" + className, DataAccessHelper.Database.AlignImport , new SqlParameter("SSN", ssn));

            if (!cachedInfo.ContainsKey(typeof(T)))
                lock (cachedInfo)
                    cachedInfo[typeof(T)] = new RecordTypeInfo<T>();
            IRecordTypeInfo info = cachedInfo[typeof(T)];
            for (int index = 0; index < data.Count; index++)
            {
                T record = data[index];
                info.PopulateLists(record);
                if (formatter != null)
                    formatter(record, (index + 1));
                if (!LoanHelper.ObjectIsValid(ssn, record))
                    throw new Exception("Found a record tied to an invalid loan!");

                string header = GetHeaderInformation(ssn, recordType, (index + 1).ToString().PadLeft(4, '0'), owner);
                yield return RecordCompiler.CompileRecord(header, record);
            }
        }

        public static string GetHeaderInformation(string ssn, string recordType, string seqNumber, string owner)
        {
            
            string year = DateTime.Now.Year.ToString();
            string julDay = DateTime.Now.DayOfYear < 100 ? "0" + DateTime.Now.DayOfYear : DateTime.Now.DayOfYear.ToString();
           
            string seq = "01";

            if(!System.Text.RegularExpressions.Regex.IsMatch(ssn,@"^(?!219-09-9999|078-05-1120)(?!666|000|9\d{2})\d{3}(?!00)\d{2}(?!0{4})\d{4}$"))
            {
                Console.WriteLine(ssn);
            }

            string output = string.Format("B{0}{1}{2}{6}00000{3}{4}{5}", year, julDay, seq, ssn, recordType, seqNumber, owner);
            return output;
        }
    }
}
