////////////////////////////////////////////////////////////////////////////////
// StickyWindows
//
// Copyright (c) 2009 Riccardo Pietrucci, 2017 Thomas Freudenberg
//
// This software is provided 'as-is', without any express or implied warranty.
// In no event will the author be held liable for any damages arising from
// the use of this software.
// Permission to use, copy, modify, distribute and sell this software for any
// purpose is hereby granted without fee, provided that the above copyright
// notice appear in all copies and that both that copyright notice and this
// permission notice appear in supporting documentation.
//
//////////////////////////////////////////////////////////////////////////////////


using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace StickyWindows {
    public abstract class BaseFormAdapter {
        private static double _scaleX;
        private static double _scaleY;

        private bool      _isExtendedFrameMarginInitialized;
        private Rectangle _extendedFrameMargin;

        protected abstract Rectangle InternalBounds { get; set; }

        public abstract IntPtr Handle               { get; }
        public abstract Size   MaximumSize          { get; set; }
        public abstract Size   MinimumSize          { get; set; }
        public abstract bool   Capture              { get; set; }
        public abstract void   Activate();
        public abstract Point PointToScreen(Point point);

        static BaseFormAdapter()
        {
            InitDisplayScalingFactors();
        }

        private void InitExtendedFrameMargin() {
            if (_isExtendedFrameMarginInitialized) {
                return;
            }

            Win32.RECT rect;
            if (Win32.DwmGetWindowAttribute(Handle, Win32.DWMWINDOWATTRIBUTE.ExtendedFrameBounds, out rect,
                    Marshal.SizeOf(typeof(Win32.RECT))) == 0)
            {
                var originalFormBounds = InternalBounds;
                _extendedFrameMargin = new Rectangle(
                    -(originalFormBounds.Left - rect.Left),
                    -(originalFormBounds.Top - rect.Top),
                    -(originalFormBounds.Width - (rect.Right - rect.Left)),
                    -(originalFormBounds.Height - (rect.Bottom - rect.Top)));
            }
            else
            {
                _extendedFrameMargin = Rectangle.Empty;
            }
            _isExtendedFrameMarginInitialized = true;
        }

        public Rectangle Bounds {
            get {
                InitExtendedFrameMargin();

                var bounds = InternalBounds;
                bounds.X += _extendedFrameMargin.Left;
                bounds.Y += _extendedFrameMargin.Top;
                bounds.Width += _extendedFrameMargin.Width;
                bounds.Height += _extendedFrameMargin.Height;

                return bounds;
            }
            set {
                InitExtendedFrameMargin();

                var bounds = value;
                bounds.X -= _extendedFrameMargin.Left;
                bounds.Y -= _extendedFrameMargin.Top;
                bounds.Width -= _extendedFrameMargin.Width;
                bounds.Height -= _extendedFrameMargin.Height;
                InternalBounds = bounds;
            }
        }

        public double[] BorderThickness {
            get {
                InitExtendedFrameMargin();
                InitDisplayScalingFactors();

                return new []
                {
                      _extendedFrameMargin.Left   / _scaleX,
                      _extendedFrameMargin.Top    / _scaleY,
                    - _extendedFrameMargin.Right  / _scaleX,
                    - _extendedFrameMargin.Bottom / _scaleY
                };
            }
        }

        // TODO - Get DPI settings on the particular monitor on which the form/window is displayed,
        //        rather than just using the general system parameter values.
        private static void InitDisplayScalingFactors()
        {
            if (_scaleX == 0 || _scaleY == 0)
            {
                var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;
                var dpiXProperty = typeof(System.Windows.SystemParameters).GetProperty("DpiX", bindingFlags);
                var dpiYProperty = typeof(System.Windows.SystemParameters).GetProperty("Dpi",  bindingFlags);

                int dpiX = (int) dpiXProperty.GetValue(null, null);
                int dpiY = (int) dpiYProperty.GetValue(null, null);

                // A DPI value of 96 is 100% (1-to-1 scaling).
                _scaleX = dpiX / 96.0;
                _scaleY = dpiY / 96.0;
            }
        }
    }
}
