using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameState _gameState;
    

    public void LoadGameState(GameState gameState)
    {
        _gameState = gameState;
    }

    //update loop will need to allow to choose tile/ability even the state

    private void Update()
    {
        if (_gameState.CurrentMode == GameMode.WaitingForAction)
        {
            //SelectAbility(_gameState.)
        }
    }
    //choose tile
    public void SelectTile(IEnumerable<Vector2Int> selectables)
    {

    }

    //choose an ability
    public void SelectAbility(IEnumerable<Ability> ability)
    {

    }
}
