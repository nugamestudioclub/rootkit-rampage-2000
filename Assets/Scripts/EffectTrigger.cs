using System.Collections.Generic;

public readonly struct EffectTrigger {
	public EffectTrigger(EffectType type, IList<string> tags, int damage, float damageVariance, float accuracy, float chance)
	{
		_type = type;
		_tags = new List<string>(tags);
		_damage = damage;
		_damageVariance = damageVariance;
		_accuracy = accuracy;
		_chance = chance;
	}

	private readonly EffectType _type;
	private readonly List<string> _tags;
	private readonly int _damage;
	private readonly float _damageVariance;
	private readonly float _accuracy;
	private readonly float _chance;

	public EffectType Type => _type;

	public IList<string> Tags => _tags;

	public int Damage => _damage;

	public float DamageVariance => _damageVariance;

    public float Accuracy => _accuracy;
    public float Chance => _chance;
}