using System.Windows;
// using System.Windows.Interactivity;
using Microsoft.Xaml.Behaviors;

namespace StickyWindows.WPF {
    public class StickyWindowBehavior : Behavior<Window> {
        private static readonly DependencyProperty StickyWindowProperty = DependencyProperty.Register(
            "StickyWindow", typeof(StickyWindow), typeof(StickyWindowBehavior), new PropertyMetadata(default(StickyWindow)));

        public static readonly DependencyProperty WindowTypeProperty = DependencyProperty.Register(
            "WindowType", typeof(StickyWindowType), typeof(StickyWindowBehavior), new PropertyMetadata(default(StickyWindowType), OnWindowTypeChanged));

        public static readonly DependencyProperty ClientAreaMoveKeyProperty = DependencyProperty.Register(
            "ClientAreaMoveKey", typeof(StickyWindow.ModifierKey), typeof(StickyWindowBehavior), new PropertyMetadata(default(StickyWindow.ModifierKey), OnClientAreaMoveKeyChanged));

        public static readonly DependencyProperty StickGravityProperty = DependencyProperty.Register(
            "StickGravity", typeof(int), typeof(StickyWindowBehavior), new PropertyMetadata(20, OnStickGravityChanged));

        public static readonly DependencyProperty StickToScreenProperty = DependencyProperty.Register(
            "StickToScreen", typeof(bool), typeof(StickyWindowBehavior), new PropertyMetadata(true, OnStickToScreenChanged));

        public static readonly DependencyProperty StickToOtherProperty = DependencyProperty.Register(
            "StickToOther", typeof(bool), typeof(StickyWindowBehavior), new PropertyMetadata(true, OnStickToOtherChanged));

        public static readonly DependencyProperty StickOnResizeProperty = DependencyProperty.Register(
            "StickOnResize", typeof(bool), typeof(StickyWindowBehavior), new PropertyMetadata(true, OnStickOnResizeChanged));

        public static readonly DependencyProperty StickOnMoveProperty = DependencyProperty.Register(
            "StickOnMove", typeof(bool), typeof(StickyWindowBehavior), new PropertyMetadata(true, OnStickOnMoveChanged));

        private StickyWindow StickyWindow {
            get => (StickyWindow)GetValue(StickyWindowProperty);
            set => SetValue(StickyWindowProperty, value);
        }

        public StickyWindowType WindowType {
            get => (StickyWindowType)GetValue(WindowTypeProperty);
            set => SetValue(WindowTypeProperty, value);
        }

        public StickyWindow.ModifierKey ClientAreaMoveKey {
            get => (StickyWindow.ModifierKey)GetValue(ClientAreaMoveKeyProperty);
            set => SetValue(ClientAreaMoveKeyProperty, value);
        }

        public int StickGravity {
            get => (int)GetValue(StickGravityProperty);
            set => SetValue(StickGravityProperty, value);
        }

        public bool StickToScreen {
            get => (bool)GetValue(StickToScreenProperty);
            set => SetValue(StickToScreenProperty, value);
        }

        public bool StickToOther {
            get => (bool)GetValue(StickToOtherProperty);
            set => SetValue(StickToOtherProperty, value);
        }

        public bool StickOnResize {
            get => (bool)GetValue(StickOnResizeProperty);
            set => SetValue(StickOnResizeProperty, value);
        }

        public bool StickOnMove {
            get => (bool)GetValue(StickOnMoveProperty);
            set => SetValue(StickOnMoveProperty, value);
        }

        private static void OnWindowTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var stickyWindow = (StickyWindow)d.GetValue(StickyWindowProperty);
            if (stickyWindow != null) {
                stickyWindow.WindowType = (StickyWindowType)e.NewValue;
            }
        }

        private static void OnClientAreaMoveKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var stickyWindow = (StickyWindow)d.GetValue(StickyWindowProperty);
            if (stickyWindow != null) {
                stickyWindow.ClientAreaMoveKey = (StickyWindow.ModifierKey)e.NewValue;
            }
        }

        private static void OnStickGravityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var stickyWindow = (StickyWindow)d.GetValue(StickyWindowProperty);
            if (stickyWindow != null) {
                stickyWindow.StickGravity = (int)e.NewValue;
            }
        }

        private static void OnStickToScreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var stickyWindow = (StickyWindow)d.GetValue(StickyWindowProperty);
            if (stickyWindow != null) {
                stickyWindow.StickToScreen = (bool)e.NewValue;
            }
        }

        private static void OnStickToOtherChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var stickyWindow = (StickyWindow)d.GetValue(StickyWindowProperty);
            if (stickyWindow != null) {
                stickyWindow.StickToOther = (bool)e.NewValue;
            }
        }

        private static void OnStickOnResizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var stickyWindow = (StickyWindow)d.GetValue(StickyWindowProperty);
            if (stickyWindow != null) {
                stickyWindow.StickOnResize = (bool)e.NewValue;
            }
        }

        private static void OnStickOnMoveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var stickyWindow = (StickyWindow)d.GetValue(StickyWindowProperty);
            if (stickyWindow != null) {
                stickyWindow.StickOnMove = (bool)e.NewValue;
            }
        }

        protected override void OnAttached() {
            StickyWindow?.ReleaseHandle();

            base.OnAttached();

            if (AssociatedObject.IsLoaded) {
                CreateStickyWindow();
            } else {
                AssociatedObject.Loaded += OnLoaded;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs args) {
            AssociatedObject.Loaded -= OnLoaded;
            CreateStickyWindow();
        }

        private void CreateStickyWindow() {
            var stickyWindow = AssociatedObject.CreateStickyWindow();
            stickyWindow.WindowType = WindowType;
            stickyWindow.ClientAreaMoveKey = ClientAreaMoveKey;
            stickyWindow.StickGravity = StickGravity;
            stickyWindow.StickToScreen = StickToScreen;
            stickyWindow.StickToOther = StickToOther;
            stickyWindow.StickOnResize = StickOnResize;
            stickyWindow.StickOnMove = StickOnMove;
            StickyWindow = stickyWindow;
        }

        protected override void OnDetaching() {
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
            StickyWindow?.ReleaseHandle();
            StickyWindow = null;
        }
    }
}
