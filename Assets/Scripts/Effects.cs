using System;
using System.Collections.Generic;
using UnityEngine;

public static class Effects
{
    public static Effect Resolve(KeyValuePair<string, EffectTrigger> idToEffect, GameState gameState)
    {
        Actor effectRecipient = gameState.CurrentActors[idToEffect.Key];
        return idToEffect.Value.Type switch
        {
            EffectType.Damage => ResolveDamage(idToEffect.Value, effectRecipient, gameState),
            _ => ResolveInvalid(idToEffect.Value, effectRecipient, gameState)
        };
    }

    private static Effect ResolveInvalid(EffectTrigger trigger, Actor recipient, GameState gameState)
    {
        throw new NotImplementedException();
    }

    private static Effect ResolveDamage(EffectTrigger trigger, Actor recipient, GameState gameState)
    {
        bool hit = gameState.Random.NextDouble() <= trigger.Accuracy;
        if (hit)
        {
            return new Effect(EffectType.Damage, trigger.Tags, trigger.Damage);
        }
        return new Effect(EffectType.None, trigger.Tags, 0);
    }

}