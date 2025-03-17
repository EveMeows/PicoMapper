using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Components;

namespace MonoGayme.Core.Abstractions;

public interface IDrawableComponent
{
	void Draw(SpriteBatch batch, Camera2D? camera = null);
}
