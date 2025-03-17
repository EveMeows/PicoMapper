using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;
using MonoGayme.Core.Components;
using MonoGayme.Core.Entities;

namespace MonoGayme.Core.Controllers;

public class ComponentController(Entity? parent = null)
{
	public List<Component> Components { get; } = [];

	public void Update(GameTime time)
	{
		foreach (Component component in Components)
		{
			if (component is IUpdateableComponent updateable)
				updateable.Update(time);
		}
	}

	public void Draw(SpriteBatch batch)
	{
		foreach (Component component in Components)
		{
			if (component is IDrawableComponent drawable)
				drawable.Draw(batch);
		}
	}

	public void Add(Component component)
	{
		component.Parent = parent;
		Components.Add(component);
	}

	/// <summary>
	/// Get the first component with a matching type.
	/// </summary>
	public T? Get<T>() where T : Component
		=> (T?)Components.Find(c => c is T);

	/// <summary>
	/// Get a component based on it's name
	/// </summary>
	public T? Get<T>(string name) where T : Component
		=> (T?)Components.Find(c => c.Name == name);

	/// <summary>
	/// Remove a component.
	/// </summary>
	public void Remove<T>(T component) where T : Component
	   => Components.Remove(component);

	/// <summary>
	/// Remove a component using its name.
	/// </summary>
	public void Remove(string name)
	{
		Component? c = Components.Find(c => c.Name == name);
		if (c is null) return;

		Components.Remove(c);
	}
}
