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

public class HumanoidCharacterGraphics : Node {
	[Export]
	private NodePath CharacterControllerPath = "CharacterController";
	[Export]
	private NodePath AnimationPlayerPath = "AnimationPlayer";
	[Export]
	public bool IgnoreFacingCamera = false;

    private CharacterController CharacterController;
	private AnimationPlayer AnimationPlayer;
	private Direction FacingDirection = Direction.Down;

    public override void _Ready() {
        base._Ready();
		CharacterController = GetNode<CharacterController>(CharacterControllerPath);
		AnimationPlayer = GetNode<AnimationPlayer>(AnimationPlayerPath);
    }

	public override void _Process(float delta) {
		UpdateAnimation();
		if(CharacterController.GetMapObject().GetCurrentNode() != null) {
			HandleCameraFacingDirection(GetViewport().GetCamera());
		}
	}

	/// <summary>
	/// Uses CurrentDirection and the given Camera's
	/// direction to "turn" the sprite a la Doom so it appears
	/// to be facing properly.
	/// </summary>
	private void HandleCameraFacingDirection(Camera camera) {
		if(IgnoreFacingCamera == true) {
			FacingDirection = CharacterController.GetCurrentDirection();
			return;
		}
        MapObject characterMapObject = CharacterController.GetMapObject();
        Direction currentDirection = CharacterController.GetCurrentDirection();

		Vector3 cameraForward = -camera.GlobalTransform.basis.z;
		Vector3 currentNodeForward = -characterMapObject.GetCurrentNode().GlobalTransform.basis.z;

		Direction cameraDirection = DirectionUtilities.GetClosestDirection(cameraForward);
		Direction currentNodeDirection = DirectionUtilities.GetClosestDirection(currentNodeForward);

		FacingDirection = currentDirection.AdjustUp(cameraDirection.AdjustUp(currentNodeDirection));
	}

	private void UpdateAnimation() {
        //TODO: If performance is needed on CPU, cache this string for animation
        CharacterMovementType movementType = CharacterController.GetMovementType();
		AnimationPlayer.Play(movementType.ToString() + "_" + FacingDirection.ToString());
	}
}
