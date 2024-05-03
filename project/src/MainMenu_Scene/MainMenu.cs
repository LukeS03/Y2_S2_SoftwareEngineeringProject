using System.Collections.Generic;
using Godot;

namespace WorldConquest.MainMenu_Scene;

using System.Collections.Generic;

/// <summary>
/// This class is used to control the main menu.
/// </summary>
public partial class MainMenu : Control
{
	public bool IsCapitalRisk = false;
	public bool isAutoAssign = false;
	public List<Player> Players = new List<Player>(); //A list of each of the players that have been added in the menu.
	public static Color[] PlayerColours; //A static array containing all of the colours that can be assigned to each player.

	private Button addPlayersButton;
	private VBoxContainer playersBox;

	public override void _Ready()
	{
		InitPlayerColours();
		this.GetNode<Button>("VBoxContainer/StartButton").Disabled = true; //Disable the start button until at least two players have been added.

		addPlayersButton = this.GetNode<Button>("VBoxContainer/AddPlayerBox/AddPlayersButton");
		playersBox = this.GetNode<VBoxContainer>("VBoxContainer/PlayersBox");
	}

	/// <summary>
	/// Initialises the static array that contains the colours that are assigned to each player.
	/// </summary>
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

	/// <summary>
	/// Called when the "Enable Capital Risk" checkbox is clicked in the menu.
	/// </summary>
	/// <param name="isPressed">Whether the checkbox is enabled or disabled.</param>
	public void _on_Capital_Risk_Enabled(bool isPressed)
	{
		IsCapitalRisk = isPressed;
	}

	/// <summary>
	/// Called when the "Enable Auto Assign" checkbox is clicked in the menu.
	/// </summary>
	/// <param name="isPressed">Whether the checkbox is enabled or disabled.</param>
	public void _on_Auto_Assign_Enabled(bool isPressed)
	{
		isAutoAssign = isPressed;
	}

	/// <summary>
	/// Adds players when AddPlayersButton is clicked in the main menu.
	/// </summary>
	public void _on_Add_Players_Button_Clicked()
	{
		//Enable the start button once there are at least two players.
		if(Players.Count + 1 >= 2) this.GetNode<Button>("VBoxContainer/StartButton").Disabled = false;
		
		//Only allow new players to be added if there are less than six players.
		if (Players.Count < 6)
		{
			//Initialise a new menu element for each player
			PackedScene playerScene = ResourceLoader.Load<PackedScene>("res://scenes/MainMenu_Player.tscn");
			MainMenuPlayer playerInstance = playerScene.Instantiate<MainMenuPlayer>();
			playersBox.AddChild(playerInstance);

			//Set up the player object itself.
			string playerName = "Player " + (Players.Count + 1).ToString();
			var newPlayer = new Player(playerName, PlayerColours[Players.Count], false);

			playerInstance.ThePlayer = newPlayer;
			playerInstance.playerNo = Players.Count + 1;
			Players.Add(playerInstance.ThePlayer);

			//Disable the add player button once six players have been added.
			if (Players.Count >= 6)
			{
				addPlayersButton.Disabled = true;
			}
		}
	}
	
	/// <summary>
	/// This is called when the start button is called to initialise the game-board and remove the main menu.
	/// </summary>
	public void _on_Start_Button_Clicked()
	{
		var gameScene = ResourceLoader.Load<PackedScene>("res://scenes/BoardRoot.tscn");
		var gameSceneInstance = gameScene.Instantiate<BoardRoot>();
		gameSceneInstance.Players = this.Players;
		
		//If capital risk or auto assign are enabled, enable those options in the new instance.
		if (IsCapitalRisk) gameSceneInstance.CapitalRisk = true;
		if (isAutoAssign) gameSceneInstance.AutoAssign = true;
		
		//Display the board and remove the main menu.
		GetTree().Root.AddChild(gameSceneInstance);
		GetTree().Root.RemoveChild(GetTree().Root.GetNode<Node>("MainMenu"));

	}
}
