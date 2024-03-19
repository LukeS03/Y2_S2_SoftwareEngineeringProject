using System;
using System.Collections.Generic;
using Godot;

namespace WorldConquest;

public partial class Territory : Node2D
{
	public int Identifier;
	public string TerritoryName;
	public Continent TerritoryContinent;
	public List<Territory> Connections;
	public Sprite2D TerritorySprite;
	public Area2D CollisionArea;

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

		this.Name    = this.TerritoryName;
		var theSprite  = this.GetNode<Sprite2D>("TerritorySprite");
		var newTexture = ResourceLoader.Load<CompressedTexture2D>(texturePath);
		theSprite.Texture = newTexture;
		
		this.CollisionArea = this.GetNode<Area2D>("CollisionArea");
		Initialise_Collision();
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

	void Mouse_Entered_Area()
	{
		Console.Out.WriteLine("Hello from " + TerritoryName + "!");
	}

	void Mouse_Exited_Area()
	{
		
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
