
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Controllers;

namespace MonoGayme.Core.Entities;

public abstract class Entity
{
	public readonly int ZIndex;

	public readonly ComponentController Components;

	public Vector2 Position = Vector2.Zero;
	public Vector2 Velocity = Vector2.Zero;

	public Entity(int zIndex = 0)
	{
		ZIndex = zIndex;
		Components = new ComponentController(this);
	}

	public abstract void LoadContent();

	public abstract void Update(GameTime time);
	public abstract void Draw(SpriteBatch batch, GameTime time);

	public void Process(GameTime time)
	{
		Components.Update(time);
		Update(time);
	}

	public void Render(SpriteBatch batch, GameTime time)
	{
		Components.Draw(batch);
		Draw(batch, time);
	}
}
