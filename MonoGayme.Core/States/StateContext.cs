using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGayme.Core.States;

/// <summary>
/// Helper class used for Game State handling.
/// </summary>
public class StateContext
{
	private State? _activeState;

	public void SwitchState(State state)
	{
		_activeState = state;
		_activeState.LoadContent();
	}

	public void Update(GameTime time) => _activeState?.Update(time);
	public void Draw(GameTime time, SpriteBatch batch) => _activeState?.Draw(time, batch);

	public State? State => _activeState;
}
