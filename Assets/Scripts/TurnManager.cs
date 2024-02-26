using AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor.Playables;
using UnityEngine;

public class TurnManager
{

    public IList<string> TurnOrder { get; private set; }
    public int TurnIndex { get; private set; }

    public void StartRound(GameState gameState)
    {
        // get all units
        // put them into the turn order

        TurnOrder = gameState.CurrentActors.Keys.OrderBy(_ => gameState.Random.NextDouble()).ToList();
        TurnIndex = 0;
        gameState.CurrentMode = GameMode.StartTurn;
    }

    private Actor GetNextAliveActor(GameState gameState)
    {
        bool isAlive = false;
        int previousIndex = (TurnIndex - 1 + TurnOrder.Count) % TurnOrder.Count;
        int currentIndex = previousIndex;
        do {
            currentIndex = (currentIndex + 1) % TurnOrder.Count;
            string nextUnit = TurnOrder[currentIndex];
            isAlive = gameState.CurrentActors.ContainsKey(nextUnit);
            if (gameState.CurrentActors.ContainsKey(nextUnit))
            {
                TurnIndex = currentIndex;
                return gameState.CurrentActors[nextUnit];
            }

        } while (!isAlive && currentIndex != TurnIndex);
        return null;
    }

    public void StartTurn(GameState gameState)
    {
        string nextUnit = TurnOrder[TurnIndex];
        gameState.CurrentMode = GameMode.WaitingForAction;
        //give player move and action budget
        Actor currentActor = GetNextAliveActor(gameState);
        if (currentActor == null)
        {
            //TODO end round?
            gameState.CurrentMode = GameMode.GameOver;
            return;
        }
        Debug.Log($"Current Actor in StartTurn: {currentActor.Id}");

        gameState.CurrentActor = currentActor;
        gameState.ActorHasMove = true;
        gameState.ActorHasAction = true;
        switch (currentActor.Type)
        {
            case ActorType.Player:
                StartPlayerTurn(nextUnit, gameState);
                break;
            case ActorType.Ally:
            case ActorType.AI:
                //DoAITurn(nextUnit, gameState);
                StartPlayerTurn(nextUnit, gameState);
                break;
        }
        TurnIndex = TurnIndex + 1 % TurnOrder.Count;
    }
    public Actor GetCurrentPlayer(GameState gameState)
    {
        return gameState.CurrentActors[TurnOrder[TurnIndex]];
    }


    public void DoMove(string charId, GameState gameState)
    {

    }

    public void DoAction(string charId, AbilityTrigger chosenAbility, GameState gameState)
    {

    }

    public void DoWait(string charId, GameState gameState)
    {
        EndTurn(gameState);
    }

    public bool HasActions(GameState gameState)
    {
        return false;// gameState.ActorHasAction || gameState.ActorHasMove;
    }
    private void StartPlayerTurn(string charId, GameState gameState)
    {

    }

    public void EndTurn(GameState gameState)
    {
        gameState.ActorHasMove = false;
        gameState.ActorHasAction = false;
    }

    private void DoAITurn(string charId, GameState gameState)
    {
        (int, int)[] path = GetClosestPath(charId, gameState, ActorType.Player);
        Actor curActor = gameState.CurrentActors[charId];
        // TODO recognize movement penalties
        IList<Ability> abilityList = curActor.Abilities;
        AbilityTrigger attackData;
        attackData = TryAIAttack(curActor, gameState, abilityList[1]);
        if (attackData.IsEmpty)
        {
            attackData = TryAIAttack(curActor, gameState, abilityList[0]);
        }

        if (!attackData.IsEmpty)
        {
            // TODO do attack
            DoAction(charId, attackData, gameState);
        }
        else
        {
            Vector2Int targetPos = new Vector2Int(path[curActor.Movement].Item1, path[curActor.Movement].Item2);
            curActor.Move(targetPos);
        }
    }

    private AbilityTrigger TryAIAttack(Actor actor, GameState gameState, Ability ability)
    {
        // TODO recognize movement penalties
        (int, int)[] path = GetClosestPath(actor.Id, gameState, ActorType.Player);
        if (actor.Movement + ability.Range <= path.Length)
        {
            actor.Move(new Vector2Int(path[path.Length].Item1, path[path.Length].Item2));
            // new ability context needs the selected tile
            IList<AbilityTrigger> abilityTriggers = Abilities.GetAbilityTriggers(gameState, actor.Id, ability).ToList();



        }
        return new AbilityTrigger();
    }

    private (int, int)[] GetClosestPath(string charId, GameState gameState, ActorType type)
    {
        IDictionary<string, Actor> curUnits = gameState.CurrentActors;
        Actor curActor = curUnits[charId];
        (int, int)[] shortestPath = null;
        foreach ((string key, Actor value) in curUnits)
        {
            if (!charId.Equals(key) && value.Id.Equals(type))
            {
                (int, int)[] path = AStarPathfinding.GeneratePathSync(curActor.Position.x, curActor.Position.y,
                    value.Position.x, value.Position.y, gameState.CostMap, true, false);
                // TODO recognize movement penalties for path cost
                if (shortestPath == null || shortestPath.Length > path.Length)
                {
                    shortestPath = path;
                }
            }

        }
        return shortestPath;
    }

}
