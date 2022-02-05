using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uheaa.Common;

namespace IDRRPTFED
{
    public class RecordDataGrabber
    {
        public List<AbRecordData> AbData { get; private set; } = new List<AbRecordData>();
        public List<BdRecordData> BdData { get; private set; } = new List<BdRecordData>();
        public List<BeRecordData> BeData { get; private set; } = new List<BeRecordData>();
        public List<BfRecordData> BfData { get; private set; } = new List<BfRecordData>();

        public IEnumerable<int> NullAwardIds
        {
            get
            {
                var allData = new List<IAppAndAward>();
                foreach (var property in DataProperties.Keys)
                {
                    var propertyList = property.GetValue(this) as IEnumerable<IAppAndAward>;
                    allData.AddRange(propertyList);
                }
                return allData.Where(o => o.AwardId.IsNullOrEmpty()).Select(o => o.ApplicationId);
            }
        }

        public void PopulateByDate(DataAccess da, DateTime start, DateTime end)
        {
            object[] parameters = new object[] { start, end, null };
            foreach (var kvp in DataProperties)
            {
                var value = kvp.Value.Invoke(da, parameters);
                kvp.Key.SetValue(this, value);
            }
        }

        public void PopulateByAppIds(DataAccess da, IEnumerable<int> appIds)
        {
            foreach (var kvp in DataProperties)
            {
                var propList = kvp.Key.GetValue(this) as IList;
                List<object> values = new List<object>();
                foreach (var appId in appIds)
                {
                    object[] parameters = new object[] { null, null, appId };
                    var vals = kvp.Value.Invoke(da, parameters) as IList;
                    foreach (var val in vals)
                        propList.Add(val);
                }
            }
        }

        #region Static Setup
        /// <summary>
        /// A list of a Data Properties, and their corresponding population methods from DataAccess
        /// </summary>
        private static Dictionary<PropertyInfo, MethodInfo> DataProperties = new Dictionary<PropertyInfo, MethodInfo>();
        static RecordDataGrabber()
        {
            foreach (var property in typeof(RecordDataGrabber).GetProperties())
                if (property.Name.EndsWith("Data") && property.Name.Length == 6) //only pull properties like AbData, BfData, etc.
                {
                    bool found = false;
                    foreach (var method in typeof(DataAccess).GetMethods())
                    {
                        string prefix = property.Name.Substring(0, 2);
                        if (method.Name == "Get" + prefix + "Records")
                        {
                            DataProperties.Add(property, method);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        throw new Exception("Could not find a matching DataAccess method for Property " + property.Name);
                }
        }

        #endregion
    }
}