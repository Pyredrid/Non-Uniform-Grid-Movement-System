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

[Tool]
public class MapNodeGridGenerator : Node {
	//This whole "Editor Script" is hacky af
	//I would seriously be careful using this...
	[Export]
	public bool GenerateGrid {
		get => false;
		set => GenerateGridTask(value);
	}

	[Export]
	private string Prefix = "Map Node";
	[Export]
	private int Width = 10;
	[Export]
	private int Height = 10;
	[Export]
	private Reference MapNodeScript;
	[Export]
	private Vector3 NodeRotationDegrees;

	private void GenerateGridTask(bool val) {
		Dictionary<Vector3, Spatial> generatedNodes = new Dictionary<Vector3, Spatial>();
		
		for(int x = 0; x < Width; x++) {
			for(int y = 0; y < Height; y++) {
				Spatial node = new Spatial();
				node.SetScript(MapNodeScript);
				node.Name = Prefix + "(" + x + ", " + y + ")";
				
				Vector3 position = new Vector3(x, 0, y);
				node.Translate(position);
				node.RotationDegrees = NodeRotationDegrees;


				AddChild(node);
				node.Owner = GetTree().EditedSceneRoot;
				generatedNodes.Add(position, node);
			}
		}

		foreach(Spatial generatedNode in generatedNodes.Values) {
			Vector3 generatedNodePosition = generatedNode.Translation;
			
			Spatial up = null;
			Spatial right = null;
			Spatial down = null;
			Spatial left = null;
			
			generatedNodes.TryGetValue(generatedNodePosition + Vector3.Forward, out up);
			generatedNodes.TryGetValue(generatedNodePosition + Vector3.Right, out right);
			generatedNodes.TryGetValue(generatedNodePosition + Vector3.Back, out down);
			generatedNodes.TryGetValue(generatedNodePosition + Vector3.Left, out left);

			generatedNode.Set("NodePathUp", generatedNode.GetPathTo(up));
			generatedNode.Set("NodePathRight", generatedNode.GetPathTo(right));
			generatedNode.Set("NodePathDown", generatedNode.GetPathTo(down));
			generatedNode.Set("NodePathLeft", generatedNode.GetPathTo(left));
		}
	}
}
