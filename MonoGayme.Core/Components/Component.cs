using MonoGayme.Core.Entities;

namespace MonoGayme.Core.Components;

public abstract class Component
{
	public Entity? Parent { get; internal set; }
	public string? Name { get; set; }
}
