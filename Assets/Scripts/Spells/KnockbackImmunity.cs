using UnityEngine;
using System.Collections;

// Sends the ball away from you, no matter where you're located.
public class KnockbackImmunity : Spell {
	protected override string pathToIcon => "Sprites/Icons/KnockbackImmunity";
	protected override string name => "Knockback Immunity";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	// If any of those are modified, particle system prefab needs to be updated to avoid mismatch.
	float duration = 8f;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		// Initialize needed references.
		CharacterControls controls = casterRef.gameObject.GetComponent<CharacterControls>();
		
		// Create one-time effect on top of the player.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/KnockbackImmunityAcquire");
		Vector3 effectPosition = casterRef.gameObject.transform.position;
		effectPosition.y -= 0.95f;
		GameObject instance = Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
		instance.transform.parent = casterRef.gameObject.transform;
		
		SoundManager.PlaySound("KnockbackImmunityBlessing");
		
		// Create a looping effect on top of the player.
		GameObject magicEffectAuraPrefab = Resources.Load<GameObject>("Prefabs/Magic/KnockbackImmunityAura");
		GameObject auraInstance = Object.Instantiate(magicEffectAuraPrefab, effectPosition, Quaternion.identity);
		auraInstance.transform.parent = casterRef.gameObject.transform;
		
		// Reset player knockback values every frame for the entire duration.
		float timer = 0;
		while(timer < duration) {
			controls.ResetKnockback(true);
			yield return null;
			timer += Time.deltaTime;
		}
		
		yield break;
	}
}
