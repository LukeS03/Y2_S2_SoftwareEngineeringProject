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
    class Root {
        <<Node2D>>
        + Enum GameMode
        + List~Player~ Players
    }

    class UserInterface {
        <<Scene>>
    }

    class Player {
        + Enum isCPU
        + List~Territory~ controlledTerritories
        + String Name
        + Color PlayerColour
    }

    class DebugUI {
        <<Scene>>
    }

    class World {
        <<Node2D>>
        + List~Territory~ territories
        + List~Continent~ continents
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
        + Initialise(int identifier, string name, Continent continent, string texturePath, Vector2 position)
        + InitialiseConnections(List~Territory~ territoryList, List~int~ connectionIndexes)
    }

    class Continent {
        + int tokens
        + string name
        + Continent(int id, string name, int tokens)
    }

    Root <|-- UserInterface
    Root <|-- DebugUI
    Root <|-- Player
    Root <|-- World
    World <.. Territory
    Territory <.. Continent
```

## Class Diagram for User Interface
```
```

## Class Diagram for Main Menu

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
1. When a territory is moused over, it is brightened slightly. This is handled node-side by the Territory.
2. When a Territory is then clicked, it darkens itself and a signal is sent.
3. This signal is handled by the UserInterface.

## Miscellaneous Design Notes
* This software will follow the principle of __**call down, signal up**__. That is so as to say, if a child node wants to interact with a parent node, it should communicate via Godot signals. If a parent node wants to interact with a child node, it should communicate via a direct method call to that child.

