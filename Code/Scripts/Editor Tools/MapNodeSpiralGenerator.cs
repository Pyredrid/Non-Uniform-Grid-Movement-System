using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

[Tool]
public class MapNodeSpiralGenerator : Node {
	[Export]
	public bool GenerateSpiral {
		get => false;
		set => GenerateSpiralTask(value);
	}

	[Export]
	private string Prefix = "Spiral Node";
	[Export]
	private int Width = 3;
	[Export]
	private float WidthOffset = 2;
	[Export]
	private float TotalHeight = 10;
	[Export]
	private int Steps = 16;
	[Export]
	private float TotalRotation = 360;
	[Export]
	private Vector3 StartingNodeRotationDegrees;

	[Export]
	private Reference MapNodeScript;
	
	[Export]
	private Direction SpiralForwardIsDir = Direction.Up;
	
	//TODO: Specify if spiral is clockwise or counterclockwise...

	private void GenerateSpiralTask(bool val) {
		Spatial[,] generatedNodes = new Spatial[Steps, Width];
		
		float heightPerStep = TotalHeight / (Steps);
		float rotationPerStep = TotalRotation / (Steps);
		
		for(int s = 0; s < Steps; s++) {
			for(int x = 0; x < Width; x++) {
				Spatial node = new Spatial();
				node.SetScript(MapNodeScript);
				node.Name = Prefix + " (x " + x + ") (step " + s+")";

				Vector3 position = new Vector3(0, 0, 0);
				position.x = x + WidthOffset;
				position.y = heightPerStep * s;
				position = position.Rotated(Vector3.Up, Mathf.Deg2Rad(rotationPerStep * (s + 0.5f)));
				node.Translate(position);

				AddChild(node);
				node.Owner = GetTree().EditedSceneRoot;
				
				generatedNodes[s, x] = node;
			}
		}

		for(int s = 0; s < Steps; s++) {
			for(int x = 0; x < Width; x++) {
				Spatial up = null;
				Spatial right = null;
				Spatial down = null;
				Spatial left = null;

				if(s < Steps-1) {
					up = generatedNodes[s + 1, x];
				}
				if(x < Width-1) {
					right = generatedNodes[s, x + 1];
				}
				if(s > 0) {
					down = generatedNodes[s - 1, x];
				}
				if(x > 0) {
					left = generatedNodes[s, x - 1];
				}

				if(SpiralForwardIsDir == Direction.Up) {
					if(up != null) {
						generatedNodes[s, x].Set("NodePathUp", generatedNodes[s, x].GetPathTo(up));
					}
					if(right != null) {
						generatedNodes[s, x].Set("NodePathRight", generatedNodes[s, x].GetPathTo(right));
					}
					if(down != null) {
						generatedNodes[s, x].Set("NodePathDown", generatedNodes[s, x].GetPathTo(down));
					}
					if(left != null) {
						generatedNodes[s, x].Set("NodePathLeft", generatedNodes[s, x].GetPathTo(left));
					}
				} else if(SpiralForwardIsDir == Direction.Right) {
					if(up != null) {
						generatedNodes[s, x].Set("NodePathRight", generatedNodes[s, x].GetPathTo(up));
					}
					if(right != null) {
						generatedNodes[s, x].Set("NodePathDown", generatedNodes[s, x].GetPathTo(right));
					}
					if(down != null) {
						generatedNodes[s, x].Set("NodePathLeft", generatedNodes[s, x].GetPathTo(down));
					}
					if(left != null) {
						generatedNodes[s, x].Set("NodePathUp", generatedNodes[s, x].GetPathTo(left));
					}
				} else if(SpiralForwardIsDir == Direction.Down) {
					if(up != null) {
						generatedNodes[s, x].Set("NodePathDown", generatedNodes[s, x].GetPathTo(up));
					}
					if(right != null) {
						generatedNodes[s, x].Set("NodePathLeft", generatedNodes[s, x].GetPathTo(right));
					}
					if(down != null) {
						generatedNodes[s, x].Set("NodePathUp", generatedNodes[s, x].GetPathTo(down));
					}
					if(left != null) {
						generatedNodes[s, x].Set("NodePathRight", generatedNodes[s, x].GetPathTo(left));
					}
				} else if(SpiralForwardIsDir == Direction.Left) {
					if(up != null) {
						generatedNodes[s, x].Set("NodePathLeft", generatedNodes[s, x].GetPathTo(up));
					}
					if(right != null) {
						generatedNodes[s, x].Set("NodePathUp", generatedNodes[s, x].GetPathTo(right));
					}
					if(down != null) {
						generatedNodes[s, x].Set("NodePathRight", generatedNodes[s, x].GetPathTo(down));
					}
					if(left != null) {
						generatedNodes[s, x].Set("NodePathDown", generatedNodes[s, x].GetPathTo(left));
					}
				}
			}
		}
		
		foreach(Node child in GetChildren()) {
			Spatial spatial = (Spatial)child;
			spatial.LookAt(Vector3.Zero, Vector3.Up);

			Vector3 rot = spatial.RotationDegrees;
			rot.x = 0;
			spatial.RotationDegrees = rot;
		}
	}
}
