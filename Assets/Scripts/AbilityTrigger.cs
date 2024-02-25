using System.Collections.Generic;
using UnityEngine;

public readonly struct AbilityTrigger {
	public AbilityTrigger(string casterId, IList<Vector2Int> targets, IList<KeyValuePair<string, EffectTrigger>> effects)
	{
		CasterId = casterId;
		_targets = new List<Vector2Int>(targets);
		_effects = new List<KeyValuePair<string, EffectTrigger>>(effects);
	}

	private readonly List<Vector2Int> _targets;
	private readonly List<KeyValuePair<string, EffectTrigger>> _effects;

	public readonly string CasterId { get; }
	//list of tile coordinates which the ability can affect (for rendering in UI)
	public IList<Vector2Int> Targets => _targets;
	public IList<KeyValuePair<string, EffectTrigger>> Effects => _effects;
}