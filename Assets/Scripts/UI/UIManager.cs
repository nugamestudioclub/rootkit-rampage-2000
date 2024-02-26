using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour {
	//this class should have methods just to update the UI with the payloads it needs
	// to render menus, selections, etc.

	private GameState _gameState;
	
    [SerializeField]
    private TileDisplay tileDisplay;

	[SerializeField]
	private StartMenu _startMenu;
	
    private List<KeyValuePair<AbilityType, Sprite>> _activeSprites = new List<KeyValuePair<AbilityType, Sprite>>();
    public IList<KeyValuePair<AbilityType, Sprite>> ActiveSprites => _activeSprites;

	private void Awake() {
		_startMenu.ItemSelected += StartMenu_ItemSelected;
	}
	void Start() {
		_startMenu.AddItem("Hello", null);
		_startMenu.AddItem("World", null);
		_startMenu.AddItem("1", null);
		_startMenu.AddItem("2", null);
		_startMenu.Open();
	}

	private void Update() {
		if( Input.GetMouseButtonDown(0) ) {
			//check tile clicked

			Vector3Int rawCelPos = tileDisplay.Grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			Vector2Int cellClicked = new Vector2Int(rawCelPos.x, rawCelPos.y);

			Debug.Log($"Clicked cell ({cellClicked.x}, {cellClicked.y})");
			if( _gameState.CurrentMode == GameMode.WaitingForSelection &&
				IsCellValid(_gameState, cellClicked) ) {
				SelectTile(cellClicked);
			}
			tileDisplay.UpdateTiles(_gameState.Tiles);
		}
	}

	private bool IsCellValid(GameState gameState, Vector2Int cell) {
		return gameState.SelectableTiles.Contains(cell);
	}
	
    public void LoadGameState(GameState gameState, IEnumerable<KeyValuePair<AbilityType, Sprite>> activeSprites)
    {
        _gameState = gameState;
        _activeSprites = new List<KeyValuePair<AbilityType, Sprite>>(activeSprites);
    }
	
	//choose tile
	public void SelectTile(Vector2Int selection) {
		_gameState.SelectedTile = selection;
		//get selected ability -> get triggers for that ability at the selected tile
		IList<AbilityTrigger > triggersForSelectedAbility =
			_gameState.SelectableAbilityTriggers
			.Where((triggers) => triggers.Key.Type == _gameState.SelectedAbility.Type)
			.First().Value.ToList();
		_gameState.SelectedAbilityTrigger = triggersForSelectedAbility.Where((trigger) => trigger.Selection == selection).First();
		_gameState.CurrentMode = GameMode.ResolveEffects;
	}

	//choose an ability
	public void SelectAbility(int index) {
		_gameState.SelectedAbility = _gameState.SelectableAbilities[index];

		_gameState.CurrentMode = GameMode.WaitingForSelection;
	}

	public void StartRound(int round) {

	}
	
	public void EndRound(int round) {

	}

	public void ShowAbilityMenu(
		IEnumerable<KeyValuePair<Ability,
			IEnumerable<AbilityTrigger>>> abilities) {
		throw new NotImplementedException();
	}

	public void UpdateTiles(Tile[,] tiles) {
		tileDisplay.UpdateTiles(tiles);
	}
	public void ShowSelectableTiles(IList<Vector2Int> selectableTiles) {
		throw new NotImplementedException();
	}

	private void StartMenu_ItemSelected(object sender, int index) {
		Debug.Log(index);
		_startMenu.Close();
	}
}
