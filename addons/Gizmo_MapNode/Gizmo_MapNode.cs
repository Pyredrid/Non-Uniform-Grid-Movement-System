using Godot;
using System;
#if TOOLS
[Tool]
public class Gizmo_MapNode : EditorPlugin {
	public MapNodeGizmo MapNodeGizmoPlugin;
	public override void _EnterTree() {
		base._EnterTree();
		MapNodeGizmoPlugin = new MapNodeGizmo();
		AddSpatialGizmoPlugin(MapNodeGizmoPlugin);
	}

	public override void _ExitTree() {
		base._ExitTree();
		RemoveSpatialGizmoPlugin(MapNodeGizmoPlugin);
	}
}
#endif