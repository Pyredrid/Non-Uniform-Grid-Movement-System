using Godot;
using System;

public enum Direction {
	None = -1,
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3,
}

public static class DirectionUtilities {
	public static Direction GetClosestDirection(Vector3 direction) {
		float dDotF = direction.Dot(Vector3.Forward);
		float dDotB = direction.Dot(Vector3.Back);
		float dDotL = direction.Dot(Vector3.Left);
		float dDotR = direction.Dot(Vector3.Right);

		if(dDotF > dDotB && dDotF > dDotL && dDotF > dDotR) {
			return Direction.Up;
		}
		if(dDotB > dDotF && dDotB > dDotL && dDotB > dDotR) {
			return Direction.Down;
		}
		if(dDotL > dDotF && dDotL > dDotB && dDotL > dDotR) {
			return Direction.Left;
		}
		if(dDotR > dDotF && dDotR > dDotL && dDotR > dDotB) {
			return Direction.Right;
		}
		return Direction.None;
	}
}

public static class DirectionExtensions {
	public static Vector3 Vector3(this Direction dir) {
		switch(dir) {
			case Direction.None:
				return new Vector3(0, 0, 0);
			case Direction.Up:
				return new Vector3(0, 0, -1);
			case Direction.Right:
				return new Vector3(1, 0, 0);
			case Direction.Down:
				return new Vector3(0, 0, 1);
			case Direction.Left:
				return new Vector3(-1, 0, 0);
		}
		return new Vector3(0, 0, 0);
	}
	public static Direction AdjustUp(this Direction dir, Direction newUp) {
		if(newUp == Direction.Up) {
			switch(dir) {
				case Direction.Up:
					return Direction.Up;
				case Direction.Right:
					return Direction.Right;
				case Direction.Down:
					return Direction.Down;
				case Direction.Left:
					return Direction.Left;
			}
		}
		if(newUp == Direction.Right) {
			switch(dir) {
				case Direction.Up:
					return Direction.Left;
				case Direction.Right:
					return Direction.Up;
				case Direction.Down:
					return Direction.Right;
				case Direction.Left:
					return Direction.Down;
			}
		}
		if(newUp == Direction.Down) {
			switch(dir) {
				case Direction.Up:
					return Direction.Down;
				case Direction.Right:
					return Direction.Left;
				case Direction.Down:
					return Direction.Up;
				case Direction.Left:
					return Direction.Right;
			}
		}
		if(newUp == Direction.Left) {
			switch(dir) {
				case Direction.Up:
					return Direction.Right;
				case Direction.Right:
					return Direction.Down;
				case Direction.Down:
					return Direction.Left;
				case Direction.Left:
					return Direction.Up;
			}
		}
		return Direction.None;
	}
	public static Direction Opposite(this Direction dir) {
		switch(dir) {
			case Direction.Up:
				return Direction.Down;
			case Direction.Right:
				return Direction.Left;
			case Direction.Down:
				return Direction.Up;
			case Direction.Left:
				return Direction.Right;
		}
		return Direction.None;
	}
}