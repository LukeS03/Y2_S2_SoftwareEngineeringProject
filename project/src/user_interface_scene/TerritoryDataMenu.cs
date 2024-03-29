using Godot;
using System.Collections.Generic;
using WorldConquest;

public partial class TerritoryDataMenu : VBoxContainer
{
    private Label territoryNameAndOwnerLabel;
    private Label connectionsHeaderLabel;
    private Label connectionsListLabel;

    public override void _Ready()
    {
        territoryNameAndOwnerLabel = GetNode<Label>("PathToTerritoryNameAndOwnerLabel");
        connectionsHeaderLabel = GetNode<Label>("PathToConnectionsHeaderLabel");
        connectionsListLabel = GetNode<Label>("PathToConnectionsListLabel");

        var worldNode = GetNode<World>("/BoardRoot/World");
        var territoriesList = worldNode.Territories;

        foreach (var territory in territoriesList)
        {
            var callable = new Callable(this, nameof(View_Territory));
            territory.Connect(nameof(Territory.TerritoryClicked), callable);
        }
    }

    private void View_Territory(Territory territory)
    {
        GD.Print("View_Territory called for: " + territory.TerritoryName);
        territoryNameAndOwnerLabel.Text = territory.TerritoryName + " - Owner: " + (territory.Owner != null ? territory.Owner.Name : "None");
        connectionsHeaderLabel.Text = "Connections (" + territory.Connections.Count + "):";

        string connectionsNames = "";
        foreach (var connectedTerritory in territory.Connections)
        {
            connectionsNames += connectedTerritory.TerritoryName + ", ";
        }
        connectionsListLabel.Text = connectionsNames.TrimEnd(new char[] { ',', ' ' });
    }
}
