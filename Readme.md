# VirtualDesktopWidget

Windows widget/tray icon that allows you to scroll through virtual desktop using
the mouse wheel.

It also opens Task View (the virtual desktop overview) on click.

It can be quit by right clicking and using the quit option.

## Technical details

Directly based on how [VolumeScroller](https://github.com/markbrents/VolumeScroller)
is implemented.

It would be nice to use a library like [VirtualDesktop](https://github.com/Grabacr07/VirtualDesktop)
to manage the desktops, for example:
- Currently the code calls Windows shortcuts to change desktops and showing the
  task view. It'd be better to use the API directly.
- Check the number of desktops to allow updating the tray icon with the current
  desktop index, also permitting wraparound to be implemented by rolling back to
  the first desktop upon scrolling past the last and vice-versa.

However, it currently does not work with Windows 11 22H2 due to changes in the
API. It also does not support .NET 7 (it probably could with a simple tweak to
the project files).
