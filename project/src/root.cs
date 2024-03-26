using Godot;
using System;
using System.Collections.Generic;
using WorldConquest;

public partial class root : Node2D
{
	public Enum GameStatus;
	public List<Player> Players;
	public Player CurrentTurn;
	public World GameWorld;
	public UserInterface Gui;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.GameWorld = this.GetNode<World>("World");
		this.Gui = this.GetNode<UserInterface>("UserInterface");

		this.GameWorld.InitialisedTerritories += () => SetTerritorySignals();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetTerritorySignals()
	{
		List<Territory> territoriesList = this.GameWorld.Territories;
		foreach(Territory t in territoriesList)
		{
			t.TerritoryClicked += TerritoryClickedTellUi;
		}
	}

	public void TerritoryClickedTellUi(Territory territory)
	{
		this.Gui.CurrentTerritory = territory;
	}
}
