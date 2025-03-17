using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Components;

namespace MonoGayme.Core.UI;

public class CheckBox : IElement
{
	public Color Colour { get; set; }

	private readonly string _text;
	private readonly SpriteFont _font;

	private readonly Texture2D _spriteNormal;
	private readonly Texture2D _spriteChecked;

	public Vector2 Position;
	public Vector2 TextPosition;

	public bool Checked;
	private Texture2D _active;

	public Action<bool>? OnCheckChanged;

	public CheckBox(Texture2D normal, Texture2D check, SpriteFont font, string text, Vector2 position, Vector2 textPosition, Color colour, bool isChecked = false)
	{
		_text = text;
		_font = font;

		_spriteNormal = normal;
		_spriteChecked = check;

		Colour = colour;

		Position = position;
		TextPosition = textPosition;
		TextPosition += position;

		_active = normal;

		Checked = isChecked;
		if (Checked)
		{
			_active = _spriteChecked;
			return;
		}

		_active = _spriteNormal;
	}

	public void RunAction()
	{
		Checked = !Checked;
		OnCheckChanged?.Invoke(Checked);

		if (Checked)
		{
			_active = _spriteChecked;
			return;
		}

		_active = _spriteNormal;
	}

	public void Update(Vector2 mouse)
	{
	}

	public void Draw(SpriteBatch batch, Camera2D? camera)
	{
		batch.Draw(_active, Position, Color.White);
		batch.DrawString(_font, _text, TextPosition, Colour);
	}
}