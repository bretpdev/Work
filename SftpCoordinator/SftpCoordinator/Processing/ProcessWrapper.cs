using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SftpCoordinator
{
    public class ProcessWrapper
    {
        public delegate FileOpResults ProcessDelegate(FileOpResults previousResults);
        public ProcessDelegate Process { get; set; }
        public void SetProperty(bool? value)
        {
            var memberSelectorExpression = AffectedProperty.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(memberSelectorExpression, value, null);
                }
            }
        }
        private Expression<Func<bool?>> AffectedProperty { get; set; }
        public ProcessWrapper(ProcessDelegate process, Expression<Func< bool?>> affectedProperty)
        {
            Process = process;
            AffectedProperty = affectedProperty;
        }
    }
}
