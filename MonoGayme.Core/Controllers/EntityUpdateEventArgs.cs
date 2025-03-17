using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Entities;

namespace MonoGayme.Core.Controllers;

public class EntityUpdateEventArgs(GraphicsDevice device, GameTime time, Entity entity)
{
	public GraphicsDevice Device = device;
	public GameTime GameTime = time;
	public Entity Entity = entity;
}
