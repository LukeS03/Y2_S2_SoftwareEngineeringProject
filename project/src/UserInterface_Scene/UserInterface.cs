using System.Collections.Generic;
using Godot;
using WorldConquest.UserInterface_Scene;

namespace WorldConquest.UserInterface_Scene;

/// <summary>
/// Contains the user interface as displayed when the game is in session.
/// </summary>
public partial class UserInterface : Control
{

	public Territory CurrentTerritory;
	private Player _currentTurn;
	private GameStatus _gameState;
	public TerritoryDataMenu TerritoryMenu;
	public List<UserInterfacePlayer> PlayersList;
	private Label _modeLabel;
	private Label _activePlayerLabel;
	private Button _endTurnButton;
	public UserInterfaceNumberInput NumInputMenu;
	
	/// <summary>
	/// This signal is emitted when the 'Data Menu Action Button' is clicked in the territory data menu. This button is
	/// kind of a general purpose button, whose text and behaviour change depending on the current state of the game.
	/// </summary>
	[Signal]
	public delegate void DataMenuActionEventHandler();
	
	/// <summary>
	/// Sends a signal to tell the game logic that the player has ended the current turn state, and is ready to either
	/// move to the next turn stage, or to end their turn.
	/// </summary>
	[Signal]
	public delegate void EndTurnButtonEventHandler();
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.TerritoryMenu = this.GetNode<TerritoryDataMenu>("TerritoryDataMenu");
		this._modeLabel = this.GetNode<Label>("BottomMenuBar/ModeLabel");
		this.TerritoryMenu.Visible = false; // hide the territory data menu until a territory is clicked.
		this._activePlayerLabel = this.GetNode<Label>("BottomMenuBar/ActivePlayerLabel");
		this.NumInputMenu = this.GetNode<UserInterfaceNumberInput>("UserInterfaceNumberInput");
		this._endTurnButton = this.GetNode<Button>("BottomMenuBar/EndTurnButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Called by the game logic embedded in BoardRoot whenever it receives a TerritoryClicked signal. This method calls
	/// a method of the TerritoryDataMenu to display the menu.
	/// </summary>
	public void OnTerritoryClicked()
	{
		this.TerritoryMenu.View_Territory(CurrentTerritory, _gameState, _currentTurn);
	}

	/// <summary>
	/// Called by the TerritoryDataMenu whenever it's action button is clicked. Emits a signal to BoardRoot, which
	/// is then handled and used to carry out different behaviours depending on the current turn stage of the game.
	/// </summary>
	public void OnDataMenuAction()
	{
		EmitSignal(UserInterface.SignalName.DataMenuAction);
		this.TerritoryMenu.Visible = false;
	}

	/// <summary>
	/// Called when the player clicks on the "end turn" button.
	/// </summary>
	public void onEndTurnButtonClicked()
	{
		EmitSignal(UserInterface.SignalName.EndTurnButton);
	}

	/// <summary>
	/// This method initialises each player within the user interface so that each player is represented in the UI.
	/// </summary>
	/// <param name="players">The list of players who are currently playing.</param>
	public void InitialisePlayers(List<Player> players)
	{
		//Initialise a list of GUI elements for each player.
		PlayersList = new List<UserInterfacePlayer>();
		foreach (var p in players)
		{
			//Each player gets their own 'UserInterfacePlayer' element in the GUI which shows data about that player 
			//including their name, their colour, and the amount of infantry tokens they have available.
			var playerMenuElementScene = ResourceLoader.Load<PackedScene>("res://scenes/UserInterface_Player.tscn").Instantiate();
			this.GetNode<HBoxContainer>("BottomMenuBar/PlayersHBox").AddChild(playerMenuElementScene);
			this.PlayersList.Add((UserInterfacePlayer)playerMenuElementScene);
			((UserInterfacePlayer)playerMenuElementScene).Player = p;
		}
	}

	/// <summary>
	/// Update the GUI to display which player is currently active and what turn stage they are at. This method is called
	/// by BoardRoot every time the current turn stage is changed.
	/// </summary>
	/// <param name="player"></param>
	/// <param name="state"></param>
	public void UpdateCurrentPlayerAndTurn(Player player, GameStatus state)
	{
		this._currentTurn = player;
		this._gameState   = state;
		
		//Change the text displayed in "ModeLabel" to prompt the player to take an action depending on the current
		//turn stage.
		string PlayerPromptText = "";
		switch (state)
		{
			case GameStatus.StartClaimTerritories:
				this._endTurnButton.Visible = false;
				PlayerPromptText += "Select an unclaimed territory to claim.";
				break;
			case GameStatus.StartFortifyTerritories:
				this._endTurnButton.Visible = false;
				PlayerPromptText += "Select one of your territories to fortify with one troop.";
				break;
			case GameStatus.FortifyTerritoriesStage:
				this._endTurnButton.Visible = true;
				PlayerPromptText += "Select one of your territories to fortify.";
				break;
		}

		//Update ActivePlayerLabel to display which player is currently active, and change it's font colour to match
		//that player's colour.
		this._modeLabel.Text = PlayerPromptText;
		this._activePlayerLabel.Text = player.Name + "'s turn";
		this._activePlayerLabel.Modulate = player.PlayerColour;
	}

	/// <summary>
	/// Update the amount of tokens that each player has available.
	/// </summary>
	public void UpdatePlayersAvailableTokens()
	{
		foreach (UserInterfacePlayer userInterfacePlayer in PlayersList)
		{
			userInterfacePlayer.PlayerTokensLabel.Text = "Tokens: " + userInterfacePlayer.Player.Tokens.ToString();
		}
	}
}