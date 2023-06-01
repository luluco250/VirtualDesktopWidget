using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace VirtualDesktopWidget.Services;

interface IInputBuilder
{
	IInputBuilder Send();
	IInputBuilder Clear();
	IInputBuilder AddKey(Keys key, bool down);
}

sealed class InputService
{
	sealed class InputBuilder : IInputBuilder
	{
		readonly List<INPUT> _inputs = new();

		public IInputBuilder Send()
		{
			if (_inputs.Count == 0) return this;

			var inputsSent = PInvoke.SendInput(
				CollectionsMarshal.AsSpan(_inputs),
				Marshal.SizeOf<INPUT>()
			);

			if (inputsSent != _inputs.Count)
			{
				throw new Win32Exception();
			}

			return this;
		}

		public IInputBuilder Clear()
		{
			_inputs.Clear();
			return this;
		}

		public IInputBuilder AddKey(Keys key, bool down)
		{
			_inputs.Add(new INPUT
			{
				type = INPUT_TYPE.INPUT_KEYBOARD,
				Anonymous =
				{
					ki = new KEYBDINPUT
					{
						dwFlags = down ? default : KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP,
						wVk = (VIRTUAL_KEY)key,
					},
				},
			});

			return this;
		}
	}

	public IInputBuilder Builder()
	{
		return new InputBuilder();
	}
}
