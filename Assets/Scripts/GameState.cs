using System;
using System.Collections.Generic;

public class GameState
{
    private readonly Dictionary<string, Actor> currentUnits = new Dictionary<string, Actor>();
    public IDictionary<string, Actor> CurrentUnits => currentUnits;

    private readonly Tile[][] tiles;
    public Tile[][] Tiles => tiles;

    private readonly float[,] costMap;

    public float[,] CostMap => costMap;

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

    public Random Random { get; } = new Random();
    public GameState(Tile[][] map)
    {
        tiles = map;
        costMap = MakeCostMap();
    }

    private float[,] MakeCostMap()
    {
        // TODO check that the dimensions are correct
        float[,] costMap = new float[tiles.Length, tiles[0].Length];
        for (int i = 0; i < costMap.GetLength(0); i++)
        {
            for (int j = 0; j < costMap.GetLength(1); j++)
            {
                switch (tiles[i][j])
                {
                    case Tile.Basic:
                        costMap[i, j] = 1;
                        break;
                }
            }
        }
        return costMap;
    }



}