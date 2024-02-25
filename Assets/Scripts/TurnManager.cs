using AStar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class TurnManager
{

    private List<string> turnOrder;
    private int turnIndex;

    public void StartRound(GameState gameState)
    {
        // get all units
        // put them into the turn order
        turnOrder = gameState.CurrentUnits.Keys.OrderBy(_ => UnityEngine.Random.Range(0f, 1f)).ToList();
        turnIndex = 0;
    }

    public void TakeTurn(GameState gameState)
    {
        string nextUnit = turnOrder[turnIndex];
        switch (gameState.CurrentUnits[nextUnit].Type)
        {
            case ActorType.Player:
                DoPlayerTurn(nextUnit, gameState);
                break;
            case ActorType.Ally:
            case ActorType.AI:
                DoAITurn(nextUnit, gameState);
                break;
        }
        turnIndex++;
    }

    private void DoPlayerTurn(string charId, GameState gameState)
    {

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
