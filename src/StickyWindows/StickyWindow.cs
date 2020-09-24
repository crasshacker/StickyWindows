////////////////////////////////////////////////////////////////////////////////
// StickyWindows
// 
// Copyright (c) 2004 Corneliu I. Tusnea, 2017 Thomas Freudenberg
//
// This software is provided 'as-is', without any express or implied warranty.
// In no event will the author be held liable for any damages arising from
// the use of this software.
// Permission to use, copy, modify, distribute and sell this software for any
// purpose is hereby granted without fee, provided that the above copyright
// notice appear in all copies and that both that copyright notice and this
// permission notice appear in supporting documentation.
//
// Notice: Check CodeProject for details about using this class
//
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace StickyWindows {
    /// <summary>
    /// Makes a window sticky, so that other sticky windows can be attached to it.
    /// </summary>
    /// <remarks>
    /// Associating a StickyWindow instance with a window makes that window sticky in some way, with respect to other
    /// StickyWindow-linked windows.  A sticky window can either serve as an anchor for other sticky windows to stick
    /// to, or as a window that will stick to an anchor when it is moved near to it, or as both an anchor and a sticky
    /// window.  When a sticky window is stuck to an anchor window, it remains attached to it when that window is
    /// moved; however, it becomes detached if the anchor window is resized.  A window can also be marked as a grabby
    /// window; such a window grabs onto a nearby anchor windows just as a sticky window would, but does not move when
    /// the anchor window is moved.
    /// </remarks>
    public class StickyWindow : NativeWindow {
        [Flags]
        private enum ResizeDir {
            Top    = 2,
            Bottom = 4,
            Left   = 8,
            Right  = 16
        }

        /// <summary>
        /// This is the set of all registered windows, including both anchors and sticky windows.
        /// </summary>
        private static readonly HashSet<StickyWindow> _stickyWindows = new HashSet<StickyWindow>();

        // Adapter for the window we're associated with
        private readonly BaseFormAdapter _originalForm;

        // Internal Message Processor
        private delegate bool ProcessMessage(ref Message m);
        private ProcessMessage _messageProcessor;

        // Messages processors based on type
        private readonly ProcessMessage  _defaultMessageProcessor;
        private readonly ProcessMessage  _resizeMessageProcessor;
        private readonly ProcessMessage  _moveMessageProcessor;

        // General Stuff
        private Rectangle    _formRect;           // form bounds
        private Rectangle    _formOriginalRect;   // bounds before last operation started
        private StickyWindow _anchor;             // the anchor window we're stuck to, if any

        // Move stuff
        private Point        _formOffsetPoint;    // calculated offset rect to be added
        private Point        _offsetPoint;        // primary offset

        // Resize stuff
        private ResizeDir    _resizeDirection;    // direction(s) of current resize operation
        private Rectangle    _formOffsetRect;     // calculated rect to fix the size

        // Public types

        [Flags]
        public enum ModifierKey
        {
            None    = 0,
            Shift   = 4, // Win32.MK.MK_SHIFT    - DOES NOT WORK WITH WinForms!
            Control = 8  // Win32.MK.MK_CONTROL
        }

        // Public properties

        /// <summary>
        /// The window type, which defines its stickiness behavior, for example whether it serves as an anchor
        /// that other windows will stick to, or itself sticks to other windows, or both.
        /// </summary>
        // public StickyWindowType WindowType { get; set; }

        private StickyWindowType _windowType;

        public StickyWindowType WindowType
        {
            get => _windowType;

            set
            {
                if (value == StickyWindowType.None && _windowType != StickyWindowType.None)
                {
                    Unregister();
                }
                else if (value != StickyWindowType.None && _windowType == StickyWindowType.None)
                {
                    Register();
                }
                _windowType = value;
            }
        }


        /// <summary>
        /// The strength of the "pull", or "stickiness", of the window, expressed as the number of pixels distant
        /// the window's edge must be positioned in order to attach or detach from the edge of another window.
        /// The default value is 20;
        /// </summary>
        public int StickGravity { get; set; } = 20;

        /// <summary>
        /// Allow the form to stick while resizing
        /// Default value = true
        /// </summary>
        public bool StickOnResize { get; set; }

        /// <summary>
        /// Allow the form to stick while moving
        /// Default value = true
        /// </summary>
        public bool StickOnMove { get; set; }

        /// <summary>
        /// Allow sticking to Screen Margins
        /// Default value = true
        /// </summary>
        public bool StickToScreen { get; set; }

        /// <summary>
        /// Allow sticking to other StickWindows
        /// Default value = true
        /// </summary>
        public bool StickToOther { get; set; }

        /// <summary>
        /// Specifies the key(s) that, if pressed/down when the left mouse button is clicked in the client area
        /// of the window, allows the window to be dragged as if the mouse were dragging the window's title bar.
        /// A value of ModifierKey.None disables dragging from the window's client area.
        /// Default value = ModifierKey.None.
        /// </summary>
        public ModifierKey ClientAreaMoveKey { get; set; }

        /// <summary>
        /// Make the form Sticky
        /// </summaryThe Great Reversal: How America Gave Up on Free Market>
        /// <param name="form">Form to be made sticky</param>
        public StickyWindow(Form form)
            : this(new WinFormAdapter(form), StickyWindowType.Sticky) {}

        /// <summary>
        /// Make the form Sticky
        /// </summaryThe Great Reversal: How America Gave Up on Free Market>
        /// <param name="form">Form to be made sticky</param>
        /// <param name="windowType">The type of sticky window</param>
        public StickyWindow(Form form, StickyWindowType windowType)
            : this(new WinFormAdapter(form), windowType) {}

        /// <summary>
        /// Make the form Sticky
        /// </summary>
        /// <param name="form">Form to be made sticky</param>
        public StickyWindow(BaseFormAdapter form)
            : this(form, StickyWindowType.Sticky) {
        }

        /// <summary>
        /// Make the form Sticky
        /// </summary>
        /// <param name="form">Form to be made sticky</param>
        /// <param name="windowType">The type of sticky window</param>
        public StickyWindow(BaseFormAdapter form, StickyWindowType windowType) {
            WindowType = windowType;

            _originalForm = form;

            _formRect       = Rectangle.Empty;
            _formOffsetRect = Rectangle.Empty;

            _formOffsetPoint = Point.Empty;
            _offsetPoint     = Point.Empty;

            StickOnMove         = true;
            StickOnResize       = true;
            StickToScreen       = true;
            StickToOther        = true;
            ClientAreaMoveKey   = ModifierKey.None;

            _defaultMessageProcessor = DefaultMsgProcessor;
            _resizeMessageProcessor  = ResizeMsgProcessor;
            _moveMessageProcessor    = MoveMsgProcessor;
            _messageProcessor        = _defaultMessageProcessor;

            AssignHandle(_originalForm.Handle);
        }


        /// <summary>
        /// If the window is a Sticky (not Anchor) window and is within StickGravity's distance of an Anchor
        /// window, stick the window to the anchor window just as if the window were being moved interactively.
        /// </summary>
        public void Stick()
        {
            if (IsGrabby(WindowType)) {
                _formRect = _originalForm.Bounds;

                if (StickToScreen) {
                    Rectangle bounds = _originalForm.Bounds;
                    Point p = new Point(bounds.Left, bounds.Top);
                    p = _originalForm.PointToScreen(p);
                    var activeScr = Screen.FromPoint(p);
                    Move_Stick(activeScr.WorkingArea, false);
                }

                if (StickToOther) {
                    foreach (var sw in _stickyWindows) {
                        if (IsAnchor(sw.WindowType))
                        {
                            _anchor = Move_Stick(sw._originalForm.Bounds, true) && sw._anchor != this ? sw : null;
                        }
                    }
                }
            }
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void OnHandleChange() {
            if ((int)Handle != 0) {
                _stickyWindows.Add(this);
            } else {
                _stickyWindows.Remove(this);
            }
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m) {
            if (!_messageProcessor(ref m)) {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Processes messages during normal operations (while the form is not resized or moved)
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private bool DefaultMsgProcessor(ref Message m) {
            switch (m.Msg) {
                case Win32.WM.WM_LBUTTONDOWN: {
                    int modifiers = (int) ClientAreaMoveKey;
                    if ((modifiers != 0) && ((int)m.WParam & modifiers) == modifiers) {
                        _originalForm.Activate();
                        Point mousePoint = new Point((short)Win32.Bit.LoWord((int)m.LParam),
                                                     (short)Win32.Bit.HiWord((int)m.LParam));
                        if (Win32.ClientToScreen(_originalForm.Handle, out Win32.POINT point))
                        {
                            mousePoint.X += point.X;
                            mousePoint.Y += point.Y;
                            if (OnClientLeftButtonDown(mousePoint)) {
                                m.Result = IntPtr.Zero;
                                return true;
                            }
                        }
                    }
                    break;
                }

                case Win32.WM.WM_NCLBUTTONDOWN: {
                    _originalForm.Activate();
                    Point mousePoint = new Point((short)Win32.Bit.LoWord((int)m.LParam),
                                                 (short)Win32.Bit.HiWord((int)m.LParam));
                    if (OnNonClientLeftButtonDown((int)m.WParam, mousePoint)) {
                        m.Result = IntPtr.Zero;
                        return true;
                    }
                    break;
                }
            }

            return false;
        }

        /// <summary>
        /// Starts move operation when dragging from window's client area.
        /// </summary>
        /// <param name="iHitTest"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool OnClientLeftButtonDown(Point point) {
            var rParent = _originalForm.Bounds;
            _offsetPoint = point;

            if (StickOnMove) {
                _offsetPoint.Offset(-rParent.Left, -rParent.Top);
                StartMove();
                return true;
            }
            return false; // leave default processing
        }

        /// <summary>
        /// Checks where the click was in the NC area and starts move or resize operation
        /// </summary>
        /// <param name="iHitTest"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool OnNonClientLeftButtonDown(int iHitTest, Point point) {
            var rParent = _originalForm.Bounds;
            _offsetPoint = point;

            switch (iHitTest) {
                case Win32.HT.HTCAPTION: {
                    // request for move
                    if (StickOnMove) {
                        _offsetPoint.Offset(-rParent.Left, -rParent.Top);
                        StartMove();
                        return true;
                    }
                    return false; // leave default processing
                }

                // requests for resize
                case Win32.HT.HTTOPLEFT:
                    return StartResize(ResizeDir.Top | ResizeDir.Left);
                case Win32.HT.HTTOP:
                    return StartResize(ResizeDir.Top);
                case Win32.HT.HTTOPRIGHT:
                    return StartResize(ResizeDir.Top | ResizeDir.Right);
                case Win32.HT.HTRIGHT:
                    return StartResize(ResizeDir.Right);
                case Win32.HT.HTBOTTOMRIGHT:
                    return StartResize(ResizeDir.Bottom | ResizeDir.Right);
                case Win32.HT.HTBOTTOM:
                    return StartResize(ResizeDir.Bottom);
                case Win32.HT.HTBOTTOMLEFT:
                    return StartResize(ResizeDir.Bottom | ResizeDir.Left);
                case Win32.HT.HTLEFT:
                    return StartResize(ResizeDir.Left);
            }

            return false;
        }

        private bool StartResize(ResizeDir resDir) {
            if (StickOnResize) {
                _resizeDirection = resDir;
                _formRect = _originalForm.Bounds;
                _formOriginalRect = _originalForm.Bounds; // save the old bounds

                if (!_originalForm.Capture) // start capturing messages
                {
                    _originalForm.Capture = true;
                }

                _messageProcessor = _resizeMessageProcessor;

                return true; // catch the message
            }
            return false; // leave default processing !
        }

        private bool ResizeMsgProcessor(ref Message m) {
            if (!_originalForm.Capture) {
                Cancel();
                return false;
            }

            switch (m.Msg) {
                case Win32.WM.WM_LBUTTONUP: {
                    // ok, resize finished !!!
                    EndResize();
                    break;
                }
                case Win32.WM.WM_MOUSEMOVE: {
                    Point mousePoint = new Point((short)Win32.Bit.LoWord((int)m.LParam),
                                                 (short)Win32.Bit.HiWord((int)m.LParam));
                    Resize(mousePoint);
                    break;
                }
                case Win32.WM.WM_KEYDOWN: {
                    if ((int)m.WParam == Win32.VK.VK_ESCAPE) {
                        _originalForm.Bounds = _formOriginalRect; // set back old size
                        Cancel();
                    }
                    break;
                }
            }

            return false;
        }

        private void EndResize() {
            Cancel();
        }

        private void Resize(Point p) {
            p = _originalForm.PointToScreen(p);
            var activeScr = Screen.FromPoint(p);
            _formRect = _originalForm.Bounds;

            var iRight = _formRect.Right;
            var iBottom = _formRect.Bottom;

            // no normalize required
            // first strech the window to the new position
            if ((_resizeDirection & ResizeDir.Left) == ResizeDir.Left) {
                _formRect.Width = _formRect.X - p.X + _formRect.Width;
                _formRect.X = iRight - _formRect.Width;
            }
            if ((_resizeDirection & ResizeDir.Right) == ResizeDir.Right) {
                _formRect.Width = p.X - _formRect.Left;
            }

            if ((_resizeDirection & ResizeDir.Top) == ResizeDir.Top) {
                _formRect.Height = _formRect.Height - p.Y + _formRect.Top;
                _formRect.Y = iBottom - _formRect.Height;
            }
            if ((_resizeDirection & ResizeDir.Bottom) == ResizeDir.Bottom) {
                _formRect.Height = p.Y - _formRect.Top;
            }

            // this is the real new position
            // now, try to snap it to different objects (first to screen)

            // CARE !!! We use "Width" and "Height" as Bottom & Right!! (C++ style)
            //formOffsetRect = new Rectangle ( stickGap + 1, stickGap + 1, 0, 0 );
            _formOffsetRect.X = StickGravity + 1;
            _formOffsetRect.Y = StickGravity + 1;
            _formOffsetRect.Height = 0;
            _formOffsetRect.Width = 0;

            if (StickToScreen) {
                Resize_Stick(activeScr.WorkingArea, false);
            }

            if (StickToOther) {
                // now try to stick to other forms
                foreach (var sw in _stickyWindows) {
                    if (sw != this) {
                        Resize_Stick(sw._originalForm.Bounds, true);
                    }
                }
            }

            // Fix (clear) the values that were not updated to stick
            if (_formOffsetRect.X == StickGravity + 1) {
                _formOffsetRect.X = 0;
            }
            if (_formOffsetRect.Width == StickGravity + 1) {
                _formOffsetRect.Width = 0;
            }
            if (_formOffsetRect.Y == StickGravity + 1) {
                _formOffsetRect.Y = 0;
            }
            if (_formOffsetRect.Height == StickGravity + 1) {
                _formOffsetRect.Height = 0;
            }

            // compute the new form size
            if ((_resizeDirection & ResizeDir.Left) == ResizeDir.Left) {
                // left resize requires special handling of X & Width acording to MinSize and MinWindowTrackSize
                var iNewWidth = _formRect.Width + _formOffsetRect.Width + _formOffsetRect.X;

                if (_originalForm.MaximumSize.Width != 0) {
                    iNewWidth = Math.Min(iNewWidth, _originalForm.MaximumSize.Width);
                }

                iNewWidth = Math.Min(iNewWidth, SystemInformation.MaxWindowTrackSize.Width);
                iNewWidth = Math.Max(iNewWidth, _originalForm.MinimumSize.Width);
                iNewWidth = Math.Max(iNewWidth, SystemInformation.MinWindowTrackSize.Width);

                _formRect.X = iRight - iNewWidth;
                _formRect.Width = iNewWidth;
            } else {
                // other resizes
                _formRect.Width += _formOffsetRect.Width + _formOffsetRect.X;
            }

            if ((_resizeDirection & ResizeDir.Top) == ResizeDir.Top) {
                var iNewHeight = _formRect.Height + _formOffsetRect.Height + _formOffsetRect.Y;

                if (_originalForm.MaximumSize.Height != 0) {
                    iNewHeight = Math.Min(iNewHeight, _originalForm.MaximumSize.Height);
                }

                iNewHeight = Math.Min(iNewHeight, SystemInformation.MaxWindowTrackSize.Height);
                iNewHeight = Math.Max(iNewHeight, _originalForm.MinimumSize.Height);
                iNewHeight = Math.Max(iNewHeight, SystemInformation.MinWindowTrackSize.Height);

                _formRect.Y = iBottom - iNewHeight;
                _formRect.Height = iNewHeight;
            } else {
                // all other resizing are fine 
                _formRect.Height += _formOffsetRect.Height + _formOffsetRect.Y;
            }

            // Done !!
            _originalForm.Bounds = _formRect;
        }

        private void Resize_Stick(Rectangle toRect, bool bInsideStick) {
            if (_formRect.Right >= (toRect.Left - StickGravity) && _formRect.Left <= (toRect.Right + StickGravity)) {
                if ((_resizeDirection & ResizeDir.Top) == ResizeDir.Top) {
                    if (Math.Abs(_formRect.Top - toRect.Bottom) <= Math.Abs(_formOffsetRect.Top) && bInsideStick) {
                        _formOffsetRect.Y = _formRect.Top - toRect.Bottom; // snap top to bottom
                    } else if (Math.Abs(_formRect.Top - toRect.Top) <= Math.Abs(_formOffsetRect.Top)) {
                        _formOffsetRect.Y = _formRect.Top - toRect.Top; // snap top to top
                    }
                }

                if ((_resizeDirection & ResizeDir.Bottom) == ResizeDir.Bottom) {
                    if (Math.Abs(_formRect.Bottom - toRect.Top) <= Math.Abs(_formOffsetRect.Bottom) && bInsideStick) {
                        _formOffsetRect.Height = toRect.Top - _formRect.Bottom; // snap Bottom to top
                    } else if (Math.Abs(_formRect.Bottom - toRect.Bottom) <= Math.Abs(_formOffsetRect.Bottom)) {
                        _formOffsetRect.Height = toRect.Bottom - _formRect.Bottom; // snap bottom to bottom
                    }
                }
            }

            if (_formRect.Bottom >= (toRect.Top - StickGravity) && _formRect.Top <= (toRect.Bottom + StickGravity)) {
                if ((_resizeDirection & ResizeDir.Right) == ResizeDir.Right) {
                    if (Math.Abs(_formRect.Right - toRect.Left) <= Math.Abs(_formOffsetRect.Right) && bInsideStick) {
                        _formOffsetRect.Width = toRect.Left - _formRect.Right; // Stick right to left
                    } else if (Math.Abs(_formRect.Right - toRect.Right) <= Math.Abs(_formOffsetRect.Right)) {
                        _formOffsetRect.Width = toRect.Right - _formRect.Right; // Stick right to right
                    }
                }

                if ((_resizeDirection & ResizeDir.Left) == ResizeDir.Left) {
                    if (Math.Abs(_formRect.Left - toRect.Right) <= Math.Abs(_formOffsetRect.Left) && bInsideStick) {
                        _formOffsetRect.X = _formRect.Left - toRect.Right; // Stick left to right
                    } else if (Math.Abs(_formRect.Left - toRect.Left) <= Math.Abs(_formOffsetRect.Left)) {
                        _formOffsetRect.X = _formRect.Left - toRect.Left; // Stick left to left
                    }
                }
            }
        }

        private void StartMove() {
            _formRect = _originalForm.Bounds;
            _formOriginalRect = _originalForm.Bounds; // save original position

            if (!_originalForm.Capture) // start capturing messages
            {
                _originalForm.Capture = true;
            }

            _messageProcessor = _moveMessageProcessor;
        }

        private bool MoveMsgProcessor(ref Message m) {
            // internal message loop
            if (!_originalForm.Capture) {
                Cancel();
                return false;
            }

            switch (m.Msg) {
                case Win32.WM.WM_LBUTTONUP: {
                    // ok, move finished !!!
                    EndMove();
                    break;
                }
                case Win32.WM.WM_MOUSEMOVE: {
                    Point mousePoint = new Point((short)Win32.Bit.LoWord((int)m.LParam),
                                                 (short)Win32.Bit.HiWord((int)m.LParam));
                    Move(mousePoint);
                    break;
                }
                case Win32.WM.WM_KEYDOWN: {
                    if ((int)m.WParam == Win32.VK.VK_ESCAPE) {
                        _originalForm.Bounds = _formOriginalRect; // set back old size
                        Cancel();
                    }
                    break;
                }
            }

            return false;
        }

        private void EndMove() {
            Cancel();
        }

        private void Move(Point p) {
            p = _originalForm.PointToScreen(p);
            var activeScr = Screen.FromPoint(p); // get the screen from the point !!

            if (!activeScr.WorkingArea.Contains(p)) {
                p.X = NormalizeInside(p.X, activeScr.WorkingArea.Left, activeScr.WorkingArea.Right);
                p.Y = NormalizeInside(p.Y, activeScr.WorkingArea.Top, activeScr.WorkingArea.Bottom);
            }

            p.Offset(-_offsetPoint.X, -_offsetPoint.Y);

            // p is the exact location of the frame - so we can play with it
            // to detect the new position acording to different bounds
            _formRect.Location = p; // this is the new positon of the form

            _formOffsetPoint.X = StickGravity + 1; // (more than) maximum gaps
            _formOffsetPoint.Y = StickGravity + 1;

            if (IsAnchor(WindowType)) {
                var distanceX = _formRect.Location.X - _originalForm.Bounds.X;
                var distanceY = _formRect.Location.Y - _originalForm.Bounds.Y;

                foreach (var sw in _stickyWindows) {
                    // Move any sticky windows anchored to this window along with it.
                    if (IsSticky(sw.WindowType) && sw != this && sw._anchor == this)
                    {
                        var bounds = sw._originalForm.Bounds;
                        bounds.X += distanceX;
                        bounds.Y += distanceY;
                        sw._originalForm.Bounds = bounds;
                    }
                }
            }

            if (IsGrabby(WindowType))
            {
                if (StickToScreen) {
                    Move_Stick(activeScr.WorkingArea, false);
                }

                if (StickToOther) {
                    foreach (var sw in _stickyWindows) {
                        if (sw != this && IsAnchor(sw.WindowType))
                        {
                            // Note that we don't set the anchor if the other window already has this window as its
                            // anchor, because the bidirectional linkage will make it impossible to detach either
                            // of the windows from the other (moving either of them away from the other will just
                            // pull the other window along it, leaving them attached to one another).
                            _anchor = Move_Stick(sw._originalForm.Bounds, true) && sw._anchor != this ? sw : null;
                        }
                    }
                }
            }

            if (_formOffsetPoint.X == StickGravity + 1) {
                _formOffsetPoint.X = 0;
            }
            if (_formOffsetPoint.Y == StickGravity + 1) {
                _formOffsetPoint.Y = 0;
            }

            _formRect.Offset(_formOffsetPoint);

            _originalForm.Bounds = _formRect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toRect">Rect to try to snap to</param>
        /// <param name="bInsideStick">Allow snapping on the inside (eg: window to screen)</param>
        private bool Move_Stick(Rectangle toRect, bool bInsideStick) {
            bool stuck = false;

            // compare distance from toRect to formRect
            // and then with the found distances, compare the most closed position
            if (_formRect.Bottom >= (toRect.Top - StickGravity) && _formRect.Top <= (toRect.Bottom + StickGravity)) {
                if (bInsideStick) {
                    if ((Math.Abs(_formRect.Left - toRect.Right) <= Math.Abs(_formOffsetPoint.X))) {
                        // left 2 right
                        _formOffsetPoint.X = toRect.Right - _formRect.Left;
                        stuck = true;
                    }
                    if ((Math.Abs(_formRect.Left + _formRect.Width - toRect.Left) <= Math.Abs(_formOffsetPoint.X))) {
                        // right 2 left
                        _formOffsetPoint.X = toRect.Left - _formRect.Width - _formRect.Left;
                        stuck = true;
                    }
                }

                if (Math.Abs(_formRect.Left - toRect.Left) <= Math.Abs(_formOffsetPoint.X)) {
                    // snap left 2 left
                    _formOffsetPoint.X = toRect.Left - _formRect.Left;
                    stuck = true;
                }
                if (Math.Abs(_formRect.Left + _formRect.Width - toRect.Left - toRect.Width) <= Math.Abs(_formOffsetPoint.X)) {
                    // snap right 2 right
                    _formOffsetPoint.X = toRect.Left + toRect.Width - _formRect.Width - _formRect.Left;
                    stuck = true;
                }
            }
            if (_formRect.Right >= (toRect.Left - StickGravity) && _formRect.Left <= (toRect.Right + StickGravity)) {
                if (bInsideStick) {
                    if (Math.Abs(_formRect.Top - toRect.Bottom) <= Math.Abs(_formOffsetPoint.Y)) {
                        // Stick Top to Bottom
                        _formOffsetPoint.Y = toRect.Bottom - _formRect.Top;
                        stuck = true;
                    }
                    if (Math.Abs(_formRect.Top + _formRect.Height - toRect.Top) <= Math.Abs(_formOffsetPoint.Y)) {
                        // snap Bottom to Top
                        _formOffsetPoint.Y = toRect.Top - _formRect.Height - _formRect.Top;
                        stuck = true;
                    }
                }

                // try to snap top 2 top also
                if (Math.Abs(_formRect.Top - toRect.Top) <= Math.Abs(_formOffsetPoint.Y)) {
                    // top 2 top
                    _formOffsetPoint.Y = toRect.Top - _formRect.Top;
                    stuck = true;
                }
                if (Math.Abs(_formRect.Top + _formRect.Height - toRect.Top - toRect.Height) <= Math.Abs(_formOffsetPoint.Y)) {
                    // bottom 2 bottom
                    _formOffsetPoint.Y = toRect.Top + toRect.Height - _formRect.Height - _formRect.Top;
                    stuck = true;
                }
            }

            return stuck;
        }

        private static int NormalizeInside(int iP1, int iM1, int iM2) {
            if (iP1 <= iM1) {
                return iM1;
            }
            if (iP1 >= iM2) {
                return iM2;
            }
            return iP1;
        }

        private void Cancel() {
            _originalForm.Capture = false;
            _messageProcessor = _defaultMessageProcessor;
        }

        private void Register() {
            _stickyWindows.Add(this);
        }

        private void Unregister() {
            _stickyWindows.Remove(this);
            _anchor = null;
        }

        private static bool IsAnchor(StickyWindowType windowType) => windowType == StickyWindowType.Anchor
                                                                  || windowType == StickyWindowType.Cohesive;

        private static bool IsSticky(StickyWindowType windowType) => windowType == StickyWindowType.Sticky
                                                                  || windowType == StickyWindowType.Cohesive;

        private static bool IsGrabby(StickyWindowType windowType) => windowType == StickyWindowType.Grabby
                                                                  || windowType == StickyWindowType.Sticky
                                                                  || windowType == StickyWindowType.Cohesive;
    }
}
