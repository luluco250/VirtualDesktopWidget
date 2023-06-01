using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell;

namespace VirtualDesktopWidget.Utils.Extensions;

static class NotifyIconExtensions
{
	static readonly Func<NotifyIcon, NativeWindow> _getNotifyIconNativeWindow =
		ReflectionUtils
			.FieldGetter<NotifyIcon, NativeWindow>(
				fieldName: "_window", // .NET >= 7
				bindingFlags: BindingFlags.Instance | BindingFlags.NonPublic
			)
		??
		ReflectionUtils
			.FieldGetter<NotifyIcon, NativeWindow>(
				fieldName: "window", // .NET < 7
				bindingFlags: BindingFlags.Instance | BindingFlags.NonPublic
			)
		??
		throw new Exception("Unable to get window field from NotifyIcon");

	static readonly Func<NotifyIcon, uint> _getNotifyIconId =
		ReflectionUtils
			.FieldGetter<NotifyIcon, uint>(
				fieldName: "_id", // .NET >= 7
				bindingFlags: BindingFlags.Instance | BindingFlags.NonPublic
			)
		??
		ReflectionUtils
			.FieldGetter<NotifyIcon, uint>(
				fieldName: "id", // .NET < 7
				bindingFlags: BindingFlags.Instance | BindingFlags.NonPublic
			)
		??
		throw new Exception("Unable to get id field from NotifyIcon");

	public static NativeWindow GetNativeWindow(this NotifyIcon notifyIcon)
	{
		return _getNotifyIconNativeWindow(notifyIcon);
	}

	public static uint GetId(this NotifyIcon notifyIcon)
	{
		return _getNotifyIconId(notifyIcon);
	}

	public static Rectangle GetRect(this NotifyIcon notifyIcon)
	{
		var nativeWindow = notifyIcon.GetNativeWindow();
		var id = notifyIcon.GetId();
		var identifier = new NOTIFYICONIDENTIFIER
		{
			cbSize = (uint)Marshal.SizeOf<NOTIFYICONIDENTIFIER>(),
			hWnd = (HWND)nativeWindow.Handle,
			guidItem = Guid.Empty,
			uID = id,
		};
		var result = PInvoke.Shell_NotifyIconGetRect(in identifier, out var rect);

		if (result.Failed)
		{
			throw new Win32Exception();
		}

		return rect;
	}
}
