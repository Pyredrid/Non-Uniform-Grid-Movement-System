using Godot;
using System;

public class EnforceUp : Spatial {
	public override void _Ready() {
		VisualServer.Singleton.Connect("frame_pre_draw", this, nameof(SetUpDirection));
	}
	
	private void SetUpDirection() {
		Vector3 position = GlobalTransform.origin;
		Vector3 forward = -GlobalTransform.basis.z;
		LookAt(position + forward, Vector3.Up);
	}
}
