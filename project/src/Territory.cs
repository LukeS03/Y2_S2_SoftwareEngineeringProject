using System;
using System.Collections.Generic;
using Godot;

namespace WorldConquest;

public partial class Territory : Node2D
{
	/* Colour values for modulating when the territory is highlighted. */
	private static Color _unselectedTint = new Color(0f,0f,0f, 0f);
	private static Color _selectedTint   = new Color(255f, 255f, 255f, 0.8f);
	private static Color _hoveredTint    = new Color(128f, 128f, 128f, 0.5f);
	
	/* Attributes */
	public int Identifier;
	public string TerritoryName;
	public Continent TerritoryContinent;
	public List<Territory> Connections;
	public Sprite2D TerritorySprite;
	public Area2D CollisionArea;
	public new Player Owner;
	public int Tokens = 0;
	
	/* Signals */
	
	/// <summary>
	/// This signal is emitted when the territory is clicked on by the user. This signal is intended to be handled by
	/// the UserInterface instance. It sends a reference of itself so that the UI can display the territory data.
	/// </summary>
	[Signal]
	public delegate void TerritoryClickedEventHandler(Territory territory);

	/// <summary>
	/// Initialise the territory from an XML file. After all territories have been initialised into a
	/// list, you can then call the Initialise_Connections() method to initialise the territory
	/// connections.
	/// </summary>
	/// <param name="identifier"></param>
	/// <param name="territoryName"></param>
	/// <param name="continent"></param>
	/// <param name="texturePath"></param>
	/// <param name="position"></param>
	public void Initialise_Territory(int identifier, string territoryName, Continent continent, String texturePath, Vector2 position)
	{
		
		this.Identifier			= identifier;
		this.TerritoryName		= territoryName;
		this.TerritoryContinent = continent;
		//this.Position			= position;
		Owner = null;

		/* Loads the texture for the territory. */
		this.Name    = this.TerritoryName;
		this.TerritorySprite  = this.GetNode<Sprite2D>("TerritorySprite");
		var newTexture = ResourceLoader.Load<CompressedTexture2D>(texturePath);
		TerritorySprite.Texture = newTexture;
		
		/* Initialise collision. */
		this.CollisionArea = this.GetNode<Area2D>("CollisionArea");
		Initialise_Collision();
		
		/* Initialise mouse detection for territories. The signal for detecting clicks  */
		CollisionArea.MouseEntered += () => Mouse_Entered_Area();
		CollisionArea.MouseExited  += () => Mouse_Exited_Area();
	}

	/// <summary>
	/// Setup the <c>Territory</c> instances' connections after the instance has itself been initialised.
	/// </summary>
	/// <param name="territoryList">The game's overall territory list containing pointers to all the other territories.</param>
	/// <param name="connectionIndexes">A list of indexes for bordering territories.</param>
	public void Initialise_Connections(List<Territory> territoryList, List<int> connectionIndexes)
	{
		this.Connections = new List<Territory>();
		foreach (int i in connectionIndexes)
		{
			Connections.Add(territoryList[i]);
		}
	}

	/// <summary>
	/// Initialise collision detection after the Territory has been initialised.
	/// </summary>
	private void Initialise_Collision()
	{
		// Note: This code is derived from a Youtube tutorial by the channel 'The Shaggy Dev'. This video can be found 
		// at https://www.youtube.com/watch?v=Btk8IzhvaDo and their channel can be found at
		// https://www.youtube.com/@TheShaggyDev.
		var sprite = this.GetNode<Sprite2D>("TerritorySprite");

		var textureImage = sprite.Texture.GetImage();
		var bmpData = new Bitmap();
		bmpData.CreateFromImageAlpha(textureImage, 0.1f);
		var polygons = bmpData.OpaqueToPolygons(new Rect2I(Vector2I.Zero, textureImage.GetSize()));
		foreach(var polygon in polygons)
		{
			var collisionPolygon = new CollisionPolygon2D();
			collisionPolygon.Polygon = polygon;
			CollisionArea.AddChild(collisionPolygon);
		}
	}

	/// <summary>
	/// Run when the mouse enters the territory. Tints the territory.
	/// </summary>
	void Mouse_Entered_Area()
	{
		//this.Modulate = _hoveredTint;
	}

	/// <summary>
	/// Run when the mouse exits the territory. Removes the tint.
	/// </summary>
	void Mouse_Exited_Area()
	{
		//this.Modulate = _unselectedTint;
	}
	
	/// <summary>
	/// Run when an input event is detected on the territory. If that input event is a mouse-click, it gives the territory
	/// a tint.
	/// </summary>
	/// <param name="viewport"></param>
	/// <param name="event"></param>
	/// <param name="shape_idx"></param>
	private void _on_collision_area_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton mouseClick && mouseClick.Pressed)
		{
			// Using '1' directly for the left mouse button
			GD.Print("Clicked " + TerritoryName + "!");
			//Modulate = _selectedTint;
			EmitSignal(SignalName.TerritoryClicked, this);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="p"></param>
	public void SetTerritoryOwner(Player p)
	{
		if(this.Owner != null) this.Owner.ControlledTerritories.Remove(this);
		this.Tokens = 0;
		p.ControlledTerritories.Add(this);
		this.Owner = p;
		this.TerritorySprite.Modulate = p.PlayerColour;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
