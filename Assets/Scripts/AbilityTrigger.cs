using System.Collections.Generic;
using UnityEngine;

public readonly struct AbilityTrigger {
	public AbilityTrigger(string casterId, Vector2Int selection, IList<Vector2Int> targets, IList<KeyValuePair<string, EffectTrigger>> actorIdsToEffectTriggers)
	{
		CasterId = casterId;
		Selection = selection;
		_targets = new List<Vector2Int>(targets);
        _actorIdsToEffectTriggers = new List<KeyValuePair<string, EffectTrigger>>(actorIdsToEffectTriggers);
	}

	private readonly List<Vector2Int> _targets;
	private readonly List<KeyValuePair<string, EffectTrigger>> _actorIdsToEffectTriggers;

	public readonly string CasterId { get; }

    public readonly Vector2Int Selection { get; }
    //list of tile coordinates which the ability can affect (for rendering in UI)
    public IList<Vector2Int> Targets => _targets;
	public IList<KeyValuePair<string, EffectTrigger>> ActorIdsToEffectTriggers => _actorIdsToEffectTriggers;

	public bool IsEmpty => ActorIdsToEffectTriggers.Count == 0;
}