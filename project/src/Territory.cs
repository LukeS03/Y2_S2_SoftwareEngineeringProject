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
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
