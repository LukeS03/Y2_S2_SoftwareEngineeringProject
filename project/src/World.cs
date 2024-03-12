using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldConquest;

public partial class World : Node2D
{
	public List<Continent> Continents;

	public List<PackedScene> Territories;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string configDir = "res://config/";
		string continentsFile = configDir + "/Continents.xml";
		string territoriesFile = configDir + "/Territories.xml";
		InitialiseContinents(continentsFile);
		System.Console.WriteLine("finished parsing continents.");
		InitialiseTerritories(territoriesFile);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void InitialiseContinents(string continentPath)
	{
		
		this.Continents = new List<Continent>();
		
		//This parsing code has been acquired from https://docs.godotengine.org/en/stable/classes/class_xmlparser.html
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
		return;
	}

	private void InitialiseTerritories(string territoryPath)
	{
		
		this.Territories = new List<PackedScene>();
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
				
			}
		}
		return;
	}
}
