using UnityEngine;
using System.Collections;

// Sets character movespeed to a higher value for a short period of time.
public class BeastHaste : Spell {
	protected override string pathToIcon => "Sprites/Icons/BeastHaste";
	protected override string name => "Beast Haste";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	float duration = 6f;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		CharacterControls characterControls = casterRef.gameObject.GetComponent<CharacterControls>();
		
		characterControls.SetSpeedHaste();
		SoundManager.PlaySound("BeastHaste", 0.5f);
		
		
		Vector3 effectPosition = casterRef.gameObject.transform.position;
		effectPosition.y -= 1f;
		
		// Create one-time on top of the player.
		GameObject magicEffectAcquirePrefab = Resources.Load<GameObject>("Prefabs/Magic/BeastHasteAcquire");;
		GameObject acquireInstance = Object.Instantiate(magicEffectAcquirePrefab, effectPosition, Quaternion.identity);
		acquireInstance.transform.parent = casterRef.gameObject.transform;
		
		// Create a looping effect on top of the player.
		GameObject magicEffectAuraPrefab = Resources.Load<GameObject>("Prefabs/Magic/BeastHasteAura");
		GameObject auraInstance = Object.Instantiate(magicEffectAuraPrefab, effectPosition, Quaternion.identity);
		auraInstance.transform.parent = casterRef.gameObject.transform;
		
		yield return new WaitForSeconds(duration);
		
		// Once the duration runs out, stop aura prefab emissions.
		auraInstance.GetComponent<ParticleSystem>().Stop();
		
		characterControls.SetSpeedDefault();
	}
}
