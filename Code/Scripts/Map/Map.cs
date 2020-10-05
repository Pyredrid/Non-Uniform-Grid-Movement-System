using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

//Basically header info for each map
public class Map : Node {
	[Export]
	private string MapName = "Map Name";
	[Signal]
	public delegate void OnUnload();

	private List<MapNode> Nodes = new List<MapNode>();

	public override void _Ready() {
		base._Ready();
		Nodes.AddRange(this.GetChildrenOfType<MapNode>());
	}
	
	public List<MapNode> GetMapNodes() {
		return Nodes;
	}
}
