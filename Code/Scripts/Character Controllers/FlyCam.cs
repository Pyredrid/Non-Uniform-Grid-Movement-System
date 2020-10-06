using Godot;
using System;

public class FlyCam : Spatial {
	/*
	[Export]
	private float MovementSpeed = 2.0f;
	[Export]
	private float RotationSpeed = 2.0f;
	[Export]
	private bool InputIsActive = false;

	private Vector2 LastMousePosition = new Vector2();
	private Vector2 MouseDelta = new Vector2();
	
	public override void _Ready() {
		base._Ready();
	}
	
	public override void _Process(float delta) {
		base._Process(delta);
		if(Input.IsActionJustPressed("Toggle_FlyCam") == true) {
			InputIsActive = !InputIsActive;
			if(InputIsActive == true) {
				Input.SetMouseMode(Input.MouseMode.Captured);
			} else {
				Input.SetMouseMode(Input.MouseMode.Visible);
			}
		}
		if(InputIsActive == false) {
			return;
		}

		Vector3 rot = RotationDegrees;
		rot += new Vector3(MouseDelta.y * delta * RotationSpeed, MouseDelta.x * delta * RotationSpeed, 0);
		rot.x = Mathf.Clamp(rot.x, -80, 80);
		rot.y = rot.y % 360.0f;
		RotationDegrees = rot;

		MouseDelta = new Vector2();

		if(Input.IsActionPressed("Game_Up") == true) {
			Vector3 dir = -GlobalTransform.basis.z;
			Translate(dir * MovementSpeed * delta);
		}
		if(Input.IsActionPressed("Game_Down") == true) {
			Vector3 dir = GlobalTransform.basis.z;
			Translate(dir * MovementSpeed * delta);
		}
		if(Input.IsActionPressed("Game_Left") == true) {
			Vector3 dir = -GlobalTransform.basis.x;
			Translate(dir * MovementSpeed * delta);
		}
		if(Input.IsActionPressed("Game_Right") == true) {
			Vector3 dir = GlobalTransform.basis.x;
			Translate(dir * MovementSpeed * delta);
		}
	}

	public override void _Input(InputEvent e) {
		base._Input(e);
		if(InputIsActive == false) {
			return;
		}
		if(e is InputEventMouse em) {
			Vector2 mousePosition = em.Position;
			MouseDelta = mousePosition - LastMousePosition;
			LastMousePosition = mousePosition;
			//Input.WarpMousePosition(new Vector2(512, 512));
		}
	}
	//*/
}
