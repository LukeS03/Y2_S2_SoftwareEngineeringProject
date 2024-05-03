using System;
using Godot;

namespace WorldConquest.UserInterface_Scene;

/// <summary>
/// This class controls the menu that opens to the player when the player clicks on any of the territories. It allows
/// the players to carry out various actions on the selected territory depending on the current turn stage of the game,
/// such as claiming or fortifying that territory.
/// </summary>
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

    /// <summary>
    /// Hides the menu when the player clicks the little "X" button in the top-left of the menu.
    /// </summary>
    public void _on_Hide_Menu_Button_Clicked()
    {
        this.Visible = false;
    }

    /// <summary>
    /// Function written by Al-Doukh.
    /// Called when a territory is clicked on. Views details about that territory and prompts the user to take an action on it.
    /// </summary>
    /// <param name="territory"></param>
    /// <param name="currentState"></param>
    public void View_Territory(Territory territory, GameStatus currentState, Player currentPlayer)
    {
        this.Visible = true;
        //Generate heading text for the territory, containing the owner's name.
        string NewTerritoryNameAndOwnerLabelText = territory.TerritoryName + " - Owner: " + (territory.Owner != null ? territory.Owner.Name : "None");
        territoryNameAndOwnerLabel.Text = NewTerritoryNameAndOwnerLabelText;

        //Display each of the territory's connections.
        string connectionsNames = "";
        foreach (var connectedTerritory in territory.Connections)
        {
            connectionsNames += connectedTerritory.TerritoryName + ", ";
        }
        connectionsListLabel.Text = connectionsNames.TrimEnd(new char[] { ',', ' ' });

        //Display how many tokens are placed on the selected territory.
        this.infantryLabel.Text = "Tokens:" + territory.Tokens.ToString();

        this.actionButton.Disabled = false;

        //Prompt the player to carry out different actions depending on the current turn stage, and if necessary, 
        //disable the action button so as to make sure that player's actions follow the rules of the game.
        switch (currentState)
        {
            case GameStatus.StartClaimTerritories:
                this.actionButton.Text = "Claim";
                if(territory.Owner != null) this.actionButton.Disabled = true;
                break;
            case GameStatus.StartFortifyTerritories:
                this.actionButton.Text = "Fortify";
                if(territory.Owner != currentPlayer) this.actionButton.Disabled = true;
                break;
            case GameStatus.FortifyTerritoriesStage:
                this.actionButton.Text = "Fortify";
                if (territory.Owner != currentPlayer) this.actionButton.Disabled = true;
                break;
        }
    }
}