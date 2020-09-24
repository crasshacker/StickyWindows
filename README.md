
# StickyWindows

**Note:** This is a fork of this [StickyWindows GitHub repo](https://github.com/thoemmi/StickyWindows), which I've
retargeted to .NET Core 3.1 and improved in a few ways:

1. Resizing a WPF window in certain ways, for example by dragging the right edge of the window leftward past the
window's left edge very quickly, would cause the code to calculate and then (mis-)use a negative window width,
resulting in an exception being thrown (possibly leading to an application crash).  This issue has been resolved.

2. A StickyWindowType parameter has been added to the StickyWindow constructor.  This value determines the manner
in which the window behaves with regard to stickiness, i.e., whether it sticks to other windows when moved close
to them, whether it continues to stick when such an "anchor" window is moved, and whether the window serves as an
anchor to which other windows will stick.  The WindowType property of a StickyWindow can be changed at any time to
alter the way that window behaves with regard to stickiness.

3. The RegisterExternalForm methods have been removed.  All StickyWindow instances are automatically registered.
To unregister a window so that it no longer participates with other StickyWindows, set its WindowType propery to
StickyWindowType.None.  To reregister the window simply change its WindowType to the desired value.

4. The private static _stickGap field of the StickyWindow class has been replaced by a new public instance property
named StickGravity.  This property specifies the stickiness strength, and is expressed as the number of pixels away
from an anchor window a sticky window must be moved in order to have it become stuck to or unstuck from that window.

5. Sticky windows can be moved by dragging the mouse from within the client area of the window is a specified
modifier key (Control or Shift) is held down when the move is initiated.  By default moving a window by dragging
from within the client area is disabled (the ClientAreaMoveKey property is set to ModifierKey.None).  Set the
ClientAreaMoveKey property to either ModifierKey.Shift or ModifierKey.Control to enable it.

## Sticky Window Types

A new StickyWindowType enumeration indicates the type of a sticky window, which in turn determines its behavior with
regard to stickiness.  Available sticky types are:

* None - The window does not grab or stick to other windows, nor does it act as an anchor for others to stick to.
* Anchor - A window which Grabby and Sticky windows will latch onto when they are moved sufficiently close to it.
* Grabby - A window that attaches to an anchor when moved close to it, but doesn't move when the anchor moves.
* Sticky - A grabby window that, once it grabs an anchor, continues to stick to the anchor when it is moved.
* Cohesive - A sticky window that also operates as an anchor, so that other sticky windows will stick to it.

A StickyWindowType of Anchor indicates that the window will attract grabby and sticky windows when they move close
to it.  (The inverse is not true; it won't stick to grabby/sticky windows when moved close to them.)  Both Grabby
and Sticky windows will latch onto an Anchor window, but only Sticky windows are carried along with it when it moves.
(Sticky windows stuck to an Anchor window become unstuck if either of the two windows is resised.)

For details on the original StickyWindows project, see the
[README](https://github.com/thoemmi/StickyWindows/blob/develop/README.md)
in the repository from which this repository was forked.

