using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Components;
using MonoGayme.Core.Entities;

namespace MonoGayme.Core.Controllers;

public class EntityController : Component
{
	public List<Entity> Entities { get; } = [];
	public EventHandler<EntityUpdateEventArgs>? OnEntityUpdate;

	private bool _sort;
	private HashSet<Entity> _toRemove = [];

	/// <summary>
	/// Add an entity to the controller, and begin sorting by ZIndex.
	/// </summary>
	public void Add<T>(T entity) where T : Entity
	{
		entity.LoadContent();
		Entities.Add(entity);

		_sort = true;
	}

	/// <summary>
	/// Get the first entity with a matching type.
	/// </summary>
	public T? GetFirst<T>() where T : Entity
		=> (T?)Entities.Find(e => e is T);

	/// <summary>
	/// Queue entity for removal the next frame.
	/// </summary>
	public void QueueRemove<T>(T entity) where T : Entity
	{
		_toRemove.Add(entity);
	}

	/// <summary>
	/// Queue all entities for removal the next frame.
	/// </summary>
	public void QueueRemoveAll()
	{
		_toRemove = [.. Entities];
	}

	/// <summary>
	/// Updates each entity, then removes any queried entities. 
	/// </summary>
	public void Update(GraphicsDevice device, GameTime gameTime)
	{
		if (_sort)
		{
			Entities.Sort((e1, e2) => e1.ZIndex.CompareTo(e2.ZIndex));
			_sort = false;
		}

		foreach (Entity entity in Entities)
		{
			entity.Process(gameTime);
			OnEntityUpdate?.Invoke(this, new EntityUpdateEventArgs(device, gameTime, entity));
		}

		if (_toRemove.Count > 0)
		{
			Entities.RemoveAll(_toRemove.Contains);
			_toRemove.Clear();
		}
	}

	/// <summary>
	/// Draw each entity to the screen. 
	/// </summary>
	public void Draw(SpriteBatch batch, GameTime gameTime)
	{
		foreach (Entity entity in Entities)
		{
			entity.Render(batch, gameTime);
		}
	}
}
