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
		
		// Create an effect on top of the player.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/BeastHaste");
		Vector3 effectPosition = casterRef.gameObject.transform.position;
		effectPosition.y -= 1f;
		GameObject instance = Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
		instance.transform.parent = casterRef.gameObject.transform;
		
		yield return new WaitForSeconds(duration);
		
		characterControls.SetSpeedDefault();
	}
}
