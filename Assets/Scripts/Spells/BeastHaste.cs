using UnityEngine;
using System.Collections;

// Sets character movespeed to a higher value for a short period of time.
public class BeastHaste : Spell {
	protected override string pathToIcon => "Sprites/Icons/BeastHaste";
	protected override string name => "Beast Haste";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	float duration = 4f;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		CharacterControls characterControls = casterRef.gameObject.GetComponent<CharacterControls>();
		
		characterControls.SetSpeedHaste();
		SoundManager.PlaySound("BeastHaste", 0.5f);
		
		yield return new WaitForSeconds(duration);
		
		characterControls.SetSpeedDefault();
	}
}
