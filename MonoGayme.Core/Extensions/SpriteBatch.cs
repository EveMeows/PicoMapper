using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGayme.Extensions;

public static partial class Extensions
{
	[Conditional("DEBUG")]
	public static void DebugDrawString(this SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color colour)
		=> batch.DrawString(font, text, position, colour);
}
