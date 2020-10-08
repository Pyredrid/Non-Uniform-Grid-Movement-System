//	Copyright (C) 2020  Aaron "Pyredrid" Bekker-Dulmage
//
// 	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//	GNU General Public License for more details.
//
//	You should have received a copy of the GNU General Public License
//	along with this program.If not, see<https://www.gnu.org/licenses/>.

using Godot;
using System;

/// <summary>
/// An enum to simplify working with the 4
/// cardinal directions.  Direction.None is 
/// to handle edgecases mostly.
/// </summary>
public enum Direction {
	None = -1,
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3,
}

public static class DirectionUtilities {
	/// <summary>
	/// Given a directional Vector3, returns the closest 
	/// Direction enum in worldspace.
	/// </summary>
	/// <param name="direction">A normalized direction</param>
	//TODO: Figure out some of the edgecases this seems to have...
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
	/// <summary>
	/// Returns a directional Vector3 for this Direction using
	/// Godot's coordinate system.
	/// </summary>
	public static Vector3 ToVector3(this Direction dir) {
		switch(dir) {
			case Direction.None:
				return Vector3.Zero;
			case Direction.Up:
				return Vector3.Forward;
			case Direction.Right:
				return Vector3.Right;
			case Direction.Down:
				return Vector3.Back;
			case Direction.Left:
				return Vector3.Left;
		}
		return Vector3.Zero;
	}
	
	/// <summary>
	/// Returns a rotated direction based on the 
	/// new given "Up" direction.  
	/// e.g. Right when facing Up is Right, but Right when facing Down is Left
	/// </summary>
	public static Direction AdjustUp(this Direction dir, Direction newUp) {
		if(newUp == Direction.Up) {
			//Up from Up is Up and same with every other direction  :v
			return dir;
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
	
	/// <summary>
	/// Returns the opposite of this direction.
	/// </summary>
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