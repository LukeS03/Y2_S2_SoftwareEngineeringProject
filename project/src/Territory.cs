using System;
using System.Collections.Generic;
using Godot;

namespace WorldConquest;

public partial class Territory : Node2D
{
	public int Identifier;
	public string TerritoryName;
	public Continent TerritoryContinent;
	public List<PackedScene> Connections;
	public Sprite2D TerritorySprite;

	public void Initialise_Territory(int identifier, string territoryName, Continent continent, String texturePath, Vector2 position)
	{
		this.Identifier			= identifier;
		this.TerritoryName		= territoryName;
		this.TerritoryContinent = continent;
		this.Position			= position;

		this.GetNode<Sprite2D>("TerritorySprite");
		var texty = ResourceLoader.Load<Texture2D>(texturePath);
	}

	/// <summary>
	/// Setup the <c>Territory</c> instances' connections after the instance has itself been initialised.
	/// </summary>
	/// <param name="territoryList">The game's overall territory list containing pointers to all the other territories.</param>
	/// <param name="connectionIndexes">A list of indexes for bordering territories.</param>
	public void Initialise_Connections(List<Territory> territoryList, List<int> connectionIndexes)
	{
		return;
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
