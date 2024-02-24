using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(ActorConfig),
    menuName = "ScriptableObjects/" + nameof(ActorConfig))
]
public class ActorConfig : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int Health { get; private set; }

    [field: SerializeField]
    public int Energy { get; private set; }

    [field: SerializeField]
    public int Movement { get; private set; }

    [field: SerializeField]
    public List<AbilityConfig> Abilities { get; private set; }

    [field: SerializeField]
    public List<string> Tags { get; private set; }

    //sprite

    public Actor Generate()
    {
        return new Actor(Name, Health, Energy, Movement, Abilities.Select((a) => a.Generate()).ToList(), Tags);
    }

}