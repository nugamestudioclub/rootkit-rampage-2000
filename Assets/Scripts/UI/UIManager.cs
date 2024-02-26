using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	//this class should have methods just to update the UI with the payloads it needs
	// to render menus, selections, etc.

	private GameState _gameState;

	[SerializeField]
	private TileDisplay tileDisplay;

	[SerializeField]
	private StartMenu _startMenu;

	[SerializeField]
	private Taskbar _taskbar;

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
		OpenAbilityMenu();
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

	public void LoadGameState(GameState gameState) {
		_gameState = gameState;
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


	public void Load(GameState gameState, IEnumerable<KeyValuePair<AbilityType, Sprite>> activeSprites) {
		LoadGameState(gameState);
		_activeSprites = new List<KeyValuePair<AbilityType, Sprite>>(activeSprites);
	}

	public void StartRound(int round) {

	}

	public void EndRound(int round) {

	}

	public void ShowAbilityMenu(IEnumerable<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>> abilities) {
		_startMenu.Clear();
		foreach( var (ability, trigger) in abilities ) {
			_startMenu.AddItem(
				ability.Name,
				ActiveSprites.Where(x => x.Key == ability.Type).FirstOrDefault(null).Value
			);
		}
		OpenAbilityMenu();
	}

	public void UpdateTiles(Tile[,] tiles) {
		tileDisplay.UpdateTiles(tiles);
	}
	public void ShowSelectableTiles(IList<Vector2Int> selectableTiles) {
		throw new NotImplementedException();
	}

	private void OpenAbilityMenu() {
		_startMenu.Open();
		_taskbar.Open();
	}

	private void CloseAbilityMenu() {
		_startMenu.Close();
		_taskbar.Close();
	}

	private void SelectAbility(int index) {
		_gameState.SelectedAbility = _gameState.SelectableAbilities[index];
		_gameState.CurrentMode = GameMode.WaitingForSelection;
	}

	private void StartMenu_ItemSelected(object sender, int index) {
		Debug.Log(index);
		SelectAbility(index);
		CloseAbilityMenu();
	}
}
