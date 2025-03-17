using Microsoft.Xna.Framework;
using MonoGayme.Core.Abstractions;

namespace MonoGayme.Core.Components;

/// <summary>
///  Create a new Timer.
/// </summary>
public class Timer : Component, IUpdateableComponent
{
	private float _timer;
	private float _time;
	private readonly bool _oneShot;

	public Action? OnTimeOut;

	public Timer(float time, bool enabled, bool oneShot, string? name = null)
	{
		_time = time;
		Enabled = enabled;
		_oneShot = oneShot;

		Name = name;
	}

	public void Update(GameTime gameTime)
	{
		if (!Enabled) return;
		_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

		if (!(_timer >= _time)) return;
		_timer = 0;

		if (_oneShot)
			Enabled = false;

		OnTimeOut?.Invoke();
	}

	public void Stop() => Enabled = false;

	public void Start() => Enabled = true;

	public void Reset() => _timer = 0;

	public bool Enabled { get; set; }

	public float TimeLeft => Math.Max(0, _time - _timer);

	public float Time
	{
		get => _time;
		set
		{
			if (value < 0) return;
			_time = value;
		}
	}
}
