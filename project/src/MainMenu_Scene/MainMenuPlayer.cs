using Godot;
using System;
using WorldConquest;
namespace WorldConquest.MainMenu_Scene;

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

	private void UpdatePlayerLabel()
	{
		// Example update. Adjust based on your Player object's structure.
		_playerLabel.Text = ThePlayer.Name;
		
		_playerLabel.AddThemeColorOverride("font_color", ThePlayer.PlayerColour);
	}

	private void _on_IsAIPlayerButton_toggled(bool ToggledOn)
	{
		// Update the IsCPU value of the player and refresh the label.
		ThePlayer.IsCPU = ToggledOn;
		if (ThePlayer.IsCPU) this.ThePlayer.Name = "(AI) Player " + playerNo;
		else this.ThePlayer.Name = "Player " + playerNo;
		UpdatePlayerLabel();
	}
}
