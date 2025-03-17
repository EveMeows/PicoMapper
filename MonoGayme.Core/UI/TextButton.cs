using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGayme.Core.Components;
using MonoGayme.Core.Utilities;

namespace MonoGayme.Core.UI;

public class TextButton : Button
{
	private Rectangle _rect;
	private readonly SpriteFont _font;
	private string _text;

	private bool _holding;
	private bool _ignoreMouse;

	private bool _hasEntered;

	public TextButton(SpriteFont font, string text, Vector2 position, Color colour, bool ignoreMouse = false) : base(ignoreMouse)
	{
		Vector2 textSize = font.MeasureString(text);
		_rect = new Rectangle(
			(int)position.X,
			(int)position.Y,

			(int)textSize.X,
			(int)textSize.Y
		);

		_font = font;
		_text = text;
		Colour = colour;
		Position = position;

		_ignoreMouse = ignoreMouse;
	}

	public void SetText(string text)
	{
		_text = text;
		Vector2 textSize = _font.MeasureString(_text);
		_rect = new Rectangle(
			(int)Position.X,
			(int)Position.Y,

			(int)textSize.X,
			(int)textSize.Y
		);
	}

	public void SetPosition(Vector2 position)
	{
		Position = position;
		Vector2 textSize = _font.MeasureString(_text);
		_rect = new Rectangle(
			(int)position.X,
			(int)position.Y,

			(int)textSize.X,
			(int)textSize.Y
		);
	}

	public override void Update(Vector2 mouse)
	{
		if (_ignoreMouse) return;

		if (Collision.CheckRectPoint(mouse, _rect))
		{
			if (!_hasEntered)
				OnMouseEnter?.Invoke(this);
			_hasEntered = true;

			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				_holding = true;

			if (Mouse.GetState().LeftButton != ButtonState.Released || !_holding) return;

			RunAction();
			_holding = false;
		}
		else
		{
			OnMouseExit?.Invoke(this);
			_holding = false;
			_hasEntered = false;
		}
	}

	public override void Draw(SpriteBatch batch, Camera2D? camera)
	{
		Vector2 pos = Position;
		if (camera is not null)
			pos = camera.ScreenToWorld(Position);

		batch.DrawString(_font, _text, pos, Colour);
	}
}
