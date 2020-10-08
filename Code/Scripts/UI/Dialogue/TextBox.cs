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
/// Prints text character by character at a set rate
/// for RPG styled dialogue.
/// </summary>
public class TextBox : Node {
	/// <summary>
	/// Signal emitted when all text is finished 
	/// displaying and the textbox is about to be 
	/// closed and freed.
	/// </summary>
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

		//Pressing interact again skips to the next newline immeadiately
		//Displaying all the inbetween text without pause.
		//aka, Allows button mashing through text, but even *faster*
		//than some other games.
		if( IsPausedOnNewLine == false 
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

		//Displays text based on either a timer or handles logic for when
		//there's a pause in text being displayed.
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
			if(IsPausedOnNewLine == false) {
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

			//Newlines pause adding new characters until additional input is given
			//TODO: Use a special character to be able to pause part way through?
			if(CurrentTextIndex < TotalText.Length) {
				currentCharacter = TotalText[CurrentTextIndex].ToString();
				if(currentCharacter.Equals("\n") == true) {
					IsPausedOnNewLine = true;
				}
			}

			//Prevents infinite loops
			if(IsPausedOnNewLine == true) {
				if(Input.IsActionJustPressed("game_interact") == true) {
					CurrentText += currentCharacter;
					DialogueTimer = 0.0f;
					CurrentTextIndex++;
					IsPausedOnNewLine = false;
				}
				return;
			}
		}
	}
	
	/// <summary>
	/// Sets the text that will be displayed and resets 
	/// internal variables for displaying that text.
	/// </summary>
	/// <param name="newText">New text to be displayed</param>
	public void SetText(string newText) {
		TotalText = newText;
		CurrentText = "";
		TextLabel.Text = CurrentText;
		CurrentTextIndex = 0;
		IsPausedOnNewLine = false;
	}
}
