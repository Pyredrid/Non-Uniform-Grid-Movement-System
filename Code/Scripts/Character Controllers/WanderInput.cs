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
/// Gives input to a character controller that randomly
/// walks the character around an area.
/// </summary>
public class WanderInput : Node {
	[Export]
	private NodePath CharacterControllerPath = "CharacterController";
	private CharacterController CharacterController;

	[Export]
	private int WanderWidth = 3;
	[Export]
	private int WanderHeight = 3;
	[Export]
	private float WanderTimeMin = 2.0f;
	[Export]
	private float WanderTimeMax = 4.0f;

	private int WanderX = 0;
	private int WanderY = 0;
	private float WanderCooldown = 0.0f;
	private Random Rand = new Random();

	public override void _Ready() {
		CharacterController = GetNode<CharacterController>(CharacterControllerPath);
		WanderCooldown = Rand.RandfRange(WanderTimeMin, WanderTimeMax);
	}

	public override void _Process(float delta) {
		if(MapManager.IsMovementLocked() == true) {
			return;
		}
		
		WanderCooldown -= delta;
		if(WanderCooldown <= 0.0f) {
			Wander();
			WanderCooldown = Rand.RandfRange(WanderTimeMin, WanderTimeMax);
		}
	}
	
	private bool Wander() {
		Direction[] possibleDirections = {
			Direction.Up,
			Direction.Right,
			Direction.Down,
			Direction.Left,
		};

		if(WanderY > WanderHeight) {
			possibleDirections[0] = Direction.None;
		}
		if(WanderX > WanderWidth) {
			possibleDirections[1] = Direction.None;
		}
		if(WanderY < -WanderHeight) {
			possibleDirections[2] = Direction.None;
		}
		if(WanderX < -WanderWidth) {
			possibleDirections[3] = Direction.None;
		}

		Direction dir = Direction.None;
		dir = Rand.Pick(possibleDirections);
		if(dir == Direction.None) {
			return false;
		}

		if(CharacterController.GiveInput(dir, CharacterMovementType.Walk) == false) {
			return false;
		}

		switch(dir) {
			case Direction.Up:
				WanderY++;
				break;
			case Direction.Right:
				WanderX++;
				break;
			case Direction.Down:
				WanderY--;
				break;
			case Direction.Left:
				WanderX--;
				break;
		}
		
		return true;
	}
}
