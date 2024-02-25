using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //this class should have methods just to update the UI with the payloads it needs
    // to render menus, selections, etc.

    private GameState _gameState;

    public void LoadGameState(GameState gameState)
    {
        _gameState = gameState;
    }


    //choose tile
    public void SelectTile(Vector2Int selection)
    {
        _gameState.SelectedTile = selection;
        _gameState.CurrentMode = GameMode.ResolveAction;
    }

    //choose an ability
    public void SelectAbility(int index)
    {
        _gameState.SelectedAbility = _gameState.SelectableAbilities[index];
        _gameState.CurrentMode = GameMode.WaitingForSelection;
    }

    public void StartRound(int round)
    {

    }

    public void EndRound(int round)
    {

    }

    public void ShowAbilityMenu(IEnumerable<Ability> ability)
    {

    }
}
