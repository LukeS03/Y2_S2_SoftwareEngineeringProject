namespace WorldConquest;

public class Continent
{
    public string Name;
    public int Tokens;
    public int Identifier;

    public Continent(int id, string name, int tokens)
    {
        this.Identifier = id;
        this.Tokens = tokens;
        this.Name = name;
    }
}