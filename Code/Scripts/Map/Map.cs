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
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

//Basically header info for each map
//Can probably be set to hold more info, or fade in
//ambient effects for some maps or whatever...
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
