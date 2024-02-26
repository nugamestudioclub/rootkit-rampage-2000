using AStar;
using System.Collections.Generic;
using System.Linq;
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
        Debug.Log("turn count" + TurnOrder.Count);
        TurnIndex = 0;
        gameState.CurrentMode = GameMode.StartTurn;
    }

    private int GetNextAliveIndex(GameState gameState, int turnIndex, List<string> turnOrder)
    {
        bool isAlive;
        int currentIndex = (TurnIndex - 1 + TurnOrder.Count) % TurnOrder.Count;
        string nextUnit;
   
        for (int i = turnIndex; i < turnOrder.Count; i++)
        {
            nextUnit = turnOrder[currentIndex];
            isAlive = gameState.CurrentActors.ContainsKey(nextUnit);
            if (isAlive)
            {
                return currentIndex;
            }
        }
        for (int i = 0; i < TurnIndex; i++)
        {
            nextUnit = turnOrder[currentIndex];
            isAlive = gameState.CurrentActors.ContainsKey(nextUnit);
            if (isAlive)
            {
                return currentIndex;
            }
        }
        return -1;
    }

    public void StartTurn(GameState gameState)
    {
        TurnIndex = GetNextAliveIndex(gameState, TurnIndex, TurnOrder.ToList());
        if (TurnIndex < 0)
        {
            //TODO end round?
            gameState.CurrentMode = GameMode.GameOver;
            return;
        }
        string nextActorId = TurnOrder[TurnIndex];
        Actor currentActor = gameState.CurrentActors[nextActorId];
        gameState.CurrentActor = currentActor;

        
        Debug.Log($"Current Actor in StartTurn: {currentActor.Id}");
        //give player move and action budget
        gameState.CurrentActor = currentActor;
        gameState.ActorHasMove = true;
        gameState.ActorHasAction = true;
        switch (currentActor.Type)
        {
            case ActorType.Player:
                StartPlayerTurn(nextActorId, gameState);
                break;
            case ActorType.Ally:
            case ActorType.AI:
                //DoAITurn(nextUnit, gameState);
                StartPlayerTurn(nextActorId, gameState);
                break;
        }
        TurnIndex = (TurnIndex + 1) % TurnOrder.Count;
        gameState.CurrentMode = GameMode.WaitingForAction;
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
