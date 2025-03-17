using Microsoft.Xna.Framework.Input;

namespace MonoGayme.Core.Input;

public class VirtualButton
{
	private readonly List<Keys> _keyboard = [];
	private readonly List<Buttons> _buttons = [];
	private readonly List<MouseButton> _mouseButtons = [];
	public void AddKeyboard(Keys key)
		=> _keyboard.Add(key);

	public void AddKeyboard(params Keys[] keys)
		=> _keyboard.AddRange(keys);

	public void AddGamePad(Buttons button)
		=> _buttons.Add(button);

	public void AddGamePad(params Buttons[] buttons)
		=> _buttons.AddRange(buttons);

	public void AddMouse(MouseButton button)
		=> _mouseButtons.Add(button);

	public void AddMouse(params MouseButton[] buttons)
		=> _mouseButtons.AddRange(buttons);

	public bool IsDown(ButtonType type = ButtonType.All)
	{
		switch (type)
		{
			case ButtonType.Keyboard:
				foreach (Keys key in _keyboard)
					if (InputHelper.IsKeyDown(key)) return true;
				break;

			case ButtonType.GamePad:
				foreach (Buttons button in _buttons)
					if (InputHelper.IsGamePadDown(button)) return true;
				break;

			case ButtonType.Mouse:
				foreach (MouseButton button in _mouseButtons)
					if (InputHelper.IsMouseDown(button)) return true;
				break;

			case ButtonType.All:
				foreach (Keys key in _keyboard)
					if (InputHelper.IsKeyDown(key)) return true;

				foreach (Buttons button in _buttons)
					if (InputHelper.IsGamePadDown(button)) return true;

				foreach (MouseButton button in _mouseButtons)
					if (InputHelper.IsMouseDown(button)) return true;
				break;

			default:
				return false;
		}

		return false;
	}

	public bool IsPressed(ButtonType type = ButtonType.All)
	{
		switch (type)
		{
			case ButtonType.Keyboard:
				foreach (Keys key in _keyboard)
					if (InputHelper.IsKeyPressed(key)) return true;
				break;

			case ButtonType.GamePad:
				foreach (Buttons button in _buttons)
					if (InputHelper.IsGamePadPressed(button)) return true;
				break;

			case ButtonType.Mouse:
				foreach (MouseButton button in _mouseButtons)
					if (InputHelper.IsMousePressed(button)) return true;
				break;

			case ButtonType.All:
				foreach (Keys key in _keyboard)
					if (InputHelper.IsKeyPressed(key)) return true;

				foreach (Buttons button in _buttons)
					if (InputHelper.IsGamePadPressed(button)) return true;


				foreach (MouseButton button in _mouseButtons)
					if (InputHelper.IsMousePressed(button)) return true;
				break;

			default:
				return false;
		}

		return false;
	}
}
