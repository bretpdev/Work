using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WebApi;

namespace Uheaa.Common.WebApi
{
    /// <summary>
    /// A strongly-typed wrapper around the Session object
    /// </summary>
    public class UheaaSession
    {
        private HttpSessionStateBase session;
        public void SetSession(HttpSessionState session)
        {
            SetSession(new HttpSessionStateWrapper(session));
        }
        public void SetSession(HttpSessionStateBase session)
        {
            this.session = session;
        }
        Action onWebApiConnectionError = null;
        public void SetOnWebApiConnectionError(Action onWebApiConnectionError)
        {
            this.onWebApiConnectionError = onWebApiConnectionError;
        }

        public WebApiDataAccess WADA
        {
            get
            {
                return new WebApiDataAccess(DataAccessHelper.CurrentMode, null, () =>
                {
                    onWebApiConnectionError?.Invoke();
                });
            }
        }

        public ProcessLogRun PLR
        {
            get
            {
                return Get<ProcessLogRun>("PLR");
            }
            set
            {
                Set("PLR", value);
            }
        }

        protected T Get<T>(string name)
        {
            return (T)session[name];
        }
        protected void Set(string name, object o)
        {
            session[name] = o;
        }
    }
}