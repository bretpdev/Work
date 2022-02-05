using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uheaa.Common.WebApi
{
    public class QueryStringHelper
    {
        private List<QueryStringPair> queryStringComponents = new List<QueryStringPair>();
        public QueryStringHelper(string fullUrl)
        {
            FullUrl = fullUrl;
        }
        public string FullUrl
        {
            get
            {
                return BaseUrl + QueryString;
            }
            set
            {
                if (!value.Contains("?"))
                    value += "?";
                var split = value.Split('?');
                BaseUrl = split[0];
                QueryString = split.Skip(1).SingleOrDefault();
            }
        }
        public string BaseUrl { get; set; }
        public string QueryString
        {
            get
            {
                if (!queryStringComponents.Any())
                    return "";
                string result = "";
                foreach (var pair in queryStringComponents)
                {
                    result += "&" + pair.Key;
                    if (!string.IsNullOrWhiteSpace(pair.Value))
                        result += "=" + pair.Value;
                }
                result = "?" + result.Substring(1); //remove leading &
                return result;
            }
            set
            {
                queryStringComponents.Clear();
                value = value.Trim('?');
                foreach (string pair in value.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var split = pair.Split('=');
                    queryStringComponents.Add(new QueryStringPair(split[0], split.Skip(1).SingleOrDefault()));
                }
            }
        }
        public void RemoveKey(string key)
        {
            var item = Get(key);
            if (item != null)
                queryStringComponents.Remove(item);
        }
        public void SetValue(string key, string value)
        {
            var item = Get(key);
            if (item == null)
            {
                item = new QueryStringPair(key, value);
                queryStringComponents.Add(item);
            }
            else
                item.Value = value;
        }
        public bool ContainsKey(string key)
        {
            return Get(key) != null;
        }
        public string GetValue(string key)
        {
            return Get(key).Value;
        }

        private QueryStringPair Get(string key)
        {
            return queryStringComponents.SingleOrDefault(o => o.Key.ToLower() == key.ToLower());
        }

        private class QueryStringPair
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public QueryStringPair(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}