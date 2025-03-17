using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGayme.Core.Components;
using MonoGayme.Core.Input;
using MonoGayme.Core.UI;

namespace MonoGayme.Core.Controllers;

public class UIController(bool allowNavigation)
{
	private readonly List<IElement> _elements = [];
	private readonly List<IElement> _ignored = [];
	private readonly HashSet<IElement> _toRemove = [];

	private Keys? _kbUp;
	private Keys? _kbDown;
	private Keys? _kbAccept;

	private Buttons? _gpUp;
	private Buttons? _gpDown;
	private Buttons? _gpAccept;

	private int _activeIdx;

	/// <summary>
	/// Ran after the active element is changed.
	/// </summary>
	public EventHandler<UpdatedEventArgs>? OnActiveUpdated;

	/// <summary>
	/// Ran before the active element is changed
	/// </summary>
	public EventHandler<UpdatedEventArgs>? OnActiveUpdating;

	/// <summary>
	/// Set the keyboard UI navigation keys. Should only be called if allowNavigation is true. 
	/// </summary>
	public void SetKeyboardButtons(Keys kbUp, Keys kbDown, Keys kbAccept)
	{
		if (!allowNavigation)
		{
			Console.Error.WriteLine("Keyboard navigation disabled! Skipping.");
			return;
		}

		_kbUp = kbUp;
		_kbDown = kbDown;
		_kbAccept = kbAccept;
	}

	/// <summary>
	/// Set the controller UI navigation keys. Should only be called if allowNavigation is true.
	/// </summary>
	public void SetControllerButtons(Buttons gpUp, Buttons gpDown, Buttons gpAccept)
	{
		if (!allowNavigation)
		{
			Console.Error.WriteLine("Controller navigation disabled! Skipping.");
			return;
		}

		_gpDown = gpDown;
		_gpUp = gpUp;
		_gpAccept = gpAccept;
	}

	public void Add<TElement>(TElement element) where TElement : IElement
	{
		_elements.Add(element);

		if (allowNavigation && _elements.Count == 1)
		{
			OnActiveUpdated?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));
		}
	}

	/// <summary>
	/// Add a UI Element to the UI Controller, these are ignored by the navigator.
	/// </summary>
	public void AddIgnored<TIgnored>(TIgnored element) where TIgnored : IElement
		=> _ignored.Add(element);

	public void QueueRemoveAll()
	{
		foreach (IElement element in _elements)
			_toRemove.Add(element);

		foreach (IElement element in _ignored)
			_toRemove.Add(element);
	}

	public void Update(Vector2 mouse)
	{
		if (_activeIdx > _elements.Count - 1)
			_activeIdx = _elements.Count - 1;

		foreach (IElement element in _elements)
			element.Update(mouse);

		foreach (IElement element in _ignored)
			element.Update(mouse);

		if (_toRemove.Count > 0)
		{
			_elements.RemoveAll(_toRemove.Contains);
			_toRemove.Clear();
		}

		if (!allowNavigation) return;

		if (_gpDown.HasValue && _gpUp.HasValue && _gpAccept.HasValue)
		{
			if (InputHelper.IsGamePadPressed(_gpAccept.Value))
				_elements[_activeIdx].RunAction();

			if (InputHelper.IsGamePadPressed(_gpUp.Value))
			{
				OnActiveUpdating?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));

				_activeIdx--;
				if (_activeIdx < 0)
					_activeIdx = 0;

				OnActiveUpdated?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));
			}

			if (InputHelper.IsGamePadPressed(_gpDown.Value))
			{
				OnActiveUpdating?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));

				_activeIdx++;
				if (_activeIdx > _elements.Count - 1)
					_activeIdx = _elements.Count - 1;

				OnActiveUpdated?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));
			}
		}

		if (_kbDown.HasValue && _kbUp.HasValue && _kbAccept.HasValue)
		{
			if (InputHelper.IsKeyPressed(_kbAccept.Value))
				_elements[_activeIdx].RunAction();

			if (InputHelper.IsKeyPressed(_kbUp.Value))
			{
				OnActiveUpdating?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));

				_activeIdx--;
				if (_activeIdx < 0)
					_activeIdx = 0;

				OnActiveUpdated?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));
			}

			if (InputHelper.IsKeyPressed(_kbDown.Value))
			{
				OnActiveUpdating?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));

				_activeIdx++;
				if (_activeIdx > _elements.Count - 1)
					_activeIdx = _elements.Count - 1;

				OnActiveUpdated?.Invoke(this, new UpdatedEventArgs(_elements[_activeIdx]));
			}
		}
	}

	public void Draw(SpriteBatch batch, Camera2D? camera = null)
	{
		foreach (IElement element in _elements)
			element.Draw(batch, camera);

		foreach (IElement element in _ignored)
			element.Draw(batch, camera);
	}
}