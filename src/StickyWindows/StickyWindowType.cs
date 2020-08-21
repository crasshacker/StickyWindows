using System;

namespace StickyWindows {
    public enum StickyWindowType
    {
        StickyAnchor,   // A top level window that carries stuck windows with it when it moves.
        Sticky,         // A window that sticks to an anchor (or sticky anchor) but doesn't move with it.
        Anchor          // An anchor window that moves independently of windows stuck to it.
    }
}
