using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;

namespace MonoGayme.Core.Components.Sprites;

/// <summary>
/// Create a new 2D Sprite component.
/// </summary>
public class Sprite : Component, IDrawableComponent
{
	public bool Flipped;
	public Texture2D Texture;
	public Vector2 Origin;
	public float Scale;
	public Color Tint;
	public float Rotation;

	public Vector2? Position = null;

	public bool ExactPosition { get; set; } = false;

	public Sprite(Texture2D texture, float scale = 1, bool flipped = false, float rotation = 0, Color? tint = null, Vector2? origin = null, string? name = null)
	{
		Flipped = flipped;
		Texture = texture;
		Scale = scale;
		Origin = origin ?? Vector2.Zero;
		Tint = tint ?? Color.White;
		Rotation = rotation;
		Name = name;
	}

	public void SetCustomPosition(Vector2 position)
		=> Position = position;

	public void ResetPosition()
		=> Position = null;

	public void Draw(SpriteBatch batch, Camera2D? camera = null)
	{
		Vector2 pos = Position ?? Parent.Position;
		Vector2 relativePosition = camera?.ScreenToWorld(pos) ?? pos;
		if (ExactPosition) relativePosition.Floor();

		batch.Draw(Texture, relativePosition, null, Tint, Rotation, Origin, Scale, Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
	}
}
