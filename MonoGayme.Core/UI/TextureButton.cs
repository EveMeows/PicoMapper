using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGayme.Core.Components;
using MonoGayme.Core.Utilities;

namespace MonoGayme.Core.UI;

public class TextureButton : Button
{
	private Rectangle _rect;
	private Texture2D _textureNormal;

	private int _scale;

	private bool _holding;
	private bool _ignoreMouse;

	private bool _hasEntered;

	public TextureButton(Texture2D texture, Vector2 position, Color colour, int scale = 1, bool ignoreMouse = false) : base(ignoreMouse)
	{
		_textureNormal = texture;
		_scale = scale;

		_rect = new Rectangle(
			(int)position.X,
			(int)position.Y,

			texture.Width * scale,
			texture.Height * scale
		);

		Colour = colour;
		Position = position;

		_ignoreMouse = ignoreMouse;
	}

	public void SetPosition(Vector2 position)
	{
		Position = position;
		_rect = new Rectangle(
			(int)position.X,
			(int)position.Y,

			_textureNormal.Width * _scale,
			_textureNormal.Height * _scale
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

		batch.Draw(_textureNormal, pos, null, Colour, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
	}
}
