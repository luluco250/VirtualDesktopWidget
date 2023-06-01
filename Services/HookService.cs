using Gma.System.MouseKeyHook;

namespace VirtualDesktopWidget.Services;

sealed class HookService : IDisposable
{
	public event MouseEventHandler? MouseWheel
	{
		add => _keyboardMouseEvents.MouseWheel += value;
		remove => _keyboardMouseEvents.MouseWheel -= value;
	}

	readonly IKeyboardMouseEvents _keyboardMouseEvents = Hook.GlobalEvents();

	public void Dispose()
	{
		_keyboardMouseEvents.Dispose();
	}
}
