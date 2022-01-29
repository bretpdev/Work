using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SessionTester
{
    public class SettingsHelper : INotifyPropertyChanged
    {
        private SettingsValue<int> autoStartSeconds;
        public int AutoStartSeconds { get { return autoStartSeconds; } set { autoStartSeconds.Value = value; } }
        private SettingsValue<int> purgeDays;
        public int PurgeDays { get { return purgeDays; } set { purgeDays.Value = value; } }
        private readonly IEnumerable<ISettingsValue> NormalSettings;


        private SettingsValue<string> cornerStoneUsername;
        public string CornerStoneUsername { get { return cornerStoneUsername; } set { cornerStoneUsername.Value = value; } }
        private SettingsValue<string> cornerStonePassword;
        public string CornerStonePassword { get { return cornerStonePassword; } set { cornerStonePassword.Value = value; } }
        private SettingsValue<string> uheaaUsername;
        public string UheaaUsername { get { return uheaaUsername; } set { uheaaUsername.Value = value; } }
        private SettingsValue<string> uheaaPassword;
        public string UheaaPassword { get { return uheaaPassword; } set { uheaaPassword.Value = value; } }
        private readonly IEnumerable<ISettingsValue> LoginSettings;

        public bool AutoStartEnabled
        {
            get { return AutoStartSeconds >= 0; }
            set { AutoStartSeconds = value ? 15 : -1; }
        }
        public bool PurgeEnabled
        {
            get { return PurgeDays > 0; }
            set { PurgeDays = value ? 30 : 0; }
        }
        public bool PendingNormalChanges { get { return autoStartSeconds.HasPendingChanges || purgeDays.HasPendingChanges; } }
        public bool PendingLoginChanges { get { return cornerStonePassword.HasPendingChanges || cornerStoneUsername.HasPendingChanges || uheaaPassword.HasPendingChanges || uheaaUsername.HasPendingChanges; } }

        private Properties.Settings Settings { get; set; }
        private SettingsHelper()
        {
            Settings = Properties.Settings.Default;

            autoStartSeconds = new SettingsValue<int>(Settings, o => o.AutoStartSeconds, PropertyChangedEvent);
            purgeDays = new SettingsValue<int>(Settings, o => o.PurgeDays, PropertyChangedEvent);
            NormalSettings = new ISettingsValue[] { autoStartSeconds, purgeDays };

            cornerStoneUsername = new SettingsValue<string>(Settings, o => o.CornerStoneUsername, PropertyChangedEvent);
            cornerStonePassword = new SettingsValue<string>(Settings, o => o.CornerStonePassword, PropertyChangedEvent);
            uheaaUsername = new SettingsValue<string>(Settings, o => o.UheaaUsername, PropertyChangedEvent);
            uheaaPassword = new SettingsValue<string>(Settings, o => o.UheaaPassword, PropertyChangedEvent);
            LoginSettings = new ISettingsValue[] { cornerStonePassword, cornerStoneUsername, uheaaPassword, uheaaUsername };
        }

        public void SaveNormalSettings()
        {
            SaveSettings(NormalSettings);
        }

        public void SaveLoginSettings()
        {
            SaveSettings(LoginSettings);
        }

        private void SaveSettings(IEnumerable<ISettingsValue> values)
        {
            foreach (ISettingsValue val in values)
                val.Persist();
            Settings.Save();
        }

        public void RevertPendingChanges()
        {
            foreach (ISettingsValue val in NormalSettings.Concat(LoginSettings))
                val.Revert();
        }

        public static SettingsHelper Instance { get; internal set; }
        static SettingsHelper()
        {
            Instance = new SettingsHelper();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void PropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
                PropertyChanged(this, new PropertyChangedEventArgs("PendingNormalChanges"));
                PropertyChanged(this, new PropertyChangedEventArgs("PendingLoginChanges"));
                PropertyChanged(this, new PropertyChangedEventArgs("AutoStartEnabled"));
                PropertyChanged(this, new PropertyChangedEventArgs("PurgeEnabled"));
                foreach (ISettingsValue val in NormalSettings.Concat(LoginSettings))
                    PropertyChanged(this, new PropertyChangedEventArgs(val.PropertyName));
            }
        }
        #endregion
    }

    internal interface ISettingsValue
    {
        void Persist();
        void Revert();
        string PropertyName { get; }
    }

    internal class SettingsValue<T> : ISettingsValue, INotifyPropertyChanged
    {
        Properties.Settings Settings { get; set; }
        PropertyInfo Property { get; set; }
        public string PropertyName { get { return Property.Name; } }
        private T val;
        public T Value
        {
            get { return val; }
            set
            {
                val = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(Property.Name));
            }
        }

        public T OriginalValue
        {
            get { return (T)Property.GetValue(Settings); }
        }

        public bool HasPendingChanges { get { return !Value.Equals(OriginalValue); } }

        public static implicit operator T(SettingsValue<T> value)
        {
            return value.Value;
        }

        public void Persist()
        {
            Property.SetValue(Settings, Value);
        }

        public void Revert()
        {
            Value = OriginalValue;
        }

        public SettingsValue(Properties.Settings settings, Expression<Func<Properties.Settings, T>> valueProperty, PropertyChangedEventHandler iNotifyHandler)
        {
            this.Settings = settings;

            var property = valueProperty.Body as MemberExpression;
            var convert = valueProperty as UnaryExpression; //some expressions have a conversion step in the middle
            if (convert != null) property = convert.Operand as MemberExpression;
            this.Property = property.Member as PropertyInfo;

            Value = OriginalValue;

            PropertyChanged += iNotifyHandler;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
