using Godot;
using System;
using System.Collections.Generic;

public static class GraphicalExtensions {
	/// <summary>
	/// Iterates over every MeshInstance child
	/// to return an AABB that fits all of them.
	/// </summary>
	public static AABB GetMeshBounds(this Node parent) {
		List<MeshInstance> meshes = parent.GetChildrenOfType<MeshInstance>();
		AABB bounds = new AABB();
		foreach(var mesh in meshes) {
			bounds = bounds.Merge(mesh.GetAabb());
		}
		return bounds;
	}
}