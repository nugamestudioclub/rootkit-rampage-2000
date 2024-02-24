using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(EffectTriggerConfig),
    menuName = "ScriptableObjects/" + nameof(EffectTriggerConfig))
]
public class EffectTriggerConfig : ScriptableObject
{
    [field: SerializeField]
    public EffectType Type { get; private set; }

    [field: SerializeField]
    public int Damage { get; private set; }

    [field: SerializeField]
    public float DamageVariance { get; private set; }

    [field: Range(0, 1)]
    [field: SerializeField]
    public float Chance { get; private set; }

    [field: SerializeField]
    public int Duration { get; private set; }

    [field: SerializeField]
    public List<string> Tags { get; private set; }

    public EffectTrigger Generate()
    {
        return new EffectTrigger(Type, Tags, Damage, DamageVariance, Chance);
    }
}