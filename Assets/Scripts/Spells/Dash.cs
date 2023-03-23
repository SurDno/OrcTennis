using UnityEngine;
using System.Collections;

// Gives you a forward-facing knockback, essentially dashing you forward.
public class Dash : Spell {
	protected override string pathToIcon => "Sprites/Icons/Dash";
	protected override string name => "Dash";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		CharacterControls characterControls = casterRef.gameObject.GetComponent<CharacterControls>();
		
		float forwardAngle = casterRef.transform.eulerAngles.y - 90;
		Vector2 knockbackDirection = new Vector2(Mathf.Cos(forwardAngle * Mathf.Deg2Rad), -Mathf.Sin(forwardAngle * Mathf.Deg2Rad));
		Vector2 knockbackVector = 15 * knockbackDirection;
		characterControls.SetKnockback(knockbackVector);
		
		SoundManager.PlaySound("Dash");
		
		yield break;
	}
}
