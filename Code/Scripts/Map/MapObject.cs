using Godot;
using System;

public class MapObject : Spatial {
	[Signal]
	public delegate void OnMove(Direction dir, bool isWarp);
	[Signal]
	public delegate void OnInteractedWith(Direction directionFrom, Node interactor);

	[Export]
	public bool IsSolid = true;

	private MapNode CurrentNode;

	public override void _Ready() {
		base._Ready();
	}

	public override void _EnterTree() {
		base._EnterTree();
		MapManager.RegisterMapObject(this);
	}

	public override void _ExitTree() {
		base._ExitTree();
		MapManager.UnregisterMapObject(this);
	}

	public override void _Process(float delta) {
		base._Process(delta);
		
		//TODO: Add a way to have unconstrained MapObjects for any special events?
		//TODO: A way to lock and release MapObjects for future scripts...?
		if(CurrentNode == null) {
			CurrentNode = MapManager.GetClosestNode(GlobalTransform.origin);
			if(CurrentNode != null) {
				Warp(CurrentNode);
			}
		}
	}

	public bool Move(Direction dir) {
		if(CurrentNode == null) {
			return false;
		}
		
		MapNode nextNode = CurrentNode.GetAdjacentNode(dir);
		if(nextNode == null) {
			return false;
		}

		if(MapManager.IsTraversable(nextNode) == false) {
			return false;
		}

		CurrentNode.EmitSignal(nameof(MapNode.SteppedOff));
		CurrentNode = nextNode;
		CurrentNode.EmitSignal(nameof(MapNode.SteppedOn));
		EmitSignal(nameof(OnMove), dir, false);
		return true;
	}
	
	public bool Warp(MapNode destination) {
		CurrentNode.EmitSignal(nameof(MapNode.SteppedOff));
		CurrentNode = destination;
		CurrentNode.EmitSignal(nameof(MapNode.SteppedOn));
		EmitSignal(nameof(OnMove), Direction.None, true);
		return false;
	}

	public void InteractWithThis(Direction directionFrom, Node interactor) {
		EmitSignal(nameof(OnInteractedWith), directionFrom, interactor);
	}

	public MapNode GetCurrentNode() {
		return CurrentNode;
	}
}
