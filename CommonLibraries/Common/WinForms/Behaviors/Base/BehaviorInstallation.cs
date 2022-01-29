using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class BehaviorInstallation : IEnumerable<Behavior>, IEnumerable
    {
        private List<Behavior> behaviors = new List<Behavior>();
        public void Add(Behavior b)
        {
            behaviors.Add(b);
        }
        /// <summary>
        /// Add and install the given Behavior.
        /// </summary>
        /// <param name="b"></param>
        public void Install(Behavior b)
        {
            behaviors.Add(b);
            Install();
        }
        /// <summary>
        /// Install all behaviors in this installation.
        /// </summary>
        public void Install()
        {
            foreach (Behavior behavior in behaviors)
                behavior.Reinstall();
        }
        public T Get<T>() where T : Behavior
        {
            return (T)behaviors.Where(b => b.GetType() == typeof(T)).Single();
        }

        public BehaviorInstallation() { }
        public BehaviorInstallation(IEnumerable<Behavior> behaviors)
        {
            this.behaviors = new List<Behavior>(behaviors);
        }

        #region IEnumerable<Behavior> Members
        public IEnumerator<Behavior> GetEnumerator()
        {
            return behaviors.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return behaviors.GetEnumerator();
        }
        #endregion
    }
}
