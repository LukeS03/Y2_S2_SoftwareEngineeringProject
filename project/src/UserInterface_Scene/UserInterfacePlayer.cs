using Godot;

namespace WorldConquest.UserInterface_Scene;

public partial class UserInterfacePlayer : VBoxContainer
{
	private Label PlayerNameLabel;
	private Label PlayerTokensLabel;
	private Player player;

	public Player Player
	{
		get { return this.player; }
		set
		{
			this.player = value;
			this.PlayerNameLabel.Text = this.player.Name;
			this.PlayerTokensLabel.Text = this.player.Tokens.ToString();
			this.PlayerNameLabel.AddThemeColorOverride("1",this.player.PlayerColour);
			this.PlayerTokensLabel.AddThemeColorOverride("1",this.player.PlayerColour);
		}
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.PlayerNameLabel = this.GetNode<Label>("PlayerLabel");
		this.PlayerTokensLabel = this.GetNode<Label>("Tokens");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}