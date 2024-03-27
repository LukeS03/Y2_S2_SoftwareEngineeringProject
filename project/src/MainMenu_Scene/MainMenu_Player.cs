using Godot;
using System;

/*
	TODO:
		1. Create a Setter method that updates the label colour and label text when a player is added.
		2. Receive the signal from "IsAIPlayerButton" to update the Player's IsCPU value.
*/

public partial class MainMenu_Player : HBoxContainer
{
	public Player thePlayer {
		get;
		set { /* Update label colour; update label text. */ }
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
