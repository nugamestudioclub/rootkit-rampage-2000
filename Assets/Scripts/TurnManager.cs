using AStar;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager
{

    public IList<string> TurnOrder { get; private set;}
    public  int TurnIndex { get; private set; }

    public void StartRound(GameState gameState)
    {
        // get all units
        // put them into the turn order
        TurnOrder = gameState.CurrentUnits.Keys.OrderBy(_ => gameState.Random.NextDouble()).ToList();
        TurnIndex = 0;
    }

    public void TakeTurn(GameState gameState)
    {
        string nextUnit = TurnOrder[TurnIndex];
        switch (gameState.CurrentUnits[nextUnit].Type)
        {
            case ActorType.Player:
                StartPlayerTurn(nextUnit, gameState);
                
                break;
            case ActorType.Ally:
            case ActorType.AI:
                DoAITurn(nextUnit, gameState);
                break;
        }
        TurnIndex++;
    }
    public Actor GetCurrentPlayer(GameState gameState)
    {
        return gameState.CurrentUnits[TurnOrder[TurnIndex]];
    }
    private void StartPlayerTurn(string charId, GameState gameState)
    {
        //get all abilities
        Actor currentPlayer = gameState.CurrentUnits[charId];
        //this render abilities in UI
        gameState.CurrentMode = GameMode.WaitingForAction;
        

        
    }

    

    private void DoAITurn(string charId, GameState gameState)
    {
        (int, int)[] path = GetClosestPath(charId, gameState, ActorType.Player);
        Actor curActor = gameState.CurrentUnits[charId];
        // TODO recognize movement penalties
        IList<Ability> abilityList = curActor.Abilities;
        (AbilityTrigger, string)? attackData;
        attackData = TryAIAttack(curActor, gameState, abilityList[1]);
        if (attackData == null)
        {
            attackData = TryAIAttack(curActor, gameState, abilityList[0]);
        }

        if (attackData != null)
        {
            // TODO do attack
        }
        else
        {
            Vector2Int targetPos = new Vector2Int(path[curActor.Movement].Item1, path[curActor.Movement].Item2);
            curActor.Move(targetPos);
        }
    }

    private (AbilityTrigger, string)? TryAIAttack(Actor actor, GameState gameState, Ability ability)
    {
        // TODO recognize movement penalties
        (int, int)[] path = GetClosestPath(actor.Id, gameState, ActorType.Player);
        if (actor.Movement + ability.Range <= path.Length)
        {
            actor.Move(new Vector2Int(path[path.Length].Item1, path[path.Length].Item2));
            AbilityTrigger trigger = Abilities.Resolve(new AbilityContext(gameState, actor.Id, ability));
            return (trigger, trigger.Effects[0].Key);
        }
        else return null;
    }

    private (int, int)[] GetClosestPath(string charId, GameState gameState, ActorType type)
    {
        IDictionary<string, Actor> curUnits = gameState.CurrentUnits;
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
