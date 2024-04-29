using System.Collections.Generic;
using System.Runtime.Versioning;
using Godot;
using WorldConquest.UserInterface_Scene;

namespace WorldConquest;

public partial class BoardRoot : Node2D
{
	private int _currentPlayerIndex = -1;
	private Territory _territoryBuffer;
	
	public GameStatus GameState;
	public List<Player> Players;
	public Player CurrentTurn;
	public World GameWorld;
	public UserInterface Gui;
	
	//game config:
	public bool AutoAssign = false;
	public bool CapitalRisk = false;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.GameWorld = this.GetNode<World>("World");
		this.Gui = this.GetNode<UserInterface>("UserInterface");
		
		
		SetTerritorySignals();

		this.Gui.DataMenuAction += DataMenuActionSignalReceived;
		this.Gui.NumInputMenu.SpinBoxInputConfirmed += SpinboxInputSignal;
		this.Gui.EndTurnButton += endTurnButtonClicked;

		this.GameState = GameStatus.StartClaimTerritories;
		
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
		SetCurrentPlayer();
		this.Gui.UpdateCurrentPlayerAndTurn(this.CurrentTurn, this.GameState);
		if(AutoAssign) autoAssignTerritories();
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
	
	private void DataMenuActionSignalReceived()
	{
		switch (GameState)
		{
			case GameStatus.StartClaimTerritories:
				StartClaimTerritories();
				break;
			case GameStatus.StartFortifyTerritories:
				StartFortifyTerritories();
				break;
			case GameStatus.FortifyTerritoriesStage:
				FortifyTerritory();
				break;
		}
	}

	private void SpinboxInputSignal(int numInput)
	{
		switch (this.GameState)
		{
			case GameStatus.FortifyTerritoriesStage:
				this.Gui.CurrentTerritory.Tokens += numInput;
				this.CurrentTurn.Tokens -= numInput;
				this.Gui.UpdatePlayersAvailableTokens();
				break;
		}
	}

	private void TurnTransition()
	{
		switch (GameState)
		{
			case GameStatus.StartClaimTerritories:
				if (this.GameWorld.GetUnclaimedTerritories().Count != 0)
				{
					SetCurrentPlayer();
					this.Gui.UpdateCurrentPlayerAndTurn(this.CurrentTurn, this.GameState);
				}
				else
				{
					this.GameState = GameStatus.StartFortifyTerritories;
					TurnTransition();
				}
				break;
			case GameStatus.StartFortifyTerritories:
				if (Player.AnyPlayersHaveRemainingTokens(Players))
				{
					do
					{
						SetCurrentPlayer();
					} while (CurrentTurn.Tokens <= 0);
					this.Gui.UpdateCurrentPlayerAndTurn(this.CurrentTurn, this.GameState);
				}
				else
				{
					this.GameState = GameStatus.FortifyTerritoriesStage;
					this._currentPlayerIndex = -1;
					TurnTransition();
				}
				break;
			case GameStatus.FortifyTerritoriesStage:
				//Calculate the amount of tokens the player gets this turn.
				SetCurrentPlayer();
				this.Gui.UpdateCurrentPlayerAndTurn(CurrentTurn, GameState);
				this.CurrentTurn.Tokens += 3; // add the minimum number of extra tokens
				foreach (Continent c in this.GameWorld.PlayerOwnsContinents(CurrentTurn)) this.CurrentTurn.Tokens += c.Tokens; // and the respective tokens for continents owned by player
				this.Gui.UpdatePlayersAvailableTokens();
				break;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	private void StartClaimTerritories()
	{
		this.Gui.CurrentTerritory.SetTerritoryOwner(this.CurrentTurn);
		this.Gui.CurrentTerritory.Tokens = 1;
		this.CurrentTurn.Tokens -= 1;
		this.Gui.UpdatePlayersAvailableTokens();
		TurnTransition();
	}

	/// <summary>
	/// 
	/// </summary>
	private void StartFortifyTerritories()
	{
		this.Gui.CurrentTerritory.Tokens += 1;
		this.CurrentTurn.Tokens -= 1;
		this.Gui.UpdatePlayersAvailableTokens();
		TurnTransition();
	}

	private void FortifyTerritory()
	{
		this.Gui.NumInputMenu.ShowNumberInput("Fortify Territory", 0, CurrentTurn.Tokens);
	}

	private void autoAssignTerritories()
	{
		foreach (var t in GameWorld.Territories)
		{
			SetCurrentPlayer();
			t.Owner = CurrentTurn;
			CurrentTurn.ControlledTerritories.Add(t);
		}

		_currentPlayerIndex = -1;
		GameState = GameStatus.FortifyTerritoriesStage;
		TurnTransition();
	}

	private void endTurnButtonClicked()
	{
		TurnTransition();
	}
}
