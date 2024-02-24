using System.Collections.Generic;
using UnityEngine;

public readonly struct AbilityOutcome {
	// TODO: add constructor

	private readonly List<Vector2> _targets;
	private readonly List<KeyValuePair<string, Effect>> _effects;

	public IList<Vector2> Targets => _targets;
	public IList<KeyValuePair<string, Effect>> Effects => _effects;
}