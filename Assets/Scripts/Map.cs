using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public readonly struct Map
{
    public Map(string name, int width, int height, Tile[,] tiles, 
        IEnumerable<KeyValuePair<string, Vector2Int>> spawnPositions)
    {
        Name = name;
        Width = width;
        Height = height;
        _tiles = tiles;
        _spawnPositions = spawnPositions.ToList();
    }

    public readonly string Name { get; }
    public readonly int Width { get; }

    public readonly int Height { get; }

    private readonly Tile[,] _tiles;
    public Tile[,] Tiles => _tiles;

    private readonly List<KeyValuePair<string, Vector2Int>> _spawnPositions;
    public IEnumerable<KeyValuePair<string, Vector2Int>> SpawnPositions => _spawnPositions;


}