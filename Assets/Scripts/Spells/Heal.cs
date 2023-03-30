using UnityEngine;
using System.Collections;

// Heals character to full HP.
public class Heal : Spell {
	protected override string pathToIcon => "Sprites/Icons/Heal";
	protected override string name => "Heal";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		CharacterHealth characterHealth = casterRef.gameObject.GetComponent<CharacterHealth>();
		
		characterHealth.Heal();
		SoundManager.PlaySound("Heal", 0.5f);
		
		// Create one-time on top of the player.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/Heal");
		Vector3 effectPosition = casterRef.gameObject.transform.position;
		effectPosition.y -= 1f;
		GameObject acquireInstance = Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
		acquireInstance.transform.parent = casterRef.gameObject.transform;
		
		yield break;
	}
}
