using UnityEngine;
using System.Collections;

// Sets character movespeed to a higher value for a short period of time.
public class BeastHaste : Spell {
	protected override string pathToIcon => "Sprites/Icons/BeastMaster19";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	float duration = 4f;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		CharacterControls characterControls = casterRef.gameObject.GetComponent<CharacterControls>();
		
		characterControls.SetSpeedHaste();
		
		yield return new WaitForSeconds(duration);
		
		characterControls.SetSpeedDefault();
	}
}
