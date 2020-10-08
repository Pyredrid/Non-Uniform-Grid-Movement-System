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
