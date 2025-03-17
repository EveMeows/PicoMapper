using Microsoft.Xna.Framework;

namespace MonoGayme.Core.Utilities;

public static class Collision
{
	public static bool CheckRectPoint(Vector2 point, Rectangle rect)
		=> point.X >= rect.X && point.X <= rect.X + rect.Width && point.Y >= rect.Y && point.Y <= rect.Y + rect.Height;

	public static bool CheckRects(Rectangle first, Rectangle second)
		=> first.X + first.Width >= second.X && first.X <= second.X + second.Width && first.Y + first.Height >= second.Y && first.Y <= second.Y + second.Height;
}
