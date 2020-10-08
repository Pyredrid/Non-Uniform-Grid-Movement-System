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

/// <summary>
/// Mostly made as a test script for more 
/// complex behaviours, handles extremely 
/// basic NPC dialogue.
/// </summary>
public class DialogueOnInteract : Node {
	[Export(PropertyHint.File)]
	private string TextBoxPrefab;
	[Export(PropertyHint.MultilineText)]
	private string Dialogue = "";
	[Export]
	private NodePath MapObjectPath = "MapObject";
	private MapObject MapObject;
	[Export]
	private NodePath CharacterControllerPath = "CharacterController";
	private CharacterController CharacterController;

	private TextBox DialogueTextBox = null;

	public override void _Ready() {
		base._Ready();
		MapObject = GetNode<MapObject>(MapObjectPath);
		CharacterController = GetNode<CharacterController>(CharacterControllerPath);
		
		MapObject.Connect(nameof(MapObject.OnInteractedWith), this, nameof(OnInteractedWith_ShowDialogue));
	}

	private void OnInteractedWith_ShowDialogue(Direction directionFrom, Node interactor) {
		if(DialogueTextBox != null) {
			return;
		}
		PackedScene textBoxPackedScene = ResourceLoader.Load<PackedScene>(TextBoxPrefab);
		DialogueTextBox = (TextBox)textBoxPackedScene.Instance();
		AddChild(DialogueTextBox);
		DialogueTextBox.SetText(Dialogue);
		CharacterController.GiveInput(directionFrom.Opposite(), CharacterMovementType.Idle);
		MapManager.LockAll();
		DialogueTextBox.Connect(nameof(TextBox.OnTextCompleted), this, nameof(OnTextCompleted_ReleaseAll));
	}
	
	//A callback to cleanup vairables and release MapObjects again
	private void OnTextCompleted_ReleaseAll() {
		MapManager.ReleaseAll();
		DialogueTextBox = null;
	}
}
