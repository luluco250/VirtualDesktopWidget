using VirtualDesktopWidget.Services;
using VirtualDesktopWidget.Utils.Extensions;

namespace VirtualDesktopWidget.Views;

sealed class MainAppContext : ApplicationContext
{
	readonly VirtualDesktopService _virtualDesktopService;
	readonly NotifyIcon _notifyIcon = new();
	readonly ContextMenuStrip _contextMenuStrip = new();

	public MainAppContext(
		VirtualDesktopService virtualDesktopService,
		HookService hookService
	)
	{
		_virtualDesktopService = virtualDesktopService;

		_contextMenuStrip.RenderMode = ToolStripRenderMode.System;
		_contextMenuStrip.Items.Add("Quit").Click += (_, _) => ExitThread();

		_notifyIcon.Text = nameof(VirtualDesktopWidget);
		_notifyIcon.ContextMenuStrip = _contextMenuStrip;
		_notifyIcon.Icon = SystemIcons.Application;
		_notifyIcon.MouseClick += OnNotifyIconClick;
		_notifyIcon.Visible = true;

		hookService.MouseWheel += OnMouseWheel;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_contextMenuStrip.Dispose();
			_notifyIcon.Dispose();
		}

		base.Dispose(disposing);
	}

	protected override void ExitThreadCore()
	{
		_notifyIcon.Visible = false;
		base.ExitThreadCore();
	}

	void OnNotifyIconClick(object? sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left) return;

		_virtualDesktopService.Overview();
	}

	void OnMouseWheel(object? sender, MouseEventArgs e)
	{
		if (!_notifyIcon.GetRect().Contains(e.Location)) return;

        if (e.Delta > 0)
		{
			_virtualDesktopService.Previous();
		}
		else
		{
			_virtualDesktopService.Next();
		}
	}
}
