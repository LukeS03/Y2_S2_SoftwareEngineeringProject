# Design Document for Sprint 2

This document is intended to give an outline for the design of the World Conquest board-game in it's second sprint.

## Sprint goals
* The game should start by showing a main menu, allowing the players to input settings such as what mode they want to play (e.g. normal World Conquest or Capital World Conquest), how many players want to play, and how many AI agents to include.
* Once ingame, the players should be able to choose their territories.
* There should be an interface which shows whose turn it is, and describes what actions have just been carried out.
* Allow players to select territories, adding user interface elements on high-light such as owning player, connections, units, etc.
* Implement the beginnings of the Player class

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

## Game logic pseudocode:
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
				End the StartFortifyTerritories phase.
				Set player index pointer to -1
				TurnTransition()
			
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

## List of Signals
* **Territory**
    * `TerritoryClicked(Territory territory)`: Indicates that a Territory has been clicked on the board. This signal is intended to be handled by the BoardRoot. Once this signal is recieved by the BoardRoot, the BoardRoot will change the UserInterface attribute `CurrentTerritory`. A setter method will then set the current 
* **UserInterface**
    * `DataMenuAction(Territory territory)`: Indicates that a territory has been selected in the TerritoryDataMenu.

## Miscellaneous Design Notes
* This software will follow the principle of __**call down, signal up**__. That is so as to say, if a child node wants to interact with a parent node, it should communicate via Godot signals. If a parent node wants to interact with a child node, it should communicate via a direct method call to that child.

## Design Notes & Issues
* Originally the `TerritoryClicked(Territory territory)` signal was to be handled directly by the User Interface. This was not possible, because it would have required finding each territory within the UserInterface class, and connecting each individual signal, which would have been problematic. Instead, the `TerritoryClicked(Territory territory)` signal will be handled by the BoardRoot, which will then set the `CurrentTerritory` attribute of the UserInterface. A setter method will be set up to update the Territory Data menu and change it's position relative to the mouse.
* There is a bug where hovering territories seems to make them invisible.
* Originally a signal called `InitialisedTerritories(List<Territory> territories)` was to be emitted once the continents, territories and connections were all initialised. This signal was redundant as Godot initialises children first before their parent nodes. 
* There were originally going to be two signals sent by the UserInterface called `StartPlayerClaimedTerritory(Territory territory)` and `StartPlayerFortifiedTerritory(Territory territory, int tokens)`. They were scapped in favour of the signal `DataMenuActionEventHandler`.