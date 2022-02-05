using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SftpCoordinator
{
    /// <summary>
    /// Represents a single step in the entire file-moving process
    /// </summary>
    public abstract class FileProcessStep
    {
        public void SetActivityLogProperty(ActivityLog log, bool? success)
        {
            MemberInfo member = null;
            if (ActivityLogProperty.Body is UnaryExpression)
            {
                var operand = (ActivityLogProperty.Body as UnaryExpression).Operand;
                member = (operand as MemberExpression).Member;
            }
            else
            {
                member = ((MemberExpression)ActivityLogProperty.Body).Member;
            }
            var property = member as PropertyInfo;
            property.SetValue(log, success, null);
        }
        /// <summary>
        /// The bool/bool? property that will be invoked with the results of this step
        /// </summary>
        public abstract Expression<Func<ActivityLog, bool?>> ActivityLogProperty { get; }
        /// <summary>
        /// Process this current step of the file-move procedure.
        /// </summary>
        public abstract FileOpResults Process(FileOpResults previous, ProjectFile projectFile, Project project, RunHistory rh);
    }
}
