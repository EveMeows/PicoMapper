using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGayme.Core.States;

/// <summary>
/// Creates a new Game State.
/// </summary>
public abstract class State
{
	public abstract void LoadContent();

	public abstract void Update(GameTime time);
	public abstract void Draw(GameTime time, SpriteBatch batch);
}
