using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    
    public GameState GameState { get; set; }

    public TurnManager TurnManager { get; set; }

    public UIManager UIManager { get; set; }

    private readonly List<Actor> _actors;
    public IList<Actor> Actors => _actors;

    private readonly List<Ability> _abilities;
    public IList<Ability> Abilities => _abilities;

    public GameManager(GameState gameState, TurnManager turnManager, UIManager uIManager, 
        IEnumerable<Actor> actors, IEnumerable<Ability> abilities)
    {
        GameState = gameState;
        TurnManager = turnManager;
        UIManager = uIManager;
        _actors = new List<Actor>(actors);
        _abilities = new List<Ability>(abilities);

    }


    public void Tick()
    {
        switch(GameState.CurrentMode)
        {
            case GameMode.StartRound:
                UIManager.StartRound(++GameState.Round);
                TurnManager.StartRound(GameState);
                break;
            case GameMode.StartTurn:
                UIManager.LoadGameState(GameState);
                TurnManager.TakeTurn(GameState);
                GameState.CurrentMode = GameMode.WaitingForAction;
                break;
            case GameMode.WaitingForAction:
                Actor currentPlayer = TurnManager.GetCurrentPlayer(GameState);
                if (currentPlayer.Type == ActorType.Player)
                {
                    UIManager.ShowAbilityMenu(currentPlayer.Abilities);
                }
                break;
            case GameMode.WaitingForSelection:
                //show selectable tiles
                break;
            case GameMode.ResolveAction:
                //take all effect triggers and get their outcomes
                // apply to actors
                break;
            case GameMode.Menu:
                break;
        }
    }
}
