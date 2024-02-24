using System.Collections.Generic;

public readonly struct Effect {
	// TODO: add constructor

	private readonly EffectType _type;
	private readonly List<string> _tags;
	private readonly int _damage;
	private readonly float _damageVariance;
	private readonly float _accuracy;

	public EffectType Type => _type;

	public IList<string> Tags => _tags;

	public int Damage => _damage;

	public float DamageVariance => _damageVariance;

	public float Accuracy => _accuracy;
}