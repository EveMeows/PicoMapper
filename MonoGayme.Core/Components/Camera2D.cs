using Microsoft.Xna.Framework;

namespace MonoGayme.Core.Components;

public class Camera2D : Component
{
	private Matrix _transform;
	private Matrix _view;

	private Vector2 _position;
	private Vector2 _origin;

	private float _z;

	public Camera2D(Vector2 pos, float zoom = 1, Vector2? origin = null)
	{
		_position = new Vector2(pos.X, pos.Y);
		_z = zoom;
		_origin = origin ?? Vector2.Zero;

		UpdateMatrices();
	}

	/// <summary>
	/// Updates the camera's matrices
	/// </summary>
	public void UpdateMatrices()
	{
		_view = Matrix.CreateTranslation(-_position.X, -_position.Y, 0);
		Matrix origin = Matrix.CreateTranslation(_origin.X, _origin.Y, 0);
		Matrix scale = Matrix.CreateScale(_z, _z, 1);

		_transform = _view * scale * origin;
	}

	public Vector2 WorldToScreen(Vector2 position)
		=> Vector2.Transform(position, _transform);

	public Vector2 ScreenToWorld(Vector2 position)
		=> Vector2.Transform(position, Matrix.Invert(_transform));

	public Matrix Transform => _transform;

	public float Zoom
	{
		get => _z;
		set
		{
			if (value == _z) return;

			_z = value;
			UpdateMatrices();
		}
	}

	public Vector2 Origin
	{
		get => _origin;
		set
		{
			if (value == _origin) return;

			_origin = value;
			UpdateMatrices();
		}
	}

	public Vector2 Position
	{
		get => _position;
		set
		{
			if (value == _position) return;

			_position = value;
			UpdateMatrices();
		}
	}

	public float X
	{
		get => _position.X;
		set
		{
			if (value == _position.X) return;

			_position.X = value;
			UpdateMatrices();
		}
	}

	public float Y
	{
		get => _position.Y;
		set
		{
			if (value == _position.Y) return;

			_position.Y = value;
			UpdateMatrices();
		}
	}
}
