using Godot;
using System;
using WorldConquest;

public partial class UserInterface : Control
{

	public Territory CurrentTerritory;

    public Player CurrentTurn;

    public TerritoryDataMenu TerritoryMenu;
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
}
