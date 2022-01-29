using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public abstract class Behavior : IDisposable
    {
        /// <summary>
        /// Called when a behavior is initialized.
        /// Behaviors should NOT react or execute code until they have been initialized
        /// </summary>
        public abstract void Install();
        /// <summary>
        /// Called when a behavior is disabled or disposed.
        /// </summary>
        public abstract void Uninstall();
        /// <summary>
        /// Should uninstall the behavior (if applicable), and then reinstall it.
        /// By default, this method simply called Uninstall() and then Install()
        /// </summary>
        public virtual void Reinstall()
        {
            Uninstall();
            Install();
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.GetType().Name == obj.GetType().Name;
        }
        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode();
        }
        public abstract string FriendlyName { get; }

        public void Dispose()
        {
            Uninstall();
        }
    }
}
