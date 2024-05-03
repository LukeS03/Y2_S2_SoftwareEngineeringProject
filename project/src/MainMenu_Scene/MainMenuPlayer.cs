using Godot;
using System;
using WorldConquest;
namespace WorldConquest.MainMenu_Scene;

/// <summary>
/// This class is used to represent each player within the main menu, allowing the player to configure that player e.g.
/// changing whether the player is an AI player or not.
/// </summary>
public partial class MainMenuPlayer : HBoxContainer
{
	private Player _player;
	private Label _playerLabel; // Assuming there's a Label node to show player info.

	// Property with setter to update label when a player is set.
	public Player ThePlayer
	{
		get => _player;
		set
		{
			_player = value;
			UpdatePlayerLabel(); // Call method to update label text and color.
		}
	}

	public int playerNo;
	
	public override void _Ready()
	{
		_playerLabel = GetNode<Label>("PlayerName"); // Adjust the path as needed.
		var isAiPlayerButton = GetNode<Button>("IsAIPlayerButton"); // Adjust the path as needed.
	}

	/// <summary>
	/// This method is used to update the player label so that the text and font colour match the player's name and
	/// colour.
	/// </summary>
	private void UpdatePlayerLabel()
	{
		// Example update. Adjust based on your Player object's structure.
		_playerLabel.Text = ThePlayer.Name;
		
		_playerLabel.AddThemeColorOverride("font_color", ThePlayer.PlayerColour);
	}

	/// <summary>
	/// This method is called when the checkbox to toggle whether the player is an AI player or not is clicked.
	/// </summary>
	/// <param name="ToggledOn"></param>
	private void _on_IsAIPlayerButton_toggled(bool ToggledOn)
	{
		// Update the IsCPU value of the player and refresh the label.
		ThePlayer.IsCPU = ToggledOn;
		
		//Add an annotation to denote that the player is an AI player.
		if (ThePlayer.IsCPU) this.ThePlayer.Name = "(AI) Player " + playerNo;
		else this.ThePlayer.Name = "Player " + playerNo;
		UpdatePlayerLabel();
	}
}
