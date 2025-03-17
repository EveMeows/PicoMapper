using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;

namespace MonoGayme.Core.Components.Sprites;

public class AnimatedSprite : Component, IUpdateableComponent, IDrawableComponent
{
	private readonly Vector2 _origin;
	private readonly Vector2 _frameSize;
	private readonly Texture2D _sprite;

	private readonly Vector2 _frameCount;

	private Rectangle _source;

	private readonly float _speed;
	private float _frameTimer;
	private int _frame;

	public bool Finished = true;
	public readonly bool Loop;

	public Action? OnSheetFinished;

	public bool Flipped = false;

	public AnimatedSprite(Texture2D sprite, Vector2 size, float speed, bool loop = false, Vector2? origin = null)
	{
		_origin = origin ?? Vector2.Zero;

		_frameCount = size;
		_sprite = sprite;

		_speed = speed;

		_frameSize = new Vector2(sprite.Width / size.X, sprite.Height / size.Y);
		_source = new Rectangle(0, 0, (int)_frameSize.X, (int)_frameSize.Y);

		Loop = loop;
	}

	public void Reset()
	{
		_frameTimer = 0;
		_frame = 0;
	}

	public void Update(GameTime time)
	{
		_frameTimer += (float)time.ElapsedGameTime.TotalSeconds;
		if (!(_frameTimer >= _speed)) return;
		_frameTimer = 0;

		_frame++;
		if (_frame >= _frameCount.X)
		{
			_frame = 0;

			if (!Loop)
			{
				Finished = true;
			}

			OnSheetFinished?.Invoke();
		}

		_source.X = (int)(_frame * _frameSize.X);
	}

	public void Draw(SpriteBatch batch, Camera2D? camera = null)
	{
		batch.Draw(_sprite, camera?.ScreenToWorld(Parent.Position) ?? Parent.Position, _source, Color.White, 0f, _origin, 1f, Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
	}

	public int Width => (int)_frameSize.X;
	public int Height => (int)_frameSize.Y;
}