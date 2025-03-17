namespace MonoGayme.Core.Utilities;

public static class Easings
{
	/// <summary>
	/// Ease out cubic mathematical function
	/// </summary>
	/// <param name="progress">A number from 0 to 1 inclusive, representing the progress of the ease.</param>
	public static float EaseOutCubic(float progress)
		=> 1 - MathF.Pow(1 - progress, 3);
}