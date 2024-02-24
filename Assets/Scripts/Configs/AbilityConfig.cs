using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(AbilityConfig),
    menuName = "ScriptableObjects/" + nameof(AbilityConfig))
]
public class AbilityConfig : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public AbilityType Type { get; private set; }

    [field: Range(0, 1)]
    [field: SerializeField]
    public float Accuracy { get; private set; }

    [field: SerializeField]
    public int EnergyCost { get; private set; }

    [field: SerializeField]
    
    public int Range { get; private set; }

    [field: SerializeField]
    public List<EffectTriggerConfig> EffectTriggers { get; private set; }

    [field: SerializeField]
    public List<string> Tags { get; private set; }

    public Ability Generate()
    {
        return new Ability(Type, Tags, Name, Accuracy, EnergyCost, Range, EffectTriggers.Select((e) => e.Generate()).ToList());
    }
}