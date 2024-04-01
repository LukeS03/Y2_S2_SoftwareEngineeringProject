using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldConquest;

public partial class World : Node2D
{
	public List<Continent> Continents;
	public List<Territory> Territories;

	[Signal]
	public delegate void InitialisedTerritoriesEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//initialise directory paths
		string configDir = "res://config/";
		string continentsFile = configDir + "/Continents.xml";
		string territoriesFile = configDir + "/Territories.xml";
		string connectionsFile = configDir + "/Connections.xml";
		
		//initialise the board.
		InitialiseContinents(continentsFile);
		System.Console.WriteLine("finished initialising continents.");
		InitialiseTerritories(territoriesFile);
		System.Console.WriteLine("Finished initialising territories.");
		InitialiseConnections(connectionsFile);
		System.Console.WriteLine("Finished initialising connections.");
		//EmitSignal(SignalName.InitialisedTerritories);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//TODO: Set up error detection and handling for these methods.
	
	/// <summary>
	/// Initialise continents from path. Must be called before initialising the 
	/// </summary>
	/// <param name="continentPath"></param>
	/// <exception cref="Exception"></exception>
	private void InitialiseContinents(string continentPath)
	{
		
		this.Continents = new List<Continent>();
		
		//This parsing code is derived from https://docs.godotengine.org/en/stable/classes/class_xmlparser.html
		var parser = new XmlParser();
		parser.Open(continentPath);
		parser.Read();
		if (parser.GetNodeName() != "continentsRoot") {throw new Exception("XML Error: continentsRoot not found.");}
		while (parser.Read() != Error.FileEof)
		{
			//Iterate through each 'node' after the continentsRoot node.
			//In this instance, "node" refers to: opening tags, data enclosed in tags, closing tags.

			if (parser.GetNodeName() == "continentsRoot" && parser.GetNodeType() == XmlParser.NodeType.ElementEnd)
			{
				//break the loop once you reach the closing node for continentsRoot.
				break;
			}
			else if (parser.GetNodeName() == "continent" && parser.GetNodeType() == XmlParser.NodeType.Element) 
			{
				//Create a new continent for each 'continent' entity in the XML file.
				//TODO: Throw an error if Continent.Identifier value is not equal to 
				var attributeBuffer = new List<string>();
				for (int id = 0; id < parser.GetAttributeCount(); id++)
				{
					attributeBuffer.Add(parser.GetAttributeValue(id));
				}

				Continent continentBuffer = new Continent(int.Parse(attributeBuffer[0]), attributeBuffer[1], int.Parse(attributeBuffer[2]));
				
				this.Continents.Add(continentBuffer);
			}
			

		}
	}

	/// <summary>
	/// Initialises territories once continents have been initialised.
	/// </summary>
	/// <param name="territoryPath"></param>
	private void InitialiseTerritories(string territoryPath)
	{
		
		this.Territories = new List<Territory>();
		var parser = new XmlParser();
		parser.Open(territoryPath);
		while (parser.Read() != Error.FileEof)
		{
			if (parser.GetNodeName() == "territory" && parser.GetNodeType() == XmlParser.NodeType.Element)
			{
				var attrBuffer = new List<string>();
				for (int id = 0; id < parser.GetAttributeCount(); id++)
				{
					attrBuffer.Add(parser.GetAttributeValue(id));
				}

				int idBuf         = int.Parse(attrBuffer[0]);
				string nameBuf    = attrBuffer[1];
				int contIdBuf     = int.Parse(attrBuffer[2]);
				string spritePath = attrBuffer[3];
				int xBuf          = int.Parse(attrBuffer[4]);
				int yBuf          = int.Parse(attrBuffer[5]);

				Vector2 position = new Vector2(xBuf, yBuf);
				
				var territorySceneResource = ResourceLoader.Load<PackedScene>("res://scenes/Territory.tscn").Instantiate();
				this.AddChild(territorySceneResource);
				((Territory)territorySceneResource).Initialise_Territory(idBuf, nameBuf, Continents[contIdBuf], spritePath, position);
				this.Territories.Add((Territory)territorySceneResource);
				
			}
		}
	}

	/// <summary>
	/// Initialises connections between territories once the continents and territories have been initialised.
	/// </summary>
	/// <param name="connectionsPath"></param>
	private void InitialiseConnections(string connectionsPath)
	{
		var parser = new XmlParser();
		parser.Open(connectionsPath);
		int territoryIndex = 0;
		while (parser.Read() != Error.FileEof)
		{
			//Read through each "Connections" node.
			if (parser.GetNodeName() == "Connections" && parser.GetNodeType() == XmlParser.NodeType.Element)
			{
				List<int> connectionIndexes = new List<int>();
				
				//For each "Connections" node, iterate through each of it's child nodes until the parser reaches the 
				//closing XML tag.
				while (!(parser.GetNodeName() == "Connections" && parser.GetNodeType() == XmlParser.NodeType.ElementEnd))
				{
					//If the current tag is a text element, add it to the list of connection indexes.
					if (parser.GetNodeType() == XmlParser.NodeType.Element && parser.GetNodeName() == "Connection")
					{
						connectionIndexes.Add(int.Parse(parser.GetAttributeValue(0)));
					}
					parser.Read();
				}
				Territories[territoryIndex].Initialise_Connections(this.Territories, connectionIndexes);
				territoryIndex++;
			}
		}
	}

	/// <summary>
	/// Returns a list of territories that have no owner set.
	/// </summary>
	/// <returns>A List of unclaimed territories.</returns>
	public List<Territory> GetUnclaimedTerritories()
	{
		List<Territory> unclaimedTerritories = new List<Territory>();
		foreach (var t in Territories)
		{
			if (t.Owner == null) unclaimedTerritories.Add(t);
		}

		return unclaimedTerritories;
	}
}
