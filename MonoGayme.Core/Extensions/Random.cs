namespace MonoGayme.Extensions;

public static partial class Extensions
{
	public static float NextSingle(this Random random, float min, float max)
		=> min + random.NextSingle() * (max - min);
}
