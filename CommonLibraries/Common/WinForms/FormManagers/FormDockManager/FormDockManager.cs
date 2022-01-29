using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class FormDockManager
    {
        public Form Parent { get; internal set; }
        public Form Child { get; internal set; }
        private FormDockLocations? Location { get; set; }
        private int ChildOffset { get; set; }
        public FormDockManager(Form parent, Form child)
        {
            Parent = parent;
            Child = child;
        }
        public void Dock(FormDockLocations location)
        {
            Parent.Move += Parent_Move;
            Child.Move += Child_Move;

            Location = location;
            SyncChild();
        }
        public void Undock()
        {
            Parent.Move -= Parent_Move;
            Child.Move -= Child_Move;

            Location = null;
        }
        void Parent_Move(object sender, EventArgs e)
        {
            SyncChild();
        }
        void Child_Move(object sender, EventArgs e)
        {
            SyncParent();
        }

        bool childSyncing = false;
        /// <summary>
        /// Sync the child to the current location of the parent.
        /// </summary>
        private void SyncChild()
        {
            if (!Location.HasValue) return;
            if (parentSyncing) return;
            childSyncing = true;
            if (Location.Value == FormDockLocations.Left)
            {
                var left = Parent.Left - Child.Width;
                var top = Parent.Top + ChildOffset;
                if (top + Child.Height > Parent.Bottom)
                    top = Parent.Bottom - Child.Height;
                if (top < Parent.Top)
                    top = Parent.Top;
                Child.SetDesktopLocation(left, top);
            }
            childSyncing = false;
        }

        bool parentSyncing = false;
        /// <summary>
        /// Sync the parent to the current location of the child
        /// </summary>
        private void SyncParent()
        {
            if (!Location.HasValue) return;
            if (childSyncing) return;
            parentSyncing = true;
            if (Location.Value == FormDockLocations.Left)
            {
                var left = Child.Right;
                var top = Parent.Top;
                if (Child.Top < top)
                    top = Child.Top;
                if (Child.Top + Child.Height > top + Parent.Height)
                    top = Child.Bottom - Parent.Height;
                if (Parent.Height < Child.Height)
                    top = Child.Top;
                Parent.SetDesktopLocation(left, top);
                ChildOffset = Child.Top - Parent.Top;
            }
            parentSyncing = false;
        }
    }
}
