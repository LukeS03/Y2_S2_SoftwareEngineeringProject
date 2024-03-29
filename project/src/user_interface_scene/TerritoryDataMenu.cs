using Godot;

namespace WorldConquest.user_interface_scene;

public partial class TerritoryDataMenu : VBoxContainer
{
    private Label territoryNameAndOwnerLabel;
    private Label connectionsHeaderLabel;
    private Label connectionsListLabel;

    public override void _Ready()
    {
        territoryNameAndOwnerLabel = GetNode<Label>("TerritoryNameAndOwner");
        connectionsListLabel = GetNode<Label>("ConnectionsListLabel");
    }

    public void View_Territory(Territory territory)
    {
        GD.Print("View_Territory called for: " + territory.TerritoryName);
        string NewTerritoryNameAndOwnerLabelText = territory.TerritoryName + " - Owner: " + (territory.Owner != null ? territory.Owner.Name : "None");
        territoryNameAndOwnerLabel.Text = NewTerritoryNameAndOwnerLabelText;

        string connectionsNames = "";
        foreach (var connectedTerritory in territory.Connections)
        {
            connectionsNames += connectedTerritory.TerritoryName + ", ";
        }
        connectionsListLabel.Text = connectionsNames.TrimEnd(new char[] { ',', ' ' });
    }
}