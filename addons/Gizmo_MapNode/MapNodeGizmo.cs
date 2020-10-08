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
using System.Collections;
using System.Collections.Generic;

#if TOOLS
[Tool]
public class MapNodeGizmo : EditorSpatialGizmoPlugin {
	private Reference MapNodeReference;
	public MapNodeGizmo() : base() {
		CreateMaterial("gizmo_green", new Color(1.0f, 1.0f, 1.0f), false, true);
		CreateMaterial("gizmo_blue", new Color(0.0f, 1.0f, 1.0f), false, false);
		CreateMaterial("gizmo_connection_up", new Color(0.0f, 1.0f, 0.0f), false, false);
		CreateMaterial("gizmo_connection_right", new Color(1.0f, 0.0f, 0.0f), false, false);
		CreateMaterial("gizmo_connection_down", new Color(1.0f, 0.0f, 1.0f), false, false);
		CreateMaterial("gizmo_connection_left", new Color(1.0f, 1.0f, 0.0f), false, false);
		MapNodeReference = ResourceLoader.Load("res://Code/Scripts/Map/MapNode.cs");
	}
	
	public override string GetName() {
		return "Map Node";
	}

	public override bool HasGizmo(Spatial spatial) {
		return (spatial.GetScript() == MapNodeReference);
	}

	public override void Redraw(EditorSpatialGizmo gizmo) {
		//base.Redraw(gizmo);
		gizmo.Clear();

		Spatial spatial = gizmo.GetSpatialNode();
		
		Vector3[] yLines = {
			new Vector3(0.0f, 0.0f, 0.0f),
			spatial.Transform.basis.y * 0.25f,
		};

		Vector3[] zLines = {
			new Vector3(0.0f, 0.0f, 0.0f),
			(spatial.Transform.basis.z * 0.25f)// + (spatial.Transform.basis.y * 0.25f),
		};

		gizmo.AddLines(yLines, GetMaterial("gizmo_green", gizmo));
		gizmo.AddLines(zLines, GetMaterial("gizmo_blue", gizmo));

		Spatial up = spatial.GetNode<Spatial>(spatial.Get("NodePathUp") + "");
		Spatial right = spatial.GetNode<Spatial>(spatial.Get("NodePathRight") + "");
		Spatial down = spatial.GetNode<Spatial>(spatial.Get("NodePathDown") + "");
		Spatial left = spatial.GetNode<Spatial>(spatial.Get("NodePathLeft") + "");

		if(up != null) {
			Vector3 neighborPoint = up.GlobalTransform.origin - spatial.GlobalTransform.origin;
			neighborPoint = new Quat(-spatial.Rotation).Xform(neighborPoint);
			Vector3[] connectionLine = {
				new Vector3(0.0f, 0.0f, 0.0f),
				neighborPoint,
			};
			gizmo.AddLines(connectionLine, GetMaterial("gizmo_connection_up", gizmo));
		}
		if(right != null) {
			Vector3 neighborPoint = right.GlobalTransform.origin - spatial.GlobalTransform.origin;
			neighborPoint = new Quat(-spatial.Rotation).Xform(neighborPoint);
			Vector3[] connectionLine = {
				new Vector3(0.0f, 0.0f, 0.0f),
				neighborPoint,
			};
			gizmo.AddLines(connectionLine, GetMaterial("gizmo_connection_right", gizmo));
		}
		if(down != null) {
			Vector3 neighborPoint = down.GlobalTransform.origin - spatial.GlobalTransform.origin;
			neighborPoint = new Quat(-spatial.Rotation).Xform(neighborPoint);
			Vector3[] connectionLine = {
				new Vector3(0.0f, 0.0f, 0.0f),
				neighborPoint,
			};
			gizmo.AddLines(connectionLine, GetMaterial("gizmo_connection_down", gizmo));
		}
		if(left != null) {
			Vector3 neighborPoint = left.GlobalTransform.origin - spatial.GlobalTransform.origin;
			neighborPoint = new Quat(-spatial.Rotation).Xform(neighborPoint);
			Vector3[] connectionLine = {
				new Vector3(0.0f, 0.0f, 0.0f),
				neighborPoint,
			};
			gizmo.AddLines(connectionLine, GetMaterial("gizmo_connection_left", gizmo));
		}
	}
}
#endif