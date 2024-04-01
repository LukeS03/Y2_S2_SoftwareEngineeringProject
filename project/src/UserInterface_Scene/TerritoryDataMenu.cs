using System;
using Godot;

namespace WorldConquest.UserInterface_Scene;

public partial class TerritoryDataMenu : VBoxContainer
{
    private Label territoryNameAndOwnerLabel;
    private Label connectionsHeaderLabel;
    private Label connectionsListLabel;
    private Label infantryLabel;
    private Button actionButton;

    public override void _Ready()
    {
        territoryNameAndOwnerLabel = GetNode<Label>("TitleContainer/TerritoryNameAndOwner");
        connectionsListLabel = GetNode<Label>("ConnectionsListLabel");
        this.actionButton = GetNode<Button>("ActionButton");
        this.infantryLabel = GetNode<Label>("InfantryLabel");
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
    public void View_Territory(Territory territory, GameStatus currentState, Player currentPlayer)
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

        this.infantryLabel.Text = "Tokens:" + territory.Tokens.ToString();

        this.actionButton.Disabled = false;

        switch (currentState)
        {
            case GameStatus.StartClaimTerritories:
                this.actionButton.Text = "Claim";
                if(territory.Owner != null) {this.actionButton.Disabled = true;}
                break;
            case GameStatus.StartFortifyTerritories:
                this.actionButton.Text = "Fortify";
                if(territory.Owner != currentPlayer) {this.actionButton.Disabled = true;}
                break;
            default:
                Console.Out.WriteLine("https://www.youtube.com/watch?v=j5BXUF_4PP0");
                break;
        }
    }
}