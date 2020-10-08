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
/// Takes keyboard input and uses 
/// that to give input to a CharacterController.
/// </summary>
public class PlayerInput : Node {
	[Export]
	private NodePath CharacterControllerPath = "CharacterController";
	private CharacterController CharacterController;
	
	//Loads this map on ready, mostly just for testing right now...
	[Export(PropertyHint.File)]
	private string InitialMap = "";
	
	public override void _Ready() {
		CharacterController = GetNode<CharacterController>(CharacterControllerPath);
		MapManager.LoadMap(InitialMap);
	}

	public override void _Process(float delta) {
		CharacterMovementType movementType = CharacterMovementType.Walk;
		if(Input.IsActionPressed("game_run") == true) {
			movementType = CharacterMovementType.Run;
		}
		if(Input.IsActionPressed("game_up") == true) {
			CharacterController.GiveInput(Direction.Up, movementType);
		} else if(Input.IsActionPressed("game_right") == true) {
			CharacterController.GiveInput(Direction.Right, movementType);
		} else if(Input.IsActionPressed("game_down") == true) {
			CharacterController.GiveInput(Direction.Down, movementType);
		} else if(Input.IsActionPressed("game_left") == true) {
			CharacterController.GiveInput(Direction.Left, movementType);
		}
		if(Input.IsActionJustPressed("game_interact") == true) {
			CharacterController.InteractWithObjectInFront();
		}
	}
}
