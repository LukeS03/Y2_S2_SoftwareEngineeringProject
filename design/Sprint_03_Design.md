# Design Document for Sprint 3
This document is intended to describe the design for the third Sprint of the development process for the World Conquest video-game. The goal of this sprint include:
* Implementing the first stage of a player's turn, during which they can place troops.
* Implementing a system that gives players troop bonuses depending on if they control any continents.
* Add a GUI element to show how many troops are on each territory without having to click on said territory.
* Polishing some of the previously implemented features:
    * Fixing a bug that seems to cause territory interconnections to be loaded incorrectly.
    * Loading the game background from file by the World class' initialisation function rather than being defined in Godot itself.
    * Updating the ingame background image to show connections over sea.
* Completing a few tasks which were originally slated to be carried out in the second sprint, but were carried forwards.
    * Merging the development of the Main Menu and using it to initialise and launch a game.
    * Designing and implementing a system for controlling AI players.
    * Adding support to the UI to deal with AI player turns.
* Removing inconsistencies between the design document and the actual code itself (e.g. misnamed variables)

## Class Diagram for Main Game Scene
```mermaid
classDiagram
    class BoardRoot {
        <<Node2D>>
        + Enum GameMode
        + List~Player~ Players
        + Player CurrentTurn
        + UserInterface Gui
        + TerritoryClickedTellUi()
        - SelectNextPlayer()
        - SetTerritorySignals()
        - StartClaimTerritories()
        - StartFortifyTerritories()
        - DataMenuActionSignalReceived()
        - TurnTransition()
        - StartPhaseClaimTerritories()
        - StartPhaseFortifyTerritories
    }

    class UserInterface {
        <<Scene>>
        + List~Player~ Players
        + Player CurrentTurn
        + Territory SelectedTerritory
        + Enum CurrentMode currentMode
        + Initialise_Interface(List~Player~ Players)
    }

    class GameStatus {
        <<Enumeration>>
        StartClaimTerritories
        StartFortifyTerritories
    }

    class Player {
        + Boolean IsCPU
        + List~Territory~ ControlledTerritories
        + String Name
        + Color PlayerColour
        + int Tokens
    }

    class World {
        <<Node2D>>
        + List~Territory~ territories
        + List~Continent~ continents
        + List~Territory~ GetUnclaimedTerritories()
        - InitialiseContinents(String continentPath)
        - InitialiseTerritories(String territoryPath)
        - InitialiseConnections(String connectionsPath)
    }


    class Territory {
        <<Scene>>
        + int identifier
        + Continent continent
        + List~Territory~ connections
        + Player Owner
        + int UnitTokens
        + Initialise(int identifier, string name, Continent continent, string texturePath, Vector2 position)
        + InitialiseConnections(List~Territory~ territoryList, List~int~ connectionIndexes)
    }

    class Continent {
        + int tokens
        + string name
        + Continent(int id, string name, int tokens)
    }

    BoardRoot <|-- UserInterface
    BoardRoot <|-- Player
    BoardRoot <|-- World
    World <.. Territory
    World <.. Continent
    Territory <.. Continent
    GameStatus --|> UserInterface
    GameStatus--|>BoardRoot 
```

## List of Signals
* **Territory**
    * `TerritoryClicked(Territory territory)`: Indicates that a Territory has been clicked on the board. This signal is intended to be handled by the BoardRoot. Once this signal is recieved by the BoardRoot, the BoardRoot will change the UserInterface attribute `CurrentTerritory`. A setter method will then set the current 
* **UserInterface**
    * `DataMenuAction(Territory territory)`: Indicates that a territory has been selected in the TerritoryDataMenu.
    * `SpinBoxInputConfirmed(int numInput)`: Sent by UserInterfaceNumberInput. `numInput` is the number that the user inputted into the box. 

## Game logic pseudocode:
The following pseudocode describes some of the functions within the `BoardRoot` class which are used to transition between different turn stages and different players. 
```
Ready:
	Set current GameState to StartClaimTerritoriesStage
	Set the the first player using SetCurrentPlayer()
	Update the GUI to reflect the current player and stage.

DataMenuActionSignalReceived: (Called when the button in the DataMenu is clicked.)
	If in StartClaimTerritories phase:
		StartPhaseClaimTerritories()
	If in StartFortifyTerritories phase:
		StartPhaseFortifyTerritories()

EndTurnStageButton: (Called when the end turn stage button is clicked)
    if in TurnPlacementStage:


TurnTransition:
	If in StartClaimTerritories Phase:
		If there are any unclaimed territories left (i.e. if World.GetUnclaimedTerritories() returns a non-empty list):
			Select a new player to claim a territory using SetCurrentPlayer()
		Else:
			Set phase to StartFortifyTerritories
			TurnTransition()
	if in StartFortifyTerritories:
		If all territories are claimed:
			If there is at least one player with remaining tokens:
				While the currently selected player has no tokens:
					Select the next player to fortify a territory using SetCurrentPlayer()
			If none of the players have any tokens left:
				Set phase to TurnPlacementStage.
				Set player index pointer to -1
				TurnTransition()
    If in TurnPlacementStage:
        /* Note: This turn will only end once the player clicks "EndStage", so unlike the previous two stages, the logic for transitioning to the next stage will be handled by the event handler for when the EndTurn button is pressed.*/
        Select the next player using SetCurrentPlayer()
        Tally up how many extra troops the player gets that turn:
            3 + continent bonuses for any continents returned by World.PlayerOwnsTerritories(Player p)
        Add the total extra troops for this turn to the player's Tokens.
        Update the UI to reflect the current turn stage and active player.

			
StartPhaseClaimTerritories:
	Get the currently selected territory from the GUI.
	Assign the claimed territory to the player.
	Add the territory to the player's internal list of territories.
	TurnTransition()
	

StartPhaseFortifyTerritories:
	Get the currently selected territory from the GUI.
	Assign said territory one extra infantry token.
	Take one infantry token from the player.
	TurnTransition()


List<Territory> World.GetUnclaimedTerritories:
	Return a list of territories that have no owner.

List<Continent> World.PlayerOwnsTerritories(Player p):
    Returns a list of continents in which the player specified owns all territories.
```

# Designs from Previous Sprints
## Process for Selecting a Territory
```mermaid
flowchart TB
    a["The player agent clicks on a territory."]
    b["The territory sends a signal `TerritoryClicked(Territory territory)` to the BoardRoot."]
    c["BoardRoot handles the signal, passing the clicked territory to the UserInterface's `OnTerritoryClicked` method."]
    d["The UserInterface passes the clicked Territory object and the current game state to the TerritoryDataMenu."]
    e["The TerritoryDataMenu shows information about the clicked territory."]

    a-->b-->c-->d-->e
```

## Process for player interaction in the Claim Stage.
```mermaid
flowchart TB
    subgraph "BoardRoot"
        a["BoardRoot selects the next player"]
        b["BoardRoot selects the next turn."]
        c["BoardRoot sets the UserInterface's CurrentTurn and CurrentMode values."]
        g["The BoardRoot receives the signal `DataActionEvent` from the UserInterface."]
        h["The BoardRoot assigns the territory to that user, or if it is in the StartPhaseFortifyTerritories stage, it adds one troop to that territory."]
    end
    subgraph "UserInterface"
        d["The UserInterface prompts the player to make a move. The button in the TerritoryDataMenu is updated with respect to the current turn."]
        f["The UserInterface sends a signal `DataActionEvent`."]
    end
    subgraph "user agent"
        e["The player selects a territory and clicks the button in the Territory Data menu."]
    end

    a --> b --> c --> d --> e --> f --> g --> h --> a
```

## Flow Chart for Territory Allocation
```mermaid
flowchart TB
    A["Select the first player."]
    Aalt["Select the next player."]
    B["Call UI's SetCurrentPlayer() method to set chosen player"]
    Opt1{"Have all territories been claimed?"}

    DLeft["Let player select an unclaimed to place one unit on."]
    DRight["Let the player select a territory that they own to place a unit on."]

    END{"Do the players have any armies left?"}

    END -->|"No"|Aalt
    A --> B
    Aalt --> B
    B --> Opt1
    Opt1 -->|"No"|DLeft
    Opt1 -->|"Yes"|DRight
    DLeft --> END
    DRight --> END
```
