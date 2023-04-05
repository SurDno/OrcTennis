using UnityEngine;
using System.Collections;

// Heals character to full HP.
public class MassHeal : Spell {
	protected override string pathToIcon => "Sprites/Icons/MassHeal";
	protected override string name => "Mass Heal";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		SoundManager.PlaySound("Heal", 0.8f);
		
        foreach (CharacterHealth player in (CharacterHealth[])Object.FindObjectsOfType(typeof(CharacterHealth))) {
			// Ignore all hostile players.
			if(player.gameObject.GetComponent<CharacterOwner>().GetOwner().GetTeam() != casterRef.gameObject.GetComponent<CharacterOwner>().GetOwner().GetTeam())
				continue;
			
			// Ignore all dead allies.
			if(player.IsDead())
				continue;
			
			player.Heal();
		
			// Create one-time on top of each healed player.
			GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/Heal");
			Vector3 effectPosition = player.gameObject.transform.position;
			effectPosition.y -= 1f;
			GameObject acquireInstance = Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
			acquireInstance.transform.parent = player.gameObject.transform;
		}
		
		yield break;
	}
}
