using System;

public static class GameRules
{
    public static void ApplyEffect(Effect effect, Actor recipient, GameState state)
    {
         switch(effect.Type)
        {
            case EffectType.Damage: 
                ApplyDamage(effect, recipient, state);
                break;
            default:  
                HandleInvalidEffect(effect, recipient, state);
                break;
        };
    }

    private static void HandleInvalidEffect(Effect effect, Actor recipient, GameState state)
    {
        throw new NotImplementedException();
    }

    private static void ApplyDamage(Effect effect, Actor recipient, GameState state)
    {
        recipient.CurrentHealth -= effect.Damage;
    }
}