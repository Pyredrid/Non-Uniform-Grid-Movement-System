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
