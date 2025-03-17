using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;

namespace MonoGayme.Core.Components.Sprites;

public class SpriteSheet : Component, IDrawableComponent
{
	private readonly Texture2D _sprite;

	private readonly Vector2 _origin;

	private readonly Vector2 _frameSize;
	private Vector2 _frameCount;

	private Rectangle _source;

	private readonly bool _wrap;

	public SpriteSheet(Texture2D sprite, Vector2 frameCount, bool wrap = false, Vector2? origin = null)
	{
		_origin = origin ?? Vector2.Zero;
		_frameCount = frameCount;

		_frameSize = _frameSize = new Vector2(sprite.Width / frameCount.X, sprite.Height / frameCount.Y);
		_source = new Rectangle(0, 0, (int)_frameSize.X, (int)_frameSize.Y);

		_wrap = wrap;


		_sprite = sprite;
	}

	public void Draw(SpriteBatch batch, Camera2D? camera = null)
	{
		batch.Draw(_sprite, camera?.ScreenToWorld(Parent.Position) ?? Parent.Position, _source, Color.White, 0, _origin, 1, SpriteEffects.None, 0);
	}

	/// <summary>
	/// Increment the cell by its Y axis.
	/// </summary>
	/// <param name="increment">How many times should it should be incremented by.</param>
	public void IncrementX(int increment = 1) { }

	/// <summary>
	/// Decrement the cell by its X axis.
	/// </summary>
	/// <param name="decrement">How many times it should be decremented by.</param>
	public void DecrementX(int decrement = 1) { }

	/// <summary>
	/// Increment the cell by its Y axis.
	/// </summary>
	/// <param name="increment">How many times it should be incremented by.</param>
	public void IncrementY(int increment = 1)
	{
		int value = (int)(increment * _frameSize.Y);
		if (_source.Y + value > _frameSize.Y)
		{
			if (_wrap)
			{
				_source.Y = 0;
				return;
			}

			value = (int)_frameSize.Y;
		}

		_source.Y += value;
	}

	/// <summary>
	/// Decrement the cell by it's Y axis.
	/// </summary>
	/// <param name="decrement">How many times it should be decremented by.</param>
	public void DecrementY(int decrement = 1)
	{
		int value = (int)(decrement * _frameSize.Y);
		if (_source.Y - value < 0)
		{
			if (_wrap)
			{
				_source.X = (int)_frameSize.X;
				return;
			}

			value = 0;
		}

		_source.Y -= value;
	}

	public float X => _source.X;
	public float Y => _source.Y;
}
