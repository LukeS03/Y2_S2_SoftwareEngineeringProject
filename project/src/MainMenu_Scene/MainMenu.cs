using Godot;
using System;
using System.Collections.Generic;
using WorldConquest;

/*
 * TODO: 
 1 Toggling the "Enable Capital Risk" option should toggle "IsCapitalRisk" boolean.
 2 Pressing the "Add Player" button should add a new player until there are six or more players.
	a Each time the button is pressed, it should initialise a new instance of "MainMenu_Player.tscn" as a child of the "PlayersBox" node.
	b When there are six players, the button to add players should be greyed out and disabled.
	c Each player should be named as "Player n" where 'n' is a placeholder for the number of that player.
	d Pressing the "Is AI" button should toggle whether that player isCPU or not.
 4 I will handle the initialisation of the game scene next.
 */
public partial class MainMenu : Control
{
	public bool IsCapitalRisk;
	public List<Player> Players;
	public static Color[] PlayerColours;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private static void InitPlayerColours()
	{
		PlayerColours = new Color[6];
		PlayerColours[0] = new Color(Colors.Red);
		PlayerColours[1] = new Color(Colors.Blue);
		PlayerColours[3] = new Color(Colors.Green);
		PlayerColours[4] = new Color(Colors.Purple);
		PlayerColours[5] = new Color(Colors.Orange);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_Capital_Risk_Enabled()
	{
		//Called when the "CapitalRiskCheckBox" is clicked.	
	}

	public void _on_Add_Players_Button_Clicked()
	{
		//Called when "AddPlayersButton" is clicked.
	}

	public void _on_Start_Button_Clicked()
	{
		// Called when "StartButton" is clicked.
	}
}
