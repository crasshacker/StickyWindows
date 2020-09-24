using System;

namespace StickyWindows {
    public enum StickyWindowType
    {
        None,           // The window is not sticky, nor does it act as an anchor.
        Anchor,         // A (normally top level) window that carries stuck windows with it when it moves.
        Grabby,         // A window that, when it approaches an Anchor (or Cohesive) window, grabs onto it.
        Sticky,         // A Grabby window that remains stuck to an Anchor/Cohesive window as it is moved.
        Cohesive        // A Sticky window that's also an anchor window (other sticky windows stick to it).
    }
}
