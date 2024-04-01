using System.Collections.Generic;
using Godot;
using WorldConquest.UserInterface_Scene;

namespace WorldConquest.UserInterface_Scene;

public partial class UserInterface : Control
{

	public Territory CurrentTerritory;
	private Player _currentTurn;
	private GameStatus _gameState;
	public TerritoryDataMenu TerritoryMenu;
	public List<UserInterfacePlayer> PlayersList;
	private Label _modeLabel;

	[Signal]
	public delegate void DataMenuActionEventHandler();
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.TerritoryMenu = this.GetNode<TerritoryDataMenu>("TerritoryDataMenu");
		this._modeLabel = this.GetNode<Label>("BottomMenuBar/ModeLabel");
		this.TerritoryMenu.Visible = false; // hide the territory data menu until a territory is clicked.
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnTerritoryClicked()
	{
		this.TerritoryMenu.View_Territory(CurrentTerritory, _gameState);
	}

	public void OnDataMenuAction()
	{
		EmitSignal(UserInterface.SignalName.DataMenuAction);
		this.TerritoryMenu.Visible = false;
	}

	public void InitialisePlayers(List<Player> players)
	{
		PlayersList = new List<UserInterfacePlayer>();
		foreach (var p in players)
		{
			var playerMenuElementScene = ResourceLoader.Load<PackedScene>("res://scenes/UserInterface_Player.tscn").Instantiate();
			this.GetNode<HBoxContainer>("BottomMenuBar/PlayersHBox").AddChild(playerMenuElementScene);
			this.PlayersList.Add((UserInterfacePlayer)playerMenuElementScene);
			((UserInterfacePlayer)playerMenuElementScene).Player = p;
		}
	}

	public void UpdateCurrentPlayerAndTurn(Player player, GameStatus state)
	{
		this._currentTurn = player;
		this._gameState   = state;
		string newText;
		newText = player.Name + ": ";
		switch (state)
		{
			case GameStatus.StartClaimTerritories:
				newText += "Select an unclaimed territory to claim.";
				break;
			case GameStatus.StartFortifyTerritories:
				newText += "Select one of your territories to fortify with one troop.";
				break;
		}

		this._modeLabel.Text = newText;
	}
}