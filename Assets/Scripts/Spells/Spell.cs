using UnityEngine;
using System.Collections;

public abstract class Spell {
	public enum CastType {
		Instant,
		Hit
	}
	// Path to icon from Resources folder without an extension.
	protected abstract string pathToIcon { get; }
	
	// Type of the spell.
	protected abstract CastType type { get; }

	// Whether the spel disappears after cast.
	protected abstract bool singleUse {get; }
	
	// Function for all abilities.
	public abstract IEnumerator Cast(CharacterAbilities casterRef);
	
	// Properties for abilities that function as hit types.
	public virtual float minBallSpeed {get; }
	public virtual float maxBallSpeed {get; }
	public virtual float chargeMaxTime {get; }
	public virtual float knockbackValue {get; }
	
	public virtual IEnumerator Charge(CharacterAbilities casterRef) { yield break; }
	public virtual void EndCharge(CharacterAbilities casterRef) { return; }

	public Sprite GetIcon() {
		return Resources.Load<Sprite>(pathToIcon);
	}

	public CastType GetCastType() {
		return type;
	}
	
	public bool GetSingleUse() {
		return singleUse;
	}
}
