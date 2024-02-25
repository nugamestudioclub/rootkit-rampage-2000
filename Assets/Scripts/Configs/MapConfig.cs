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

}