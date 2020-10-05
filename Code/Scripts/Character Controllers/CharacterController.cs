using Godot;
using Godot.Collections;
using System;

public class CharacterController : Node {
	[Export]
	private NodePath MapObjectPath = "MapObject";
	[Export]
	private NodePath AnimationPlayerPath = "AnimationPlayer";
	[Export]
	private NodePath Sprite3DPath = "Sprite3D";
	[Export]
	private float Speed = 8.0f;
	[Export]
	private float RunMultiplier = 1.5f;
	[Export]
	public bool IgnoreFacingCamera = false;

	private MapObject MapObject;
	private AnimationPlayer AnimationPlayer;
	//TODO: Decouple the character controller code from the graphics code?
	private Sprite3D Sprite3D;

	private Vector3 PreviousTranslation;
	private Vector3 TargetTranslation;
	private Quat PreviousRotation;
	private Quat TargetRotation;
	private float MovementInterpolation = 0.0f;
	
	private float MoveCooldown = -1.0f;
	private Direction CurrentDirection = Direction.Down;
	private Direction FacingDirection = Direction.Down;

	//TODO:  Make this an enum for different types of movement
	//Like walking, running, sprinting, crawling, swimming, sneaking...
	//TODO: Also a colliding animation
	public CharacterMovementType MovementType = CharacterMovementType.Idle;

	public override void _Ready() {
		MapObject = GetNode<MapObject>(MapObjectPath);
		AnimationPlayer = GetNode<AnimationPlayer>(AnimationPlayerPath);
		Sprite3D = GetNode<Sprite3D>(Sprite3DPath);

		MapObject.Connect(nameof(MapObject.OnMove), this, nameof(OnMove_HandlePosition));
	}
	
	public override void _Process(float delta) {
		UpdateAnimation();
		if(MoveCooldown > 0.0f) {
			HandleMoving(delta);
		}
		if(MapObject.GetCurrentNode() != null) {
			HandleCameraFacingDirection(GetViewport().GetCamera());
		}
	}
	
	public Direction GetCurrentDirection() {
		return CurrentDirection;
	}

	public bool GiveInput(Direction dir, CharacterMovementType movementType) {
		if(dir == Direction.None) {
			return false;
		}
		if(MoveCooldown > 0.0f) {
			return false;
		}
		MovementType = movementType;
		CurrentDirection = dir;
		MapObject.Move(dir);
		return true;
	}
	
	private void HandleCameraFacingDirection(Camera camera) {
		if(IgnoreFacingCamera == true) {
			FacingDirection = CurrentDirection;
			return;
		}
		Vector3 cameraForward = -camera.GlobalTransform.basis.z;
		Vector3 currentNodeForward = -MapObject.GetCurrentNode().GlobalTransform.basis.z;
		Direction cameraDirection = DirectionUtilities.GetClosestDirection(cameraForward);
		Direction currentNodeDirection = DirectionUtilities.GetClosestDirection(currentNodeForward);
		FacingDirection = CurrentDirection.AdjustUp(cameraDirection.AdjustUp(currentNodeDirection));
	}

	private void UpdateAnimation() {
		AnimationPlayer.Play(MovementType.ToString() + "_" + FacingDirection.ToString());
	}

	private void HandleMoving(float delta) {
		float speed = Speed;
		if(MovementType == CharacterMovementType.Run) {
			speed *= RunMultiplier;
		}
		//Handle cooldown and interpolation
		MoveCooldown -= (speed * delta);

		if(MoveCooldown <= 0.0f) {
			MapObject.Translation = TargetTranslation;
			MapObject.Rotation = TargetRotation.GetEuler();
			MovementInterpolation = 1.0f;
			MovementType = CharacterMovementType.Idle;
		} else {
			MovementInterpolation += (speed * delta);
			
			MapObject.Translation = PreviousTranslation.LinearInterpolate(TargetTranslation, MovementInterpolation);
			MapObject.Rotation = PreviousRotation.Slerp(TargetRotation, MovementInterpolation).GetEuler();
		}
	}

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
			
			MoveCooldown = 0.0f;
		}
	}
}

public enum CharacterMovementType {
	Idle = 0,
	Walk = 1,
	Run = 2,
}
