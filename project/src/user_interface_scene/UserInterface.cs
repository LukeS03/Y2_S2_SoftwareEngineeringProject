using System.Collections.Generic;
using Godot;

namespace WorldConquest.user_interface_scene;

public partial class UserInterface : Control
{

	public Territory CurrentTerritory;

	public Player CurrentTurn;

	public TerritoryDataMenu TerritoryMenu;

	public List<UserInterfacePlayer> PlayersList;
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.TerritoryMenu = this.GetNode<TerritoryDataMenu>("TerritoryDataMenu");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnTerritoryClicked()
	{
		this.TerritoryMenu.View_Territory(CurrentTerritory);
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
}