using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

//TODO: Make a ReadOnlyGameState which cant be modified to use with functions
public class GameState
{
    //essentially if we need to wait for user input or some timed event
    //like an animation, set this to false
    //once waiting is over we can tick again
    public bool ReadyToTick { get; set; } = true;

    private readonly Dictionary<string, Actor> _currentActors = new Dictionary<string, Actor>();
    public IDictionary<string, Actor> CurrentActors => _currentActors;

    public Actor CurrentActor { get; set; }
    public bool ActorHasMove { get; set; }
    public bool ActorHasAction { get; set; }



    public Vector2Int SelectedTile { get; set; }

    private readonly List<Ability> _selectableAbilities = new List<Ability>();
    public IList<Ability> SelectableAbilities => _selectableAbilities;

    private readonly Dictionary<Ability, IEnumerable<Vector2Int>> _selectableAbilityTiles = new Dictionary<Ability, IEnumerable<Vector2Int>>();
    public IDictionary<Ability, IEnumerable<Vector2Int>> SelectableAbilityTiles => _selectableAbilityTiles;

    public Ability SelectedAbility { get; set; }

    public IList<Vector2Int> SelectableTiles  {
        get
        {
            return SelectableAbilityTiles[SelectedAbility].ToList();
        }
     }

    private readonly List<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>> _selectableAbilityTriggers
        = new List<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>>();
    public IList<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>>
        SelectableAbilityTriggers => _selectableAbilityTriggers;
    public AbilityTrigger SelectedAbilityTrigger { get; set; }

    private readonly List<KeyValuePair<string, Effect>> _activeEffects = new List<KeyValuePair<string, Effect>>();
    public IList<KeyValuePair<string, Effect>> ActiveEffects => _activeEffects;


    public Map Map {get; private set;}


    private readonly Tile[,] tiles;
    public Tile[,] Tiles => tiles;

    private readonly float[,] _costMap;

    public float[,] CostMap => _costMap;

    private GameMode _currentMode;
    public GameMode CurrentMode
    {
        get => _currentMode;
        set
        {
            PreviousMode = CurrentMode;
            _currentMode = value;
            Debug.Log($"Game Mode Updated from {Enum.GetName(typeof(GameMode), PreviousMode)} to " +
                $"{Enum.GetName(typeof(GameMode), CurrentMode)}");
        }
    }
    public GameMode PreviousMode { get; private set; }

    public System.Random Random { get; } = new System.Random();

    public int Round { get; set; }
    public GameState(Map map, Dictionary<string, Actor> startingUnits)
    {
        Map = map;
        tiles = map.Tiles;
        _costMap = MakeCostMap();
        _currentActors = startingUnits;
        foreach (KeyValuePair<string, Actor> idToActor in startingUnits)
        {
            IDictionary<string, Vector2Int> spawnPosDictmap = new Dictionary<string, Vector2Int>(Map.SpawnPositions);
            idToActor.Value.Spawn(idToActor.Key, ActorType.Player, spawnPosDictmap[idToActor.Key]);
        }

    }

    private float[,] MakeCostMap()
    {
        // TODO check that the dimensions are correct
        float[,] costMap = new float[tiles.GetLength(0), tiles.GetLength(1)];
        for (int i = 0; i < costMap.GetLength(0); i++)
        {
            for (int j = 0; j < costMap.GetLength(1); j++)
            {
                switch (tiles[i, j].Type)
                {
                    case TileType.Basic:
                        costMap[i, j] = 1;
                        break;
                }
            }
        }
        return costMap;
    }


    public static IList<Actor> FindActorsAtPositions(IList<Vector2Int> positions, GameState gameState)
    {
        return gameState.CurrentActors
            .Where((cu) => positions.Contains(cu.Value.Position))
            .Select((p) => p.Value)
            .ToList();
    }

}