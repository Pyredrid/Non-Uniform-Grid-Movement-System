using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapNode : Spatial {
	[Signal]
	public delegate void SteppedOn();
	[Signal]
	public delegate void SteppedOff();
	
	[Export]
	private NodePath NodePathUp = null;
	[Export]
	private NodePath NodePathRight = null;
	[Export]
	private NodePath NodePathDown = null;
	[Export]
	private NodePath NodePathLeft = null;

	private MapNode[] AdjacentNodes = new MapNode[4];

	public override void _Ready() {
		base._Ready();
		if(NodePathUp != null && NodePathUp.IsEmpty() == false) {
			AdjacentNodes[(int)Direction.Up] = GetNode<MapNode>(NodePathUp);
		}
		if(NodePathRight != null && NodePathRight.IsEmpty() == false) {
			AdjacentNodes[(int)Direction.Right] = GetNode<MapNode>(NodePathRight);
		}
		if(NodePathDown != null && NodePathDown.IsEmpty() == false) {
			AdjacentNodes[(int)Direction.Down] = GetNode<MapNode>(NodePathDown);
		}
		if(NodePathLeft != null && NodePathLeft.IsEmpty() == false) {
			AdjacentNodes[(int)Direction.Left] = GetNode<MapNode>(NodePathLeft);
		}
	}

	public MapNode GetAdjacentNode(Direction dir) {
		MapNode adjacentNode = AdjacentNodes[(int)dir];
		return adjacentNode;
	}
	
	public void SetAdjacentNode(Direction dir, MapNode adjacentNode) {
		AdjacentNodes[(int)dir] = adjacentNode;
	}
}
