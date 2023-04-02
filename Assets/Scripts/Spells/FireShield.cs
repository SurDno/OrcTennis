using UnityEngine;
using System.Collections;

// Protects you from periodic damage from being on the other side for a short amount of time.
public class FireShield : Spell {
	protected override string pathToIcon => "Sprites/Icons/FireShield";
	protected override string name => "Fire Shield";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	float duration = 8f;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		CharacterHealth characterHealth = casterRef.gameObject.GetComponent<CharacterHealth>();
		
		characterHealth.SetInvulnerability(true);
		SoundManager.PlaySound("FireShield", 0.5f);
		
		// Create a looping effect on top of the player.
		GameObject magicEffectAuraPrefab = Resources.Load<GameObject>("Prefabs/Magic/FireShieldAura");
		Vector3 effectPosition = casterRef.gameObject.transform.position;
		effectPosition.y -= 1f;
		GameObject auraInstance = Object.Instantiate(magicEffectAuraPrefab, effectPosition, Quaternion.identity);
		auraInstance.transform.parent = casterRef.gameObject.transform;
		
		yield return new WaitForSeconds(duration);
		
		// Once the duration runs out, stop aura prefab emissions.
		auraInstance.GetComponent<ParticleSystem>().Stop();
		
		characterHealth.SetInvulnerability(false);
	}
}
