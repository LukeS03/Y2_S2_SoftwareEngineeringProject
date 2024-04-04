using System.Collections.Generic;
using Godot;

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
namespace WorldConquest.MainMenu_Scene;

using System.Collections.Generic;

public partial class MainMenu : Control
{
	public bool IsCapitalRisk = false;
	public List<Control> Players = new List<Control>();
	public static Color[] PlayerColours;

	private Button addPlayersButton;
	private VBoxContainer playersBox;

	public override void _Ready()
	{
		InitPlayerColours();

		addPlayersButton = GetNode<Button>("AddPlayersButton");
		playersBox = GetNode<VBoxContainer>("PlayersBox");
	}

	private static void InitPlayerColours()
	{
		PlayerColours = new Color[6]
		{
			new Color(Colors.Red),
			new Color(Colors.Blue),
			new Color(Colors.Yellow),
			new Color(Colors.Green),
			new Color(Colors.Purple),
			new Color(Colors.Orange)
		};
	}

	public void _on_Capital_Risk_Enabled(bool isPressed)
	{
		IsCapitalRisk = isPressed;
	}

	public void _on_Add_Players_Button_Clicked()
	{
		if (Players.Count < 6)
		{
			PackedScene playerScene = ResourceLoader.Load<PackedScene>("res://MainMenu_Player.tscn");
			MainMenuPlayer playerInstance = playerScene.Instantiate<MainMenuPlayer>();


			playerInstance.Call("set_player_name", $"Player {Players.Count + 1}");
			playerInstance.Call("set_player_ai_status", false);

			playersBox.AddChild(playerInstance);
			Players.Add(playerInstance);

			if (Players.Count >= 6)
			{
				addPlayersButton.Disabled = true;
			}
		}
	}
}