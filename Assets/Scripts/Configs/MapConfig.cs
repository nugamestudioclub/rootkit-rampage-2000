using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(MapConfig),
    menuName = "ScriptableObjects/" + nameof(MapConfig))
]
public class MapConfig : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int Width { get; private set; }

    [field: SerializeField]
    public int Height { get; private set; }

    [field: SerializeField]
    public TextAsset MapData { get; private set; }


    [field: SerializeField]
    public List<ActorPositionData> PlayerSpawns { get; private set; }

    [field: SerializeField]
    public List<ActorPositionData> EnemySpawns { get; private set; }

    //sprite

    public (Map, IEnumerable<KeyValuePair<string, Actor>>) Generate()
    {
        List<ActorPositionData> spawnPositionData = new List<ActorPositionData>();
        foreach (ActorPositionData spawns in PlayerSpawns)
        {
            spawnPositionData.Add(spawns);
        }
        foreach (ActorPositionData spawns in EnemySpawns)
        {
            spawnPositionData.Add(spawns);
        }
        int idIndex = 0;
        List<KeyValuePair<string, Actor>> idsToActors = new List<KeyValuePair<string, Actor>>();
        List<KeyValuePair<string, Vector2Int>> idsToSpawnsPositions = new List<KeyValuePair<string, Vector2Int>>();
        foreach (ActorPositionData spawns in spawnPositionData)
        {
            string actorId = $"{ idIndex++ }{ spawns.ActorConfig.Name}";
            idsToActors.Add(new KeyValuePair<string, Actor>(actorId, spawns.ActorConfig.Generate()));
            idsToSpawnsPositions.Add(new KeyValuePair<string, Vector2Int>(actorId, spawns.Position));
        }
        return (new Map(Name, Width, Height, GenerateTiles(), idsToSpawnsPositions.ToList()), 
            idsToActors.ToList());
    }
    public Tile[,] GenerateTiles()
    {
        Tile[,] map = new Tile[Width, Height];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = new Tile(TileType.Basic, TileUIState.None);
            }
        }

        return map;
    }
    
}