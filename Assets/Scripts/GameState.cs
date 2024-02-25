using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    private readonly Dictionary<string, Actor> currentUnits = new Dictionary<string, Actor>();
    public IDictionary<string, Actor> CurrentUnits => currentUnits;

    private readonly Tile[,] tiles;
    public Tile[,] Tiles => tiles;

    private readonly float[,] costMap;

    public float[,] CostMap => costMap;

    private readonly List<string> turnOrder = new List<string>();
    public IList<string> TurnOrder => turnOrder;

    private readonly List<Vector2Int> selectableTiles = new List<Vector2Int>();
    public IList<Vector2Int> SelectableTiles => selectableTiles;

    public Ability SelectedAbility { get; set; }

    public Vector2Int SelectedTile { get; set; }

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

    public System.Random Random { get; } = new System.Random();
    public GameState(Tile[,] map)
    {
        tiles = map;
        costMap = MakeCostMap();
    }

    private float[,] MakeCostMap()
    {
        // TODO check that the dimensions are correct
        float[,] costMap = new float[tiles.GetLength(0), tiles.GetLength(1)];
        for (int i = 0; i < costMap.GetLength(0); i++)
        {
            for (int j = 0; j < costMap.GetLength(1); j++)
            {
                switch (tiles[i,j])
                {
                    case Tile.Basic:
                        costMap[i, j] = 1;
                        break;
                }
            }
        }
        return costMap;
    }


    public static IList<Actor> FindActorsAtPositions(IList<Vector2Int> positions, GameState gameState)
    {
        return gameState.CurrentUnits
            .Where((cu) => positions.Contains(cu.Value.Position))
            .Select((p) => p.Value)
            .ToList();
    }

}