using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Resurrects all your dead teammates.
public class RaiseUndead : Spell {
	protected override string pathToIcon => "Sprites/Icons/RaiseUndead";
	protected override string name => "Raise Undead";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		
		List<CharacterHealth> playersToResurrect = new List<CharacterHealth>();
		
		// Find targets, create effects and animations.
        foreach (CharacterHealth player in (CharacterHealth[])Object.FindObjectsOfType(typeof(CharacterHealth))) {
			// Ignore all hostile players.
			if(player.gameObject.GetComponent<CharacterOwner>().GetOwner().GetTeam() != casterRef.gameObject.GetComponent<CharacterOwner>().GetOwner().GetTeam())
				continue;
			
			// Ignore all alive allies.
			if(!player.IsDead())
				continue;
			
			playersToResurrect.Add(player);
			
			// Start resurrection animation.
			player.GetComponent<CharacterAnimation>().StartResurrectAnimation();
			
			// Create one-time on top of each resurrected player.
			GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/RaiseUndead");
			Vector3 effectPosition = player.gameObject.transform.position;
			effectPosition.y -= 0.7f;
			GameObject acquireInstance = Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
			acquireInstance.transform.parent = player.gameObject.transform;
		}
		
		// Play a sound if there are players to resurrected
		if(!playersToResurrect.Any())
			yield break;
		else
			SoundManager.PlaySound("RaiseUndead", 0.5f);
		
		// Wait for effects to play.
		yield return new WaitForSeconds(2f);
		
		// Give targets health and full control again.
		foreach(CharacterHealth player in playersToResurrect) {
			player.Heal();
			MatchController.UnregisterDeath();
		}
	}
}
