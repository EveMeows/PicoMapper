using Microsoft.Xna.Framework;

namespace MonoGayme.Extensions;

public static partial class Extensions
{
	public static void SetWindowSize(this GraphicsDeviceManager graphics, int width, int height)
	{
		graphics.PreferredBackBufferWidth = width;
		graphics.PreferredBackBufferHeight = height;

		graphics.ApplyChanges();
	}

	public static void SetWindowSize(this GraphicsDeviceManager graphics, Vector2 size)
		=> SetWindowSize(graphics, (int)size.X, (int)size.Y);
}
