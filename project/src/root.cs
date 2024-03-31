using System.Collections.Generic;
using System.Runtime.Versioning;
using Godot;

namespace WorldConquest;

public partial class root : Node2D
{
	private int _currentPlayerIndex = -1;
	private Territory _territoryBuffer;
	
	public GameStatus GameState;
	public List<Player> Players;
	public Player CurrentTurn;
	public World GameWorld;
	public user_interface_scene.UserInterface Gui;
	private bool _signalReceived = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.GameWorld = this.GetNode<World>("World");
		this.Gui = this.GetNode<user_interface_scene.UserInterface>("UserInterface");
		this.Players = new List<Player>();

		/*This code is invalid and the signal will be removed. Children are initialised before their parents and therefore
		 we don't need this signal to tell the parent instance it has been finished. It also just doesn't work lol. */
		//this.GameWorld.InitialisedTerritories += () => SetTerritorySignals();
		
		SetTerritorySignals();

		this.Gui.DataMenuAction += DataMenuActionSignalReceived;

		this.GameState = GameStatus.StartClaimTerritories;
		
		/*
		 * SAMPLE PLAYERS.
		 * TBD: Remove once the main menu is set up
		 */

		var color1 = new Color(Colors.Aqua);
		var color2 = new Color(Colors.Lavender);
		Player samplePlayer1 = new Player("Sample Player One", color1, false);
		this.Players.Add(samplePlayer1);

		Player samplePlayer2 = new Player("Sample Player Two", color2, true);
		this.Players.Add(samplePlayer2);


		int tokensPerPlayer = 9001;
		switch (Players.Count)
		{
			case 1:
				//hmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm . . .
				tokensPerPlayer = 45;
				break;
			case 2:
				tokensPerPlayer = 40;
				break;
			case 3:
				tokensPerPlayer = 35;
				break;
			case 4:
				tokensPerPlayer = 30;
				break;
			case 5:
				tokensPerPlayer = 25;
				break;
			case 6:
				tokensPerPlayer = 20;
				break;
		}
		
		foreach(var p in Players)
		{
			p.Tokens = tokensPerPlayer;
		}
		
		this.Gui.InitialisePlayers(Players);
		GameTransitionLoop();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void DataMenuActionSignalReceived()
	{
		this._signalReceived = true;
	}

	/// <summary>
	/// Select the next player.
	/// </summary>
	private void SetCurrentPlayer()
	{
		this._currentPlayerIndex = _currentPlayerIndex + 1;
		if (_currentPlayerIndex >= Players.Count)
		{
			this._currentPlayerIndex = 0;
		}
		this.CurrentTurn	 = Players[_currentPlayerIndex];
	}

	/// <summary>
	/// This method sets up click detection signals from each of the territories so that said mouse-click may be
	/// relayed to the UserInterface.
	/// </summary>
	private void SetTerritorySignals()
	{
		List<Territory> territoriesList = this.GameWorld.Territories;
		foreach(Territory t in territoriesList)
		{
			t.TerritoryClicked += TerritoryClickedTellUi;
		}
	}

	/// <summary>
	/// Tells the UserInterface that the user has clicked on a territory.
	/// </summary>
	/// <param name="territory">The territory that has been clicked on by the user.</param>
	private void TerritoryClickedTellUi(Territory territory)
	{
		this.Gui.CurrentTerritory = territory;
		this.Gui.OnTerritoryClicked();
	}

	/// <summary>
	/// 
	/// </summary>
	private void GameTransitionLoop()
	{
		this.GameState = GameStatus.StartClaimTerritories;
		while (true)
		{
			switch (GameState)
			{
				case GameStatus.StartPhase:
					StartPhase();
					break;
			}
			SetCurrentPlayer();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	private void StartPhase()
	{
		this.Gui.UpdateCurrentPlayerAndTurn(this.CurrentTurn, this.GameState);
		this.Gui.CurrentTerritory.Owner = this.CurrentTurn;
		this.CurrentTurn.ControlledTerritories.Add(this.Gui.CurrentTerritory);
		this._signalReceived = false;
	}
}