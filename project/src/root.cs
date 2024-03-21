using Godot;
using System;
using System.Collections.Generic;
using WorldConquest;

public partial class root : Node2D
{
	public Enum GameStatus;
	public List<Player> Players;
	public Player CurrentTurn;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
