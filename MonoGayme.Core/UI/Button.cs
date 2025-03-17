using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Components;

namespace MonoGayme.Core.UI;

public abstract class Button(bool ignoreMouse) : IElement
{
	public Action<Button>? OnClick;
	public Action<Button>? OnMouseEnter;
	public Action<Button>? OnMouseExit;

	public Action<Button>? OnHover;

	public Vector2 Position { get; set; }
	public Color Colour { get; set; }

	public abstract void Update(Vector2 mouse);
	public abstract void Draw(SpriteBatch batch, Camera2D? camera);

	public void RunAction()
	{
		if (OnClick is null)
		{
			Console.Error.WriteLine("Button has no OnClick callback! Skipping.");
			return;
		}

		OnClick?.Invoke(this);
	}
}
