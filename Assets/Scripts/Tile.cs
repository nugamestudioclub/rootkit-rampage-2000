public readonly struct Tile
{
    public Tile(TileType type, TileUIState state)
    {
        _type = type;
        _state = state;
    }

    private readonly TileType _type;
    private readonly TileUIState _state;

    public TileType Type => _type;
    public TileUIState State => _state;
}