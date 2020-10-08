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
/// From NPCs to interactable props, this class handles
/// things on the map that can be used or moved.
/// </summary>
public class MapObject : Spatial {
	/// <summary>
	/// Signal emitted when this MapObject moves
	/// </summary>
	[Signal]
	public delegate void OnMove(Direction dir, bool isWarp);
	/// <summary>
	/// Signal emitted when this MapObject gets 
	/// interacted with.  If you wanna be fancy
	/// and are using something like a GOAP, you
	/// can make AI fire this signal too.
	/// </summary>
	[Signal]
	public delegate void OnInteractedWith(Direction directionFrom, Node interactor);

	/// <summary>
	/// If a MapObject is solid, then `MapManager.IsTraversable()` will
	/// return false for the MapNode it is on.
	/// </summary>
	[Export]
	public bool IsSolid = true;

	private MapNode CurrentNode;

	public override void _EnterTree() {
		base._EnterTree();
		MapManager.RegisterMapObject(this);
	}

	public override void _ExitTree() {
		base._ExitTree();
		MapManager.UnregisterMapObject(this);
	}

	public override void _Process(float delta) {
		base._Process(delta);
		
		//TODO: Add a way to have unconstrained MapObjects for any special events?
		if(CurrentNode == null) {
			CurrentNode = MapManager.GetClosestNode(GlobalTransform.origin);
			if(CurrentNode != null) {
				Warp(CurrentNode);
			}
		}
	}
	
	/// <summary>
	/// Moves this MapObject in the given direction.
	/// Will return false if there was a problem moving.
	/// </summary>
	/// <returns>The move.</returns>
	/// <param name="dir">Dir.</param>
	//TODO: Instead of returning true/false, return an enum describing the move problem?
	public bool Move(Direction dir) {
		if(CurrentNode == null) {
			return false;
		}
		
		MapNode nextNode = CurrentNode.GetAdjacentNode(dir);
		if(nextNode == null) {
			return false;
		}

		if(MapManager.IsTraversable(nextNode) == false) {
			return false;
		}

		CurrentNode.EmitSignal(nameof(MapNode.SteppedOff));
		CurrentNode = nextNode;
		CurrentNode.EmitSignal(nameof(MapNode.SteppedOn));
		EmitSignal(nameof(OnMove), dir, false);
		return true;
	}
	
	/// <summary>
	/// Completely disregards collision or adjacency and
	/// puts this MapObject on the specified Node.
	/// </summary>
	public void Warp(MapNode destination) {
		CurrentNode.EmitSignal(nameof(MapNode.SteppedOff));
		CurrentNode = destination;
		CurrentNode.EmitSignal(nameof(MapNode.SteppedOn));
		EmitSignal(nameof(OnMove), Direction.None, true);
	}

	/// <summary>
	/// Emits the signal for interacting with this MapObject. This function
	/// mostly exists for convenience and readability.
	/// </summary>
	/// <param name="directionFrom">Direction from.</param>
	/// <param name="interactor">Interactor.</param>
	public void InteractWithThis(Direction directionFrom, Node interactor) {
		EmitSignal(nameof(OnInteractedWith), directionFrom, interactor);
	}
	
	//C# properties are a sin, here's a regular getter  :v
	public MapNode GetCurrentNode() {
		return CurrentNode;
	}
}
