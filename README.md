
# StickyWindows

**Note:** This is a fork of this [StickyWindows GitHub repo](https://github.com/thoemmi/StickyWindows), which I've
retargeted to .NET Core 3.x and improved in a few ways:

1. Resizing a WPF window in certain ways, for example by dragging the right edge of the window leftward past the
window's left edge very quickly, would cause the code to calculate and then (mis-)use a negative window width,
resulting in an exception being thrown (possibly leading to an application crash).  This issue has been resolved.

2. A new GetBorderThickness extension method for the WPF Window class returns the thickness of the left, top,
right, and bottom window borders, expressed in device-independent pixels.

3. The RegisterExternalForm methods have been removed, and a StickyWindowType parameter has been added to the
StickyWindow constructor.  All windows that serve as either anchors to which sticky windows will stick, or as
sticky windows themselves, are imbued with their stickiness attributes by constructing a StickyWindow instance
of the appropriate type and associating it with the actual form or window.

4. The private static _stickGap field of the StickyWindow class has been replaced by a new instance property named
StickGravity.  This property specifies the stickiness strength, and is expressed as the number of pixels away from
an anchor window a sticky window must be moved in order to have it become stuck to or unstuck from that window.

## Sticky Window Types

A new StickyWindowType enumeration indicates the type of a sticky window, which in turn determines its behavior with
regard to stickiness.  All sticky windows are created by calling the StickyWindow constructor, which accepts a sticky
window type argument whoses default is StickyWindowType.Sticky.  This value makes a window stick to windows marked as
anchors when it is moved sufficiently close to them.

A StickyWindowType of Anchor or StickyAnchor indicates that the window acts as an anchor for windows of the Sticky
type to stick to.  That is, if a sticky window is moved or closed to an anchor window it will be pulled to it and
stuck to its edge.  The difference between Anchor and StickyAnchor windows is that when a StickyAnchor window is
moved, any sticky windows currently stuck to it are moved along with it.  Anchor windows of either type do not grab
onto (i.e., stick to) other windows (even sticky windows) when being moved; a window only gets pulled in to stick
to another window if the former window is Sticky and the latter window is an Anchor or StickyAnchor.

Resizing an anchor window unsticks any windows stuck to it; this implies that windows stuck to a StickyAnchor
window don't move as a result of being stuck to an edge being resized.  Similarly, resizing a sticky window that
is stuck to an anchor window unsticks the window from the anchor.  Windows stuck to a StickyAnchor that are pulled
away from the anchor window become unstuck from that window, and moving the anchor window will no longer affect them.

For further details, see the [README](https://github.com/thoemmi/StickyWindows/blob/develop/README.md) in the
repository from which this repository was forked.

