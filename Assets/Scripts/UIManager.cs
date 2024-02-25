using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //get selected ability -> get triggers for that ability at the selected tile
        IList<AbilityTrigger > triggersForSelectedAbility = 
            _gameState.SelectableAbilityTriggers
            .Where((triggers) => triggers.Key.Type == _gameState.SelectedAbility.Type)
            .First().Value.ToList();
        _gameState.SelectedAbilityTrigger = triggersForSelectedAbility.Where((trigger) => trigger.Selection == selection).First();
        _gameState.CurrentMode = GameMode.ResolveEffects;
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

    public void ShowAbilityMenu(
        IEnumerable<KeyValuePair<Ability,
            IEnumerable<AbilityTrigger>>> abilities)
    {
        throw new NotImplementedException();
    }

    public void ShowSelectableTiles(IList<Vector2Int> selectableTiles)
    {
        throw new NotImplementedException();
    }
}
