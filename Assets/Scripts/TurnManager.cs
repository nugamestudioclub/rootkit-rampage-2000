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
        //get all abilities
        Actor currentPlayer = gameState.CurrentUnits[charId];
        ///currentPlayer.
        //this render abilities in UI
        gameState.CurrentMode = GameMode.WaitingForAction;


        gameState.CurrentMode = Game
    }

    

    private void DoAITurn(string charId, GameState gameState)
    {
        (int, int)[] path = GetClosestPath(charId, gameState);
        Actor curActor = gameState.CurrentUnits[charId];
        // TODO recognize movement penalties
        Vector2Int targetPos = new Vector2Int(path[curActor.Movement].Item1, path[curActor.Movement].Item2);
        curActor.Move(targetPos);
    }

    private (int, int)[] GetClosestPath(string charId, GameState gameState)
    {
        IDictionary<string, Actor> curUnits = gameState.CurrentUnits;
        Actor curActor = curUnits[charId];
        (int, int)[] shortestPath = null;
        foreach ((string key, Actor value) in curUnits)
        {
            if (!charId.Equals(key))
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
