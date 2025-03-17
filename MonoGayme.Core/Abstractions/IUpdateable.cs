using Microsoft.Xna.Framework;

namespace MonoGayme.Core.Abstractions;

public interface IUpdateableComponent
{
	void Update(GameTime time);
}
