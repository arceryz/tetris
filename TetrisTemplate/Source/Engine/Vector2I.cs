using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Int vector class for convenience.
/// Must be a struct to pass by value when giving to functions.
/// </summary>
public struct Vector2I
{
	public static Vector2I Zero { get { return new Vector2I(0, 0); } }

	public int X = 0;
	public int Y = 0;

	public Vector2I(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public static Vector2I operator +(Vector2I a, Vector2I b)
	{
		return new Vector2I(a.X + b.X, a.Y + b.Y);
	}

	public static Vector2I operator -(Vector2I a, Vector2I b)
	{
		return new Vector2I(a.X - b.X, a.Y - b.Y);
	}

	public override string ToString()
	{
		return $"({X}, {Y})";
	}
}
