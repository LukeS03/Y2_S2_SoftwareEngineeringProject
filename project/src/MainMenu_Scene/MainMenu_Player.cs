using Godot;
using System;

public partial class MainMenu_Player : HBoxContainer
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
    
    public override void _Ready()
    {
        _playerLabel = GetNode<Label>("PlayerLabel"); // Adjust the path as needed.

        // Connect the signal from the IsAIPlayerButton to a method.
        var isAIPlayerButton = GetNode<Button>("IsAIPlayerButton"); // Adjust the path as needed.
        isAIPlayerButton.Connect("toggled", this, nameof(_on_IsAIPlayerButton_toggled));
    }

    private void UpdatePlayerLabel()
    {
        // Example update. Adjust based on your Player object's structure.
        _playerLabel.Text = ThePlayer.Name;
        
        // Change label color based on whether the player is an AI.
        _playerLabel.Modulate = ThePlayer.IsCPU ? Colors.Gray : Colors.White;
    }

    private void _on_IsAIPlayerButton_toggled(bool buttonPressed)
    {
        // Update the IsCPU value of the player and refresh the label.
        ThePlayer.IsCPU = buttonPressed;
        UpdatePlayerLabel();
    }
}