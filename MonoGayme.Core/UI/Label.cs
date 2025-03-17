using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Components;

namespace MonoGayme.Core.UI;

public class Label(string text, Color colour, SpriteFont font, Vector2 position) : IElement
{
	public Color Colour { get; set; } = colour;
	public string Text { get; set; } = text;
	public Vector2 Position = position;

	public void RunAction() { }
	public void Update(Vector2 mouse) { }

	public void Draw(SpriteBatch batch, Camera2D? camera)
		=> batch.DrawString(font, Text, Position, Colour);
}