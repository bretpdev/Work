using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// When attached to a form, allows custom grow/shrink animations for displaying/hiding the form.
    /// </summary>
    public class FormPopoverManager
    {
        public Form Form { get; internal set; }
        public FormState State { get; internal set; }
        public FormAnchor Anchor { get; internal set; }

        public Point AnchorPoint { get; internal set; }
        Size MaxSize { get; set; }
        Size MinSize { get; set; }
        /// <summary>
        /// The form should never extend past these bounds (ensuring the form always overlaps its parent in some way)
        /// </summary>
        Rectangle ExtendedBounds { get; set; }
        public FormPopoverManager(Form form)
        {
            Anchor = FormAnchor.None;
            Form = form;
            RegisterGrowShrinkEvents();
            RegisterMouseEvents();
        }

        #region Grow/Shrink
        void RegisterGrowShrinkEvents()
        {
            Form.VisibleChanged += Form_VisibleChanged;
            Form.FormClosing += Form_FormClosing;
        }

        void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = Form.DialogResult;
            if (State != FormState.Shrinking)
            {
                e.Cancel = true;
                SlideBack(() =>
                    Shrink(() =>
                          {
                              Form.DialogResult = dr; //will close the form without resetting to DialogResult.Cancel
                              //RemoveCloseButton introduces the unintended side-effect of sometimes causing the
                              //owner window to jump to the background when the child window has close.
                              //the following line seems to fix the issue
                              if (Form.Owner != null) Form.Owner.Activate();
                          }
                    )
                 );
            }
        }

        void Form_VisibleChanged(object sender, EventArgs e)
        {
            if (Form.Visible)
            {
                Grow(() => Focus());
            }
            else
            {
                Shrink();
            }
        }

        public bool Focus()
        {
            var firstTab = Form.Controls.Cast<Control>().OrderBy(c => c.TabIndex).FirstOrDefault();
            if (firstTab != null)
                return firstTab.Focus();
            return Form.Focus();
        }

        const int expandFrames = 15; //expand/contract fully in 15 steps.
        const int expandMilliseconds = 40; //expand/contract fully within 40 milliseconds

        Timer GrowShrinkTimer { get; set; }
        protected void Grow(Action endingAction = null)
        {
            State = FormState.Growing;
            GrowShrink(MaxSize.Width, MaxSize.Height, () =>
            {
                endingAction.SafeExecute();
                State = FormState.Normal;
                Focus();
            });
        }
        public void Shrink()
        {
            Shrink(() =>
            {
                try
                {
                    Form.Hide();
                }
                catch (InvalidOperationException)
                {
                    //Form is disposed at this point.  Don't worry about it
                }
            });
        }
        protected void Shrink(Action endingAction)
        {
            State = FormState.Shrinking;
            GrowShrink(MinSize.Width, MinSize.Height, () => endingAction.SafeExecute());
        }
        private void GrowShrink(int endingWidth, int endingHeight, Action endingAction = null)
        {
            decimal width = Form.Width;
            decimal height = Form.Height;
            int steps = expandFrames;
            decimal incW = (endingWidth - Form.Width) / (decimal)steps;
            decimal incH = (endingHeight - Form.Height) / (decimal)steps;
            if (GrowShrinkTimer != null)
            {
                GrowShrinkTimer.Stop();
                GrowShrinkTimer.Dispose();
            }
            GrowShrinkTimer = new Timer();
            GrowShrinkTimer.Interval = expandMilliseconds / steps;
            GrowShrinkTimer.Tick += (ob, ea2) =>
            {
                width += incW;
                height += incH;
                AnchorForm((int)width, (int)height);
                steps--;
                if (steps == 0)
                {
                    GrowShrinkTimer.Stop();
                    if (endingAction != null)
                        endingAction();
                }
            };
            GrowShrinkTimer.Start();
        }
        /// <summary>
        /// Slide the form back into its anchor position
        /// </summary>
        private void SlideBack(Action endingAction = null)
        {
            State = FormState.Sliding;
            decimal slideAmount = 60;
            decimal slideX = slideAmount;
            decimal slideY = slideAmount;
            Point diff = Point.Empty;
            Point target = Point.Empty;
            Action calcDiff = new Action(() =>
            {
                target = CalculatePosition(Form.Width, Form.Height);
                diff = new Point(Form.Location.X - target.X, Form.Location.Y - target.Y);
            });
            calcDiff();
            if (diff.X > 0)
            {
                if (diff.Y > 0)
                    Anchor = FormAnchor.TopLeft;
                else if (diff.Y < 0)
                    Anchor = FormAnchor.BottomLeft;
            }
            else if (diff.X < 0)
            {
                if (diff.Y > 0)
                    Anchor = FormAnchor.TopRight;
                else if (diff.Y < 0)
                    Anchor = FormAnchor.BottomRight;
            }
            //recalculate target in case anchor changed it.
            calcDiff();
            decimal left = diff.X;
            decimal top = diff.Y;
            if (Math.Abs(left) > Math.Abs(top))
                slideY *= left == 0 ? 0 : (top / left);
            else
                slideX *= top == 0 ? 0 : (left / top);
            if (left < 0 && slideX > 0)
                slideX = -slideX;
            else if (left > 0 && slideX < 0)
                slideX = -slideX;
            if (top < 0 && slideY > 0)
                slideY = -slideY;
            else if (top > 0 && slideY < 0)
                slideY = -slideY;


            Timer slideTimer = new Timer();
            slideTimer.Interval = 1;
            slideTimer.Tick += (ob, ea2) =>
            {
                bool xMore = (Math.Abs(Form.Left - target.X) > Math.Abs(slideX * 2));
                bool yMore = (Math.Abs(Form.Top - target.Y) > Math.Abs(slideY * 2));
                if (xMore)
                    left -= slideX;
                if (yMore)
                    top -= slideY;
                Form.SetDesktopBounds((int)(target.X + left), (int)(target.Y + top), Form.Width, Form.Height);

                if (!xMore && !yMore)
                {
                    slideTimer.Stop();
                    State = FormState.Normal;
                    endingAction.SafeExecute();
                }
            };
            slideTimer.Start();
        }
        #endregion

        #region Form Movement
        private Type[] moveTypes = new Type[] { typeof(Label), typeof(GroupBox), typeof(TableLayoutPanel), typeof(FlowLayoutPanel), typeof(Panel), typeof(Form) };
        private void RegisterMouseEvents()
        {
            var controls = Form.Controls.Cast<Control>().Recurse(c => c.Controls.Cast<Control>());
            controls = controls.Append(Form);
            foreach (Control c in controls)
            {
                Type type = c.GetType();
                while (type != null)
                {
                    if (moveTypes.Contains(type))
                    {
                        c.MouseDown += Form_MouseDown;
                        c.MouseUp += Form_MouseUp;
                        c.MouseMove += Form_MouseMove;
                        break;
                    }
                    type = type.BaseType;
                }
            }
        }
        Point? mouseDownPoint = null;
        Rectangle? mouseDownBounds = null;
        void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownPoint != null && State == FormState.Normal)
            {
                Point cursor = Cursor.Position;
                int deltaX = cursor.X - mouseDownPoint.Value.X;
                int deltaY = cursor.Y - mouseDownPoint.Value.Y;
                int left = mouseDownBounds.Value.Left + deltaX;
                if (left < ExtendedBounds.Left)
                    left = ExtendedBounds.Left;
                if (left > ExtendedBounds.Right - Form.Width)
                    left = ExtendedBounds.Right - Form.Width;
                int top = mouseDownBounds.Value.Top + deltaY;
                if (top < ExtendedBounds.Top)
                    top = ExtendedBounds.Top;
                if (top > ExtendedBounds.Bottom - Form.Height)
                    top = ExtendedBounds.Bottom - Form.Height;
                Form.DesktopBounds = new Rectangle(left, top, Form.Width, Form.Height);
            }
        }

        void Form_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownPoint = null;
            mouseDownBounds = null;
        }

        void Form_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = Cursor.Position;
            mouseDownBounds = Form.DesktopBounds;
        }
        #endregion

        /// <summary>
        /// Displays the form at the mouse location.  Does not allow for form movement.
        /// </summary>
        public bool? ShowPopoverDialog(Form owner)
        {
            Form.Owner = owner;
            AnchorPoint = Cursor.Position;
            int midX = owner.Left + (owner.Width / 2);
            int midY = owner.Top + (owner.Height / 2);
            if (AnchorPoint.X <= midX)
            {
                if (AnchorPoint.Y <= midY)
                    Anchor = FormAnchor.TopLeft;
                else
                    Anchor = FormAnchor.BottomLeft;
            }
            else
            {
                if (AnchorPoint.Y <= midY)
                    Anchor = FormAnchor.TopRight;
                else
                    Anchor = FormAnchor.BottomRight;
            }
            FixedMode();
            int margin = 30;
            int width = MaxSize.Width - margin;
            int height = MaxSize.Height - margin;
            ExtendedBounds = new Rectangle(owner.DesktopBounds.Left - width, owner.DesktopBounds.Top - height, owner.DesktopBounds.Width + width * 2, owner.DesktopBounds.Height + height * 2);

            AnchorForm();
            var result = Form.ShowDialog(owner);
            if (result == DialogResult.OK)
                return true;
            if (result == DialogResult.Cancel)
                return false;
            return null;
        }

        public void ManualPopover(Point p, FormAnchor anchorStyle)
        {
            Anchor = anchorStyle;
            FixedMode();
            AnchorPoint = p;
            AnchorForm();
            Form.BeginInvoke(new Action(() => { Form.Show(); Form.Activate(); }));
        }

        protected void AnchorForm()
        {
            AnchorForm(Form.Width, Form.Height);
        }
        protected void AnchorForm(int width, int height)
        {
            Form.Bounds = new Rectangle(CalculatePosition(width, height), new Size(width, height));
        }

        protected Point CalculatePosition(int width, int height)
        {
            width = Math.Max(width, MinSize.Width);
            height = Math.Max(height, MinSize.Height);
            switch (Anchor)
            {
                case FormAnchor.Top:
                case FormAnchor.Left:
                case FormAnchor.TopLeft:
                    return AnchorPoint;
                case FormAnchor.Right:
                case FormAnchor.TopRight:
                    return new Point(AnchorPoint.X - width, AnchorPoint.Y);
                case FormAnchor.Bottom:
                case FormAnchor.BottomRight:
                    return new Point(AnchorPoint.X - width, AnchorPoint.Y - height);
                case FormAnchor.BottomLeft:
                    return new Point(AnchorPoint.X, AnchorPoint.Y - height);
            }
            return Form.Location;
        }

        protected void FixedMode()
        {
            Form.StartPosition = FormStartPosition.Manual;
            var controls = Form.Controls.Cast<Control>().Recurse(c => c.Controls.Cast<Control>());
            int maxHeight = controls.Max(c => c.Bottom) + 16;
            int maxWidth = controls.Max(c => c.Right) + 16;
            if (MaxSize == Size.Empty)
                MaxSize = new Size(maxWidth, maxHeight);
            Form.AutoSize = false;
            if (!Anchor.InFlag(FormAnchor.StaticHeight))
                Form.Height = 0;
            if (!Anchor.InFlag(FormAnchor.StaticWidth))
                Form.Width = 0;
            //we set these to 0, but they won't go that low.
            int minWidth = Form.Width;
            int minHeight = Form.Height;
            MinSize = new Size(minWidth, minHeight);
            Form.ShowInTaskbar = false;
            RemoveCloseButton();
            if (Anchor.InFlag(FormAnchor.StaticWidth))
                Form.Width = minWidth;
            if (Anchor.InFlag(FormAnchor.StaticHeight))
                Form.Height = minHeight;
        }



        delegate int WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        IntPtr oldWndProc;
        WndProc newProc;
        protected void RemoveCloseButton()
        {
            if (newProc == null) //only do this once
            {
                //todo: figure out why this code wipes out topmost
                bool topMost = Form.TopMost;

                //convert to border only
                SetWindowLong(Form.Handle, GWL_STYLE, WS_THICKFRAME);
                //fix graphical glitches from the above statement
                SetWindowPos(Form.Handle, IntPtr.Zero, 0, 0, 100, 50, SetWindowPosFlags.FrameChanged);

                newProc = new FormPopoverManager.WndProc(MyWndProc);
                oldWndProc = SetWindowLong(Form.Handle, GWL_WNDPROC, newProc);

                Form.TopMost = topMost;
            }
        }

        protected int MyWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            const int WM_NCHITTEST = 0x0084;
            if (msg == WM_NCHITTEST)//don't allow resizing
            {
                switch (DefWindowProc(hWnd, msg, wParam, lParam))
                {
                    case HTBOTTOM:
                    case HTBOTTOMLEFT:
                    case HTBOTTOMRIGHT:
                    case HTLEFT:
                    case HTRIGHT:
                    case HTTOP:
                    case HTTOPLEFT:
                    case HTTOPRIGHT:
                        return HTBORDER;
                }
            }
            return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);
        }

        private const int HTBORDER = 18;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        [DllImport("user32.dll")]
        static extern int DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        private const int GWL_STYLE = -16;
        const int GWL_WNDPROC = -4;
        const int WS_THICKFRAME = 0x00040000; //thick resizable border, no title bar
        const int GWL_HWNDPARENT = -8;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, WndProc newProc);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
        [DllImport("user32")]
        private static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        enum ShowWindowCommands : int
        {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            Hide = 0,
            /// <summary>
            /// Activates and displays a window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when displaying the window 
            /// for the first time.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 2,
            /// <summary>
            /// Maximizes the specified window.
            /// </summary>
            Maximize = 3, // is this the right value?
            /// <summary>
            /// Activates the window and displays it as a maximized window.
            /// </summary>       
            ShowMaximized = 3,
            /// <summary>
            /// Displays a window in its most recent size and position. This value 
            /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
            /// the window is not activated.
            /// </summary>
            ShowNoActivate = 4,
            /// <summary>
            /// Activates the window and displays it in its current size and position. 
            /// </summary>
            Show = 5,
            /// <summary>
            /// Minimizes the specified window and activates the next top-level 
            /// window in the Z order.
            /// </summary>
            Minimize = 6,
            /// <summary>
            /// Displays the window as a minimized window. This value is similar to
            /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowMinNoActive = 7,
            /// <summary>
            /// Displays the window in its current size and position. This value is 
            /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowNA = 8,
            /// <summary>
            /// Activates and displays the window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position. 
            /// An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,
            /// <summary>
            /// Sets the show state based on the SW_* value specified in the 
            /// STARTUPINFO structure passed to the CreateProcess function by the 
            /// program that started the application.
            /// </summary>
            ShowDefault = 10,
            /// <summary>
            ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
            /// that owns the window is not responding. This flag should only be 
            /// used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 11
        }
        [Flags()]
        private enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsynchronousWindowPosition = 0x4000,
            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,
            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,
            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,
            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,
            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,
            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
            /// contents of the client area are saved and copied back into the client area after the window is sized or 
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,
            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,
            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,
            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,
            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,
            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,
            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,
            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,
            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }

    }
}
