using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

/*public interface Ability {
	AbilityType Type { get; }

	AbilityOutcome Cast(AbilityContext context);
}
*/

public static class Abilities
{
    public static AbilityTrigger Resolve(AbilityContext context)
    {
        return context.Ability.Type switch
        {
            AbilityType.Basic => ResolveBasic(context),
            _ => ResolveInvalid(context)
        };
    }
    private static AbilityTrigger ResolveInvalid(AbilityContext context)
    {
        throw new Exception();
    }
    private static AbilityTrigger ResolveBasic(AbilityContext context)
    {
        IList<Vector2Int> targets = FindTargetsInRange(context);
        IList<Actor> actors = FindActorsAtTargets(targets, context);
        IList<KeyValuePair<string, EffectTrigger>> effects = new List<KeyValuePair<string, EffectTrigger>>();

        foreach (Actor actor in actors)
        {
            foreach (EffectTrigger trigger in context.Ability.EffectTriggers)
            {
                effects.Add(new KeyValuePair<string, EffectTrigger>(actor.Id, trigger));

            }
        }
        return new AbilityTrigger(targets, effects);
    }

    private static IList<Vector2Int> FindTargetsInRange(AbilityContext context)
    {
        Vector2Int casterLocation = context.GameState.CurrentUnits[context.CasterId].Position;
        Tile[][] board = context.GameState.Tiles;
        IList<Vector2Int> targets = new List<Vector2Int>();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(0); i++)
            {
                //tile is valid and is in range

                if ((casterLocation.x - i * casterLocation.x - i) + (casterLocation.y - j * casterLocation.y - j) <= context.Ability.Range * context.Ability.Range)
                {
                    targets.Add(new Vector2Int(i, j));
                }
            }
        }
        return targets;
    }
    private static IList<Actor> FindActorsAtTargets(IList<Vector2Int> targets, AbilityContext context)
    {
        return context.GameState.CurrentUnits
            .Where((cu) => targets.Contains(cu.Value.Position))
            .Select((p) => p.Value)
            .ToList();
    }
}