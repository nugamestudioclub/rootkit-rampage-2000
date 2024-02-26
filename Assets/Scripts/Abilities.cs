using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class Abilities
{
    public static IEnumerable<KeyValuePair<AbilityContext, AbilityTrigger>>
        ResolveAll(IEnumerable<AbilityContext> contexts)
    {
        foreach (var context in contexts)
        {
            yield return new(context, Resolve(context));
        }
    }
    public static AbilityTrigger Resolve(AbilityContext context)
    {
        return context.Ability.Type switch
        {
            AbilityType.Basic => ResolveBasic(context),
            _ => ResolveBasic(context),
            //_ => ResolveInvalid(context)
        };
    }
    private static AbilityTrigger ResolveInvalid(AbilityContext context)
    {
        throw new Exception();
    }
    private static AbilityTrigger ResolveBasic(AbilityContext context)
    {
        IList<Vector2Int> targets = FindTargetsInRange(context);
        IList<Actor> actors = GameState.FindActorsAtPositions(targets, context.GameState);
        IList<KeyValuePair<string, EffectTrigger>> effects = new List<KeyValuePair<string, EffectTrigger>>();

        foreach (Actor actor in actors)
        {
            foreach (EffectTrigger trigger in context.Ability.EffectTriggers)
            {
                effects.Add(new KeyValuePair<string, EffectTrigger>(actor.Id, trigger));
            }
        }
        return new AbilityTrigger(context.CasterId, context.Selection, targets, effects);
    }

    public static IEnumerable<AbilityTrigger> GetAbilityTriggers(GameState state, string charId, Ability ability)
    {
        //maybe move state modification to what invokes this rather than here?
        //state.SelectedAbility = ability;
        //state.SelectableTiles.Clear();
        IList<AbilityTrigger> abilityTriggers = new List<AbilityTrigger>();

        foreach (Vector2Int selection in FindValidSelections(state, charId, ability))
        {
            //state.SelectableTiles.Add(selection);
            AbilityTrigger trigger = Resolve(new AbilityContext(state, charId, selection, ability));
            abilityTriggers.Add(trigger);
        }
        return abilityTriggers;
    }


    private static bool InRange(Vector2Int startPoint, Vector2Int endPoint, float distance)
    {
        return (startPoint.x - endPoint.x) * (startPoint.x - endPoint.x)
            + (startPoint.y - endPoint.y) * (startPoint.y - endPoint.y)
            <= distance * distance;
    }
    public static IEnumerable<Vector2Int> FindValidSelections(GameState state, string casterId, Ability abilty)
    {

        Vector2Int casterLocation = state.CurrentActors[casterId].Position;
        Tile[,] board = state.Tiles;
        IList<Vector2Int> selections = new List<Vector2Int>();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                //tile is valid and is in range

                if (InRange(casterLocation, new Vector2Int(i, j), abilty.Range))
                {
                    selections.Add(new Vector2Int(i, j));
                }
            }
        }
        Debug.Log($"Valid selections: {selections.Count}");
        return selections;
    }

    private static IList<Vector2Int> FindTargetsInRange(AbilityContext context)
    {
        Vector2Int casterLocation = context.GameState.CurrentActors[context.CasterId].Position;
        Tile[,] board = context.GameState.Tiles;
        IList<Vector2Int> targets = new List<Vector2Int>();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                //tile is valid and is in range

                if (InRange(casterLocation, new Vector2Int(i, j), context.Ability.Range))
                {
                    targets.Add(new Vector2Int(i, j));
                }
            }
        }
        return targets;
    }

}