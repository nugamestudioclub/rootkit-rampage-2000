using System.Collections.Generic;
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
    public List<Vector2Int> PlayerPositions { get; private set; }

    [field: SerializeField]
    public List<ActorPositionData> EnemyPositions { get; private set; }

    //sprite

    public Tile[,] Generate()
    {
        Tile[,] map = new Tile[Width,Height];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = new Tile(TileType.Basic, TileUIState.None);
            }
        }

        return map;
    }

    public GameState GenerateNextGameState(List<Actor> players)
    {
        GameState state = new GameState(Generate());
        for (int i = 0; i < PlayerPositions.Count && i < players.Count; i++)
        {
            players[i].Spawn(players[i].Id, ActorType.Player, PlayerPositions[i]);
            state.CurrentUnits.Add(players[i].Id, players[i]);
        }
        for (int i = 0; i <  EnemyPositions.Count; i++)
        {
            ActorPositionData enemyData = EnemyPositions[i];
            Actor enemyActor = enemyData.ActorConfig.Generate();
            enemyActor.Spawn(enemyActor.Name + i, ActorType.AI, enemyData.Position);
            state.CurrentUnits.Add(enemyActor.Id, enemyActor);
        }
        return state;
    }

}