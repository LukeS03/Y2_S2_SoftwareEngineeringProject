using Godot;

namespace WorldConquest.UserInterface_Scene;

public partial class TerritoryDataMenu : VBoxContainer
{
    private Label territoryNameAndOwnerLabel;
    private Label connectionsHeaderLabel;
    private Label connectionsListLabel;
    public GameStatus CurrentState;

    public override void _Ready()
    {
        territoryNameAndOwnerLabel = GetNode<Label>("TitleContainer/TerritoryNameAndOwner");
        connectionsListLabel = GetNode<Label>("ConnectionsListLabel");
    }

    public void _on_Hide_Menu_Button_Clicked()
    {
        this.Visible = false;
    }

    /// <summary>
    /// Function written by Al-Doukh
    /// Called when a territory is clicked on. Views details about that territory and prompts the user to take an action on it.
    /// </summary>
    /// <param name="territory"></param>
    /// <param name="currentState"></param>
    public void View_Territory(Territory territory, GameStatus currentState)
    {
        this.Visible = true;
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