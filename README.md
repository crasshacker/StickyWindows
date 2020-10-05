
# StickyWindows

**Note:** This is a fork of this [StickyWindows GitHub repo](https://github.com/thoemmi/StickyWindows), which I've
retargeted to .NET Core 3.1 and made a number of changes to, including significant (breaking) changes to the API.
Because of the significance of the API changes, I plan to manage this as a separate project rather than making pull
requests of the original StickyWindows project to incorporate these changes.

This library provides a means for a WinForms or WPF application to mark one or more of its application windows as
sticky, so that they stick to other windows (of the same application instance) or to the edge of the screen when
moved sufficiently close.  This is done by associating a StickyWindow instance with each window that should stick
to others (or the screen edge) when close, as well as with each window that should serve as an "anchor" that such
sticky windows can be stuck to.  Various properties on the StickyWindow object can be set to control exactly how
the window behaves with regard to stickiness:

* WindowType        - Determines the overall sticky behavior of this window (see Sticky Window Types below).
* StickGravity      - The distance (in pixels) the window will move to stick to an anchor window or screen edge.
* StickOnMove       - If true, the window should jump to nearby windows or the screen edge when being moved.
* StickOnResize     - If true, the window should jump to nearby windows or the screen edge when being resized.
* StickToScreen     - If true, the window should stick to the edge of the screen when moved close to it.
* StickToOther      - If true, the window should stick to other nearby windows when moved close to them.
* StickToOutside    - If true, the window should stick to the outside edge of another window.
* StickToInside     - If true, the window should stick to the inside edge of another window.
* StickToCorners    - If true, the window should stick to corners of another window.
* ClientAreaMoveKey - Specifies whether Ctrl or Shift keys allow dragging a window's from within its client area.

The default WindowType is StickyWindowType.Sticky, the default StickGravity is 20, and the default ClientAreaMoveKey
is StickyWindow.ModifierKey.None; the default value for all other properties is true.

## Usage

To make a window sticky, associate a StickyWindow instance with the window:

WinForms:

    using StickyWindows;    // requires StickyWindows.DLL

    var stickyWindow = new StickyWindow(myForm, StickyWindowType.Sticky);
    // Set stickyWindow properties appropriately here...

WPF:

    using StickyWindows;        // requires StickyWindows.DLL
    using StickyWindows.WPF;    // requires StickyWindows.WPF.DLL

    var stickyWindow = myWindow.CreateStickyWindow(StickyWindowType.Sticky);
    // Set stickyWindow properties appropriately here...

If you set a window's position programmatically you can call the `Stick()` method after positioning the window to
force the window to behave as if it were moved to that position with the mouse.  In other words, if the window is
grabby/sticky and is positioned close enough to one or more anchor windows, it will attach itself to those windows.

## Sticky Window Types

A StickyWindow's WindowType property (of type StickyWindowType) determines the window's behavior with regard to other
StickyWindows.  The StickyWindowType enumeration values are:

* None     - The window does not grab or stick to other windows, nor does it act as an anchor for others to stick to.
* Anchor   - A window which Grabby and Sticky windows will latch onto when they are moved within range of it.
* Grabby   - A window that attaches to an anchor when moved close to it, but doesn't move with the anchor window.
* Sticky   - A grabby window that, once it grabs an anchor window, remains stuck to it when the anchor window is moved.
* Cohesive - A sticky window that also operates as an anchor, so that other sticky windows will stick to it.

A StickyWindowType of Anchor indicates that the window will attract grabby and sticky windows when they move close
to it.  (The inverse is not true; it won't stick to grabby/sticky windows when it is moved close to them.)  Both
Grabby and Sticky windows will latch onto an Anchor window, but only Sticky windows are carried along with it when
it moves.  (Sticky windows stuck to an Anchor window become unstuck if either of the two windows is resized.)

A Cohesive window acts as both an Anchor and a Sticky window.  When a Cohesive window A is moved close to another
Cohesive window B and becomes stuck to it, window B serves as an anchor, and will carry window A with it when it
is moved.  However, if window A is moved it does not carry window B with it, even though they are both Cohesive;
if it did, there would be no way to detach the windows from one another.  Thus, whether a Cohesive window behaves
as an Anchor or as a Sticky window depends upon whether another window was stuck to it, or whether it was instead
stuck to another window.

