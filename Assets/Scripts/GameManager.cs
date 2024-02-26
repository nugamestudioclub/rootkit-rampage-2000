using System;
using System.Collections.Generic;
using System.Linq;
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

        if (GameState.ReadyToTick)
        {
            Debug.Log("ready to tick");
            switch (GameState.CurrentMode)
            {
                case GameMode.StartRound:
                    DoStartRound();
                    break;
                case GameMode.StartTurn:
                    Debug.Log("before starting turn");
                    DoStartTurn();
                    break;
                case GameMode.WaitingForAction:
                    DoWaitingForAction();
                    break;
                case GameMode.WaitingForSelection:
                    DoWaitingForSelection();
                    break;
                case GameMode.ResolveEffects:
                    DoResolveEffects();
                    break;
                case GameMode.PlayingEffectAnimation:
                    DoPlayingEffectAnimation();
                    break;
                //might not need this now that tick will only be called if ready
                case GameMode.FinishedEffectAnimation:
                    DoFinishedEffectAnimation();
                    break;
                case GameMode.EndTurn:
                    DoEndTurn();
                    break;
                case GameMode.EndRound:
                    DoEndRound();
                    break;
                case GameMode.Menu:
                    DoMenu();
                    break;
                case GameMode.GameOver:
                    DoGameOver();
                    break;
                default:
                    Debug.Log($"Game Mode: " +
                        $"{Enum.GetName(typeof(GameMode), GameState.CurrentMode)} not handled in Tick");
                    break;
            }
        }

    }

    private void DoStartRound()
    {
        UIManager.StartRound(++GameState.Round);
        TurnManager.StartRound(GameState);
        GameState.ReadyToTick = true;
    }

    private void DoStartTurn()
    {
        Debug.Log("starting turn");
        UIManager.LoadGameState(GameState);
        TurnManager.StartTurn(GameState);
        //GameState.CurrentMode = GameMode.WaitingForAction;
        GameState.ReadyToTick = true;
    }

    private void DoWaitingForAction()
    {
       // IList<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>> abilitiesToTriggers
         //           = new List<KeyValuePair<Ability, IEnumerable<AbilityTrigger>>>();
        Actor currentPlayer = GameState.CurrentActor;
        foreach (Ability ability in currentPlayer.Abilities)
        {
            GameState.SelectableAbilities.Add(ability);
        }

        foreach (Ability ability in currentPlayer.Abilities)
        {
            //might want to validate if action can be performed
            GameState.SelectableAbilityTiles.Add(
                ability,
                Abilities.FindValidSelections(GameState, currentPlayer.Id, ability)
                );
            IList<AbilityTrigger> abilityTriggers = Abilities.GetAbilityTriggers(GameState, GameState.CurrentActor.Id, ability).ToList();
            GameState.SelectableAbilityTriggers.Add(
                new KeyValuePair<Ability, IEnumerable<AbilityTrigger>>(ability, abilityTriggers));
            
        }
        if (currentPlayer.Type == ActorType.Player)
        {
            UIManager.ShowAbilityMenu(GameState.SelectableAbilityTriggers);
        } else
        {
            Debug.Log("AI action");
            UIManager.ShowAbilityMenu(GameState.SelectableAbilityTriggers);
        }
        GameState.ReadyToTick = false;
    }

    private void DoWaitingForSelection()
    {
        //show selectable tiles
        Debug.Log(GameState.SelectableTiles.Count);
        foreach (Vector2Int pos in GameState.SelectableTiles)
        {
            Tile currentTile = GameState.Tiles[pos.x, pos.y];
            GameState.Tiles[pos.x, pos.y] = new Tile(currentTile.Type, TileUIState.Selectable);
        }
        UIManager.UpdateTiles(GameState.Tiles);

        GameState.ReadyToTick = false;
    }

    private void DoResolveEffects()
    {

        //set active effects with selected tile and selected action


        //take all effect triggers and get their outcomes
        foreach (var effectTrigger in GameState.ActiveEffectTriggers)
        {
            GameState.ActiveEffects.Add(
                new KeyValuePair<string, Effect>(
                    effectTrigger.Key, 
                    Effects.Resolve(effectTrigger, GameState)));
        }
        // apply to actors
        Debug.Log($"GameState.ActiveEffects.Count:{ GameState.ActiveEffects.Count}");
        foreach (var effectToApply in GameState.ActiveEffects)
        {
            GameRules.ApplyEffect(effectToApply.Value, GameState.CurrentActors[effectToApply.Key], GameState);
        }
        //TODO: Need to update tiles accordingly
        foreach (Vector2Int pos in GameState.SelectableTiles)
        {
            Tile currentTile = GameState.Tiles[pos.x, pos.y];
            GameState.Tiles[pos.x, pos.y] = new Tile(currentTile.Type, TileUIState.None);
        }
        UIManager.UpdateTiles(GameState.Tiles);
        GameState.SelectableAbilities.Clear();
        GameState.SelectableAbilityTiles.Clear();
        GameState.SelectableAbilityTriggers.Clear();
        //TODO transition
        GameState.ReadyToTick = true;
        GameState.CurrentMode = GameMode.FinishedEffectAnimation;
    }

    private void DoPlayingEffectAnimation()
    {
        //TODO
        //do UI events for effects

        GameState.ReadyToTick = false;
    }

    private void DoFinishedEffectAnimation()
    {
        GameState.ActiveEffects.Clear();
        var newActors = GameState.CurrentActors.Where(actor => !actor.Value.IsDead).ToList();
        GameState.CurrentActors.Clear();
        foreach(var aliveActor in newActors )
        {
            GameState.CurrentActors.Add(aliveActor);
        }
        Debug.Log($"Current actors {GameState.CurrentActors.Count}");
        UIManager.UpdateActors(GameState.CurrentActors
            .Select(cu => new KeyValuePair<Actor, Vector2Int>(cu.Value, cu.Value.Position)));
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
        GameState.ReadyToTick = true;
    }

    private void DoEndTurn()
    {
        //TODO
        TurnManager.EndTurn(GameState);

        //TODO game over check
        if (GameState.CurrentActors.Count == 1)
        {
            GameState.CurrentMode = GameMode.GameOver; 
        }
        else
        {
            GameState.CurrentMode = GameMode.StartTurn;
        }
        Debug.Log("ending turn");
        GameState.ReadyToTick = true;
    }

    private void DoEndRound()
    {
        //TODO
        GameState.ReadyToTick = true;
    }

    private void DoMenu()
    {
        //TODO
        GameState.ReadyToTick = true;
    }

    private void DoGameOver()
    {
        //TODO
        GameState.ReadyToTick = false;
        Debug.Log("GAME OVER");
    }
}
