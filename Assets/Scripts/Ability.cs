using System.Collections;
using System.Collections.Generic;

/*public interface Ability {
	AbilityType Type { get; }

	AbilityOutcome Cast(AbilityContext context);
}
*/

public readonly struct Ability
{
    public Ability(AbilityType type, IList<string> tags, string name, float accuracy, int energyCost, int range, IList<EffectTrigger> effectTriggers)
    {
        _type = type;
        _tags = new List<string>(tags);
        _name = name;
        _accuracy = accuracy;
        _energyCost = energyCost;
        _range = range;
        _effectTriggers = new List<EffectTrigger>(effectTriggers);
    }

    private readonly AbilityType _type;
    private readonly List<string> _tags;
    private readonly string _name;
    private readonly float _accuracy;
    private readonly int _energyCost;
    private readonly int _range;
    private readonly List<EffectTrigger> _effectTriggers;

    public AbilityType Type => _type;

    public IList<string> Tags => _tags;

    public string Name => _name;

    public float Accuracy => _accuracy;

    public int EnergyCost => _energyCost;

    public int Range => _range;

    public IList<EffectTrigger> EffectTriggers => _effectTriggers;
}