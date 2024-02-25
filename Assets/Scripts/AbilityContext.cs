public readonly struct AbilityContext {
	public AbilityContext(GameState gameState, string casterId, Ability ability)
	{
		_gameState = gameState;
		_casterId = casterId;
		_ability = ability;
	}


	private readonly GameState _gameState;
	private readonly string _casterId;
	private readonly Ability _ability;


    public GameState GameState => _gameState;
    public string CasterId => _casterId;
	public Ability Ability => _ability;

}