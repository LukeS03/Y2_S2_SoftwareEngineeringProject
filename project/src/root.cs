using Godot;
using System;
using System.Collections.Generic;
using WorldConquest;

public partial class root : Node2D
{
	private int _currentPlayerIndex = -1;
	private Territory _territoryBuffer;
	
	public GameStatus GameState;
	public List<Player> Players;
	public Player CurrentTurn;
	public World GameWorld;
	public UserInterface Gui;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.GameWorld = this.GetNode<World>("World");
		this.Gui = this.GetNode<UserInterface>("UserInterface");

		/*This code is invalid and the signal will be removed. Children are initialised before their parents and therefore
		  we don't need this signal to tell the parent instance it has been finished. It also just doesn't work lol. */
		//this.GameWorld.InitialisedTerritories += () => SetTerritorySignals();
		
		SetTerritorySignals();

		this.GameState = GameStatus.StartClaimTerritories;
		
		/*
		 * SAMPLE PLAYERS.
		 * TBD: Remove once the main menu is set up
		 */

		int TokensPerPlayer;
		switch (Players.Count)
		{
			
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
		this.Gui.CurrentTurn = this.CurrentTurn;
		
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
		while (true)
		{
			SetCurrentPlayer();
			switch (GameState)
			{
				case GameStatus.StartClaimTerritories:
					StartClaimTerritories();
					break;
				case GameStatus.StartFortifyTerritories:
					StartClaimTerritories();
					break;
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	private void StartClaimTerritories()
	{
		/*
		 * 1. set the UI's GameStatus to StartClaimTerritories
		 * 2. Set the UI's CurrentTurn to match this one's
		 * 3. Await the signal from the UI.
		 * 4. Assign the chosen territory to the territory returned by the UI.
		 */
	}

	/// <summary>
	/// 
	/// </summary>
	private void StartFortifyTerritories()
	{
		/*
		 * kinda the same as above tbh
		 */
	}
}
