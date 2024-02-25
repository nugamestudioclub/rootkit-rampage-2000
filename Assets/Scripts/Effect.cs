using System.Collections.Generic;

public readonly struct Effect
{
    public Effect(EffectType type, IList<string> tags, int damage)
    {
        _type = type;
        _tags = new List<string>(tags);
        _damage = damage;
    }

    private readonly EffectType _type;
    private readonly List<string> _tags;
    private readonly int _damage;

    public EffectType Type => _type;

    public IList<string> Tags => _tags;

    public int Damage => _damage;
}