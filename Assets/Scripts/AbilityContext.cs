using UnityEngine;

public readonly struct AbilityContext {
	public AbilityContext(GameState gameState, string casterId, Vector2Int selection, Ability ability)
	{
		_gameState = gameState;
		_casterId = casterId;
		_ability = ability;
		_selection = selection;
	}


	private readonly GameState _gameState;
	private readonly string _casterId;
	private readonly Ability _ability;
	private readonly Vector2Int _selection;


    public GameState GameState => _gameState;
    public string CasterId => _casterId;
	public Ability Ability => _ability;
	public Vector2Int Selection => _selection;

}