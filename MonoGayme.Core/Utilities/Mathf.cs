using Microsoft.Xna.Framework;

namespace MonoGayme.Core.Utilities;

/// <summary>
/// Global class for misc. math methods.
/// </summary>
public static class Mathf
{
	/// <summary>
	/// Moves a float towards another every frame with an increment. 
	/// </summary>
	/// <param name="from">The current float.</param>
	/// <param name="to">The target float.</param>
	/// <param name="delta">The increment for every frame.</param>
	public static float MoveTowards(float from, float to, float delta)
	{
		if (Math.Abs(to - from) <= delta)
			return to;

		return from + Math.Sign(to - from) * delta;
	}

	public static Vector2 MoveTowards(Vector2 from, Vector2 to, float delta)
	{
		if ((to - from).Length() <= delta)
			return to;

		return from + Vector2.Normalize(to - from) * delta;
	}
}
