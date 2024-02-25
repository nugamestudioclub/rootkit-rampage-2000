using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;

public class GameManager
{

    public GameState GameState { get; set; }

    public TurnManager TurnManager { get; set; }

    public UIManager UIManager { get; set; }

    private readonly List<Actor> _actors;
    public IList<Actor> LoadableActors => _actors;

    private readonly List<Ability> _abilities;
    public IList<Ability> LoadableAbilities => _abilities;

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
        if (GameState.CurrentMode != GameState.PreviousMode)
        {
            Debug.Log($"Game Mode Updated: {Enum.GetName(typeof(GameMode), GameState.CurrentMode)}");
        }
        switch (GameState.CurrentMode)
        {
            case GameMode.StartRound:
                UIManager.StartRound(++GameState.Round);
                TurnManager.StartRound(GameState);
                break;
            case GameMode.StartTurn:
                UIManager.LoadGameState(GameState);
                TurnManager.StartTurn(GameState);
                GameState.CurrentMode = GameMode.WaitingForAction;
                break;
            case GameMode.WaitingForAction:
                IList<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>> abilitiesToTriggers
                    = new List<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>>();
                Actor currentPlayer = TurnManager.GetCurrentPlayer(GameState);

                foreach (Ability ability in currentPlayer.Abilities)
                {
                    IList<AbilityTrigger> abilityTriggers = Abilities.GetAbilityTriggers(GameState, currentPlayer.Id, ability).ToList();
                    abilitiesToTriggers.Add(
                        new KeyValuePair<Ability, IEnumerable<AbilityTrigger>>(ability, abilityTriggers));
                }

                if (currentPlayer.Type == ActorType.Player)
                {
                    UIManager.ShowAbilityMenu(abilitiesToTriggers);
                }
                break;
            case GameMode.WaitingForSelection:
                //show selectable tiles
                GameState.SelectableTiles.Add(new Vector2Int(1, 1));
                GameState.SelectableTiles.Add(new Vector2Int(5, 2));
                foreach (Vector2Int pos in GameState.SelectableTiles)
                {
                    Tile currentTile = GameState.Tiles[pos.x, pos.y];
                    GameState.Tiles[pos.x, pos.y] = new Tile(currentTile.Type, TileUIState.Selectable);
                }
                UIManager.UpdateTiles(GameState.Tiles);
                break;
            case GameMode.ResolveEffects:
                //set active effects with selected tile and selected action


                //take all effect triggers and get their outcomes
                // apply to actors
                foreach (var effectToApply in GameState.ActiveEffects)
                {
                    GameRules.ApplyEffect(effectToApply.Value, GameState.CurrentUnits[effectToApply.Key], GameState);
                }

                break;
            case GameMode.PlayingEffectAnimation:
                //do UI events for effects
                break;
            case GameMode.FinishedEffectAnimation:
                GameState.ActiveEffects.Clear();
                //need to do logic if the actor can make any more moves with turn
                //end turn if wait or no actions possible
                if (TurnManager.HasActions(GameState))
                {
                    GameState.CurrentMode = GameMode.WaitingForAction;
                }
                else
                {
                    GameState.CurrentMode = GameMode.EndTurn;
                }

                break;
            case GameMode.Menu:
                break;
        }
    }
}
