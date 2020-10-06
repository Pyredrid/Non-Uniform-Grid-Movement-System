using Godot;
using System;

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
		if(WanderCooldown <= 0.0f && Wander() == true) {
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
		Direction dir = Direction.None;

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
