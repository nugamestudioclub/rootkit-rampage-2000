using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility {
	AbilityType Type { get; }

	AbilityOutcome Cast(AbilityContext context);
}