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
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles higher-level movement of a MapObject for
/// behaviours like the Player and NPCs.
/// </summary>
public class CharacterController : Node {
	[Signal]
	public delegate void OnInput(Direction dir, CharacterMovementType movementType);

	[Export]
	private NodePath MapObjectPath = "MapObject";
	[Export]
	private float Speed = 8.0f;
	[Export]
	private float RunMultiplier = 1.5f;

	private MapObject MapObject;

	private Vector3 PreviousTranslation;
	private Vector3 TargetTranslation;
	private Quat PreviousRotation;
	private Quat TargetRotation;
	private float MovementInterpolation = 0.0f;
	
	private float MoveCooldown = -1.0f;
	private Direction CurrentDirection = Direction.Down;

	//Mostly determines what animation to use when moving
	//Should also consider using for special behaviours
	//by check it somehow on certain OnMoved or OnStepped signals
	private CharacterMovementType MovementType = CharacterMovementType.Idle;

	public override void _Ready() {
		MapObject = GetNode<MapObject>(MapObjectPath);

		MapObject.Connect(nameof(MapObject.OnMove), this, nameof(OnMove_HandlePosition));
	}
	
	public override void _Process(float delta) {
		if(MoveCooldown > 0.0f) {
			HandleMoving(delta);
		}
	}
	
	/// <summary>
	/// Interacts with the MapObject in front of 
	/// this CharacterController if there is one.
	/// 
	/// If there are multiple MapObjects in front, 
	/// only interacts with the first one...
	/// </summary>
	//TODO: Some sort of priority system or extra UI for interacting with multiple mapobjects on one node?
	public void InteractWithObjectInFront() {
		MapNode nodeInFront = MapObject.GetCurrentNode().GetAdjacentNode(CurrentDirection);
		if(nodeInFront == null) {
			return;
		}
		MapObject objectInFront = MapManager.GetFirstOccupant(nodeInFront);
		if(objectInFront == null) {
			return;
		}
		objectInFront.InteractWithThis(CurrentDirection, this);
	}

	public MapObject GetMapObject() {
		return MapObject;
	}

	public Direction GetCurrentDirection() {
		return CurrentDirection;
	}

	public CharacterMovementType GetMovementType(){
		return MovementType;
	}

	/// <summary>
	/// Moves this CharacterController in a 
	/// direction with a given animation.
	/// 
	/// If the given input fails, returns <see langword="false"/>.
	/// </summary>
	/// <returns><c>true</c>, if input was accepted, <c>false</c> otherwise.</returns>
	public bool GiveInput(Direction dir, CharacterMovementType movementType) {
		if(dir == Direction.None) {
			return false;
		}
		if(MapManager.IsMovementLocked() == true) {
			return false;
		}
		if(MoveCooldown > 0.0f) {
			return false;
		}
		MovementType = movementType;
		CurrentDirection = dir;
		if(movementType != CharacterMovementType.Idle) {
			if(MapObject.Move(dir) == false) {
				MovementType = CharacterMovementType.Idle;
				return false;
			}
		}
		EmitSignal(nameof(OnInput), dir, movementType);
		return true;
	}

	private void HandleMoving(float delta) {
		float speed = Speed;
		if(MovementType == CharacterMovementType.Run) {
			speed *= RunMultiplier;
		}
		
		//Handle cooldown and interpolation
		MoveCooldown -= (speed * delta);

		if(MoveCooldown <= 0.0f) {
			//Movement is done, instead of interpolating,
			//we'll just set the correct position/rotation
			MapObject.Translation = TargetTranslation;
			MapObject.Rotation = TargetRotation.GetEuler();
			MovementInterpolation = 1.0f;
			//Always Idle after moving
			MovementType = CharacterMovementType.Idle;
		} else {
			MovementInterpolation += (speed * delta);
			
			MapObject.Translation = PreviousTranslation.LinearInterpolate(TargetTranslation, MovementInterpolation);
			MapObject.Rotation = PreviousRotation.Slerp(TargetRotation, MovementInterpolation).GetEuler();
		}
	}

	/// <summary>
	/// Actually starts the process for moving 
	/// the MapObject in the world based on its position.
	/// </summary>
	protected void OnMove_HandlePosition(Direction dir, bool isWarp) {
		PreviousTranslation = MapObject.Translation;
		TargetTranslation = MapObject.GetCurrentNode().Translation;
		
		PreviousRotation = new Quat(MapObject.Rotation);
		TargetRotation = new Quat(MapObject.GetCurrentNode().Rotation);
		
		MovementInterpolation = 0.0f;

		MoveCooldown = 1.0f;
		
		if(isWarp == true) {
			MapObject.Translation = TargetTranslation;
			MapObject.Rotation = TargetRotation.GetEuler();
			MovementInterpolation = 1.0f;
			MovementType = CharacterMovementType.Idle;

			MoveCooldown = 0.0f;
		}
	}
}

//TODO: More movement types such as collision with walls, maybe crawling and jumping?
public enum CharacterMovementType {
	Idle = 0,
	Walk = 1,
	Run = 2,
}
