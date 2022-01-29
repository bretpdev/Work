using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDRUSERPRO
{
    public class ControlHelper
    {
        /// <summary>
        /// Determines if a control would be visible if all parents were also visible
        /// </summary>
        public bool WouldBeVisible(Control control)
        {
            MethodInfo mi = control.GetType().GetMethod("GetState", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mi == null)
                return control.Visible;
            return (bool)(mi.Invoke(control, new object[] { 2 }));
        }

        /// <summary>
        /// Finds all child controls for the current control being searched
        /// </summary>
        public IEnumerable<Control> GetControlHierarchy(Control root)
        {
            var queue = new Queue<Control>();

            queue.Enqueue(root);

            do
            {
                var control = queue.Dequeue();
                yield return control;

                foreach (var child in control.Controls.OfType<Control>())
                    queue.Enqueue(child);

            } while (queue.Count > 0);

        }

        public TemporarilyDisabledEvent TemporarilyDisableEvent(Control control, EventHandler handler, string eventName = null)
        {
            return new TemporarilyDisabledEvent(control, handler, eventName);
        }

        public class TemporarilyDisabledEvent : IDisposable
        {
            private Control control;
            private EventHandler handler;
            private EventInfo eventInfo;
            public TemporarilyDisabledEvent(Control control, EventHandler handler, string eventName = null)
            {
                this.control = control;
                this.handler = handler;
                eventName = eventName ?? handler.Method.Name.Split('_').Last();
                eventInfo = control.GetType().GetEvent(eventName);
                if (eventInfo == null)
                    throw new Exception(string.Format("Could not find given handler attached to event {0} on control {1}", eventName, control.Name));
                eventInfo.RemoveEventHandler(control, handler);
            }
            public void Dispose()
            {
                eventInfo.AddEventHandler(control, handler);
            }
        }

        /// <summary>
        /// Groups a Control and associated Label
        /// </summary>
        public ErrorControlGroup<T> Group<T>(Action<string, Control, Label> setError, T control, Label associatedLabel = null) where T : Control
        {
            return new ErrorControlGroup<T>(setError, control, associatedLabel);
        }
        public class ErrorControlGroup<T> : IDisposable where T : Control
        {
            public T Control { get; set; }
            public Label AssociatedLabel { get; set; }
            Action<string, Control, Label> setError;
            internal ErrorControlGroup(Action<string, Control, Label> setError, T control, Label associatedLabel = null)
            {
                this.setError = setError;
                Control = control;
                AssociatedLabel = associatedLabel;
            }

            public void SetErrorIf(string message, Func<T, bool> predicate)
            {
                if (predicate(Control))
                    setError(message, Control, AssociatedLabel);
            }
            
            public void SetError(string message)
            {
                setError(message, Control, AssociatedLabel);
            }

            public void Dispose()
            {
                //nothing to do here, IDisposable is implemented to support using statements.
            }
        }
    }
}
