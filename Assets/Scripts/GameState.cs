using System.Collections.Generic;

public class GameState
{
    private readonly Dictionary<string, Actor> currentUnits = new Dictionary<string, Actor>();
    public IDictionary<string, Actor> CurrentUnits => currentUnits;

    private readonly Tile[][] tiles;
    public Tile[][] Tiles => tiles;


    private readonly List<string> turnOrder = new List<string>();
    public IList<string> TurnOrder => turnOrder;

    private GameMode currentMode;
    public GameMode CurrentMode
    {
        get => currentMode;
        set
        {
            PreviousMode = CurrentMode;
            currentMode = value;
        }
    }
    public GameMode PreviousMode { get; private set; }
    public GameState(Tile[][] map)
    {
        tiles = map;
    }

}