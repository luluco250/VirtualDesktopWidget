namespace VirtualDesktopWidget.Services;

sealed class VirtualDesktopService
{
	readonly InputService _inputService;

	public VirtualDesktopService(
		InputService inputService
	)
	{
		_inputService = inputService;
	}

	public void Previous()
	{
		_inputService
			.Builder()
			.AddKey(Keys.LControlKey, true)
			.AddKey(Keys.LWin, true)
			.AddKey(Keys.Left, true)
			.AddKey(Keys.Left, false)
			.AddKey(Keys.LWin, false)
			.AddKey(Keys.LControlKey, false)
			.Send();
	}

	public void Next()
	{
		_inputService
			.Builder()
			.AddKey(Keys.LControlKey, true)
			.AddKey(Keys.LWin, true)
			.AddKey(Keys.Right, true)
			.AddKey(Keys.Right, false)
			.AddKey(Keys.LWin, false)
			.AddKey(Keys.LControlKey, false)
			.Send();
	}

	public void Overview()
	{
		_inputService
			.Builder()
			.AddKey(Keys.LWin, true)
			.AddKey(Keys.Tab, true)
			.AddKey(Keys.Tab, false)
			.AddKey(Keys.LWin, false)
			.Send();
	}
}
