using Godot;
using System;

public class TextBox : Node {
	[Signal]
	public delegate void OnTextCompleted();
	
	[Export]
	private float SecondsPerCharacter = 1.0f/20.0f;
	[Export]
	private NodePath TextLabelPath = "TextLabel";
	private RichTextLabel TextLabel;

	private string TotalText = "";
	private string CurrentText = "";
	private int CurrentTextIndex = 0;
	private float DialogueTimer = 0.0f;
	private bool IsPausedOnNewLine = false;

	public override void _Ready() {
		base._Ready();

		TextLabel = GetNode<RichTextLabel>(TextLabelPath);
	}

	public override void _Process(float delta) {
		base._Process(delta);
		
		DialogueTimer += delta;
		
		TextLabel.Text = CurrentText;

		//Pressing interact again skips to the next newline immeaadiately
		//Displaying all the inbetween text without pause
		if(IsPausedOnNewLine == false 
		&& CurrentTextIndex < TotalText.Length 
		&& Input.IsActionJustPressed("game_interact") == true
		) {
			string currentCharacter = TotalText[CurrentTextIndex].ToString();
			DialogueTimer = 0.0f;
			if(currentCharacter.Equals("\n") == true) {
				return;
			}
			while(true) {
				CurrentText += currentCharacter;
				CurrentTextIndex++;
				if(CurrentTextIndex >= TotalText.Length) {
					break;
				}
				currentCharacter = TotalText[CurrentTextIndex].ToString();
				if(currentCharacter.Equals("\n") == true) {
					break;
				}
			}
			return;
		}

		while(DialogueTimer > SecondsPerCharacter || IsPausedOnNewLine == true) {
			//End of text
			if(CurrentTextIndex >= TotalText.Length) {
				//Interact again to close
				if(Input.IsActionJustReleased("game_interact") == true) {
					EmitSignal(nameof(OnTextCompleted));
					QueueFree();
				}
				DialogueTimer = 0.0f;
				return;
			}

			string currentCharacter = TotalText[CurrentTextIndex].ToString();
			if(currentCharacter.Equals("\n") == true) {
				//Newlines pause adding new characters until additional input is given
				//TODO: Use a special character to be able to pause part way through?
				IsPausedOnNewLine = true;
				if(Input.IsActionJustPressed("game_interact") == true) {
					CurrentText += currentCharacter;
					DialogueTimer = 0.0f;
					CurrentTextIndex++;
					IsPausedOnNewLine = false;
				}
				return;
			} else {
				if(currentCharacter.Equals(".") == true || currentCharacter.Equals(",") == true) {
					//Some punctuation increases the time between characters
					//TODO: Check if a character is punctuation some other way?
					DialogueTimer -= SecondsPerCharacter * 5.0f;
				} else {
					DialogueTimer -= SecondsPerCharacter;
				}
				CurrentText += currentCharacter;
				CurrentTextIndex++;
			}
			//Prevents infinite loops
			if(IsPausedOnNewLine == true) {
				return;
			}
		}
	}
	
	public void SetText(string newText) {
		TotalText = newText;
		CurrentText = "";
		TextLabel.Text = CurrentText;
		CurrentTextIndex = 0;
		IsPausedOnNewLine = false;
	}
}
