using Godot;

namespace WorldConquest.UserInterface_Scene;

public partial class UserInterfacePlayer : VBoxContainer
{
	public Label PlayerNameLabel;
	public Label PlayerTokensLabel;
	public Player player;

	public Player Player
	{
		get { return this.player; }
		set
		{
			this.player = value;
			this.PlayerNameLabel.Text = this.player.Name;
			this.PlayerTokensLabel.Text = this.player.Tokens.ToString();
			this.PlayerNameLabel.AddThemeColorOverride("font_color",this.player.PlayerColour);
			this.PlayerTokensLabel.AddThemeColorOverride("font_color",this.player.PlayerColour);
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