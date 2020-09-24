using System.Windows;
using StickyWindows;

namespace StickyWindows.WPF {
    /// <summary>
    /// Extension methods for <see cref="Window"/>.
    /// </summary>
    public static class WindowExtensions {
        /// <summary>
        /// Creates a <see cref="StickyWindow"/> object for the given <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The window to create a sticky window for.</param>
        /// <returns>A <see cref="StickyWindow"/> object.</returns>
        public static StickyWindow CreateStickyWindow(this Window window, StickyWindowType windowType
                                                                        = StickyWindowType.Sticky) {
            return new StickyWindow(new WpfFormAdapter(window), windowType);
        }

        /// <summary>
        /// Gets an int[4] array containing the left, top, right, and bottom border thicknesses of the
        /// <see cref="Window"/>, in device-independent pixels.
        /// </summary>
        /// <param name="window">The window in question.</param>
        /// <returns>A four element array of doubles.</returns>
        public static double[] GetBorderThickness(this Window window) {
            return new WpfFormAdapter(window).BorderThickness;
        }
    }
}
