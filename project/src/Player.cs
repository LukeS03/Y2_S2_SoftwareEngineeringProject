using System.Collections.Generic;
using Godot;

namespace WorldConquest;

public class Player
{
    public bool            IsCPU;
    public List<Territory> ControlledTerritories;
    public int             Tokens;
    public string          Name;
    public Color           PlayerColour;

    /// <summary>
    /// Initialises a Player instance.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="playerColour"></param>
    /// <param name="isCPU"></param>
    public Player(string name, Color playerColour, bool isCPU)
    {
        this.Name = name;
        this.PlayerColour = playerColour;
        this.IsCPU = isCPU;

        this.Tokens = 0;
        this.ControlledTerritories = new List<Territory>();
    }

    public static bool AnyPlayersHaveRemainingTokens(List<Player> players)
    {
        foreach (var p in players)
        {
            if (p.Tokens > 0) return true;
        }

        return false;
    }
}