using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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

    private void Awake()
    {
        _startMenu.ItemSelected += StartMenu_ItemSelected;
    }
    void Start()
    {
        //_startMenu.AddItem("Hello", null);
        //_startMenu.AddItem("World", null);
        //_startMenu.AddItem("1", null);
        //_startMenu.AddItem("2", null);
        //OpenAbilityMenu();
    }

    private void Update()
    {
        if (_gameState.CurrentMode == GameMode.WaitingForSelection && Input.GetMouseButtonDown(0))
        {
            //check tile clicked

            Vector3Int rawCelPos = tileDisplay.Grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2Int cellClicked = new Vector2Int(rawCelPos.x, rawCelPos.y);

            Debug.Log($"Clicked cell ({cellClicked.x}, {cellClicked.y})");
            if (IsCellValid(_gameState, cellClicked))
            {
                SelectTile(cellClicked);
            }
            tileDisplay.UpdateTiles(_gameState.Tiles);
        }
    }

    private bool IsCellValid(GameState gameState, Vector2Int cell)
    {
        return gameState.SelectableTiles.Contains(cell);
    }
    public void Initialize(GameState gameState, IEnumerable<KeyValuePair<AbilityType, Sprite>> activeSprites)
    {

        _gameState = gameState;
        _activeSprites = new List<KeyValuePair<AbilityType, Sprite>>(activeSprites);
        tileDisplay.InitializeMapDimension(gameState.Map.Width, gameState.Map.Height);
    }
    public void LoadGameState(GameState gameState)
    {
        _gameState = gameState;
        UpdateTiles(gameState.Tiles);
        UpdateActors(gameState.CurrentActors
            .Select(cu => new KeyValuePair<Actor, Vector2Int>(cu.Value, cu.Value.Position)));
    }

    //choose tile
    public void SelectTile(Vector2Int selection)
    {
        _gameState.SelectedTile = selection;
        //get selected ability -> get triggers for that ability at the selected tile

        Debug.Log($"SelectableAbilityTriggers count{_gameState.SelectableAbilityTriggers.Count}");

        IList<AbilityTrigger> triggersForSelectedAbility =
            _gameState.SelectableAbilityTriggers
            .Where((triggers) => triggers.Key.Type == _gameState.SelectedAbility.Type)
            .First().Value.ToList();
        Debug.Log($"triggersForSelectedAbility count{triggersForSelectedAbility.Count}");
        _gameState.SelectedAbilityTrigger = triggersForSelectedAbility.Where((trigger) => trigger.Selection == selection).First();
        Debug.Log($"selected ability trigger casterid {_gameState.SelectedAbilityTrigger.CasterId}");
        Debug.Log($"selected ability trigger effect trigger count {_gameState.SelectedAbilityTrigger.ActorIdsToEffectTriggers.Count}");
        _gameState.CurrentMode = GameMode.ResolveEffects;
        _gameState.ReadyToTick = true;
    }




    public void StartRound(int round)
    {

    }

    public void EndRound(int round)
    {

    }

    public void ShowAbilityMenu(IEnumerable<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>> abilities)
    {
        _startMenu.Clear();
        foreach( var (ability, trigger) in abilities ) {
            bool interactable = trigger.Any();
            var sprite = ActiveSprites.FirstOrDefault(x => x.Key == ability.Type).Value;
            _startMenu.AddItem(ability.Name, sprite, interactable);
        }
        OpenAbilityMenu();
    }

    public void UpdateTiles(Tile[,] tiles)
    {
        tileDisplay.UpdateTiles(tiles);
    }

    public void UpdateActors(IEnumerable<KeyValuePair<Actor, Vector2Int>> actorPositions)
    {
        tileDisplay.UpdateActors(actorPositions);
    }

    public void ShowSelectableTiles(IList<Vector2Int> selectableTiles)
    {
        throw new NotImplementedException();
    }

    private void OpenAbilityMenu()
    {
        _startMenu.Open();
        _taskbar.Open();
    }

    private void CloseAbilityMenu()
    {
        _startMenu.Close();
        _taskbar.Close();
    }

    private void SelectAbility(int index)
    {
        //need to update the selectable tiles with the tiles the selected ability can target
        
        _gameState.SelectedAbility = _gameState.SelectableAbilities[index];
        Debug.Log($"Selected ability {_gameState.SelectableAbilities[index].Name} at index {index}");
        _gameState.CurrentMode = GameMode.WaitingForSelection;
        _gameState.ReadyToTick = true;
    }

    private void StartMenu_ItemSelected(object sender, int index)
    {
        SelectAbility(index);
        CloseAbilityMenu();
    }
}
