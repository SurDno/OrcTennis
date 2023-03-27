using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Creates a small AoE that slows anybody coming into it.
// Based on Ahsan's code.
public class FreezingField : Spell {
	protected override string pathToIcon => "Sprites/Icons/FreezingField";
	protected override string name => "Freezing Field";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	// If any of those are modified, particle system prefab needs to be updated to avoid mismatch.
	float duration = 7f;
	float radius = 4f;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		Ball ball = Object.FindObjectOfType(typeof(Ball)) as Ball;
		
		// Create an area of effect on top of the ball, make it follow the ball.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/FreezingFieldAOE");
		Vector3 effectPosition = ball.gameObject.transform.position;
		effectPosition.y -= 1f;
		GameObject instance = Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
		instance.transform.parent = ball.gameObject.transform;
		
		List<CharacterControls> affectedPlayers = new List<CharacterControls>();
		
		SoundManager.PlaySound("FreezingField", 0.75f);
		
		float timer = 0;
		while(timer < duration) {
			// Give speed back to all previously affected players before we calculate who should be slowed again.
			foreach(CharacterControls player in affectedPlayers)
				player.GiveSpeed();
			affectedPlayers = new List<CharacterControls>();
			
			// Get all colliders within freeze radius.
            Collider[] colliders = Physics.OverlapSphere(instance.transform.position, radius); 
			
			// Sort through colliders to find actual potential targets.
            foreach (Collider collider in colliders) {
				// Ignore all non-player colliders.
				if (!collider.gameObject.GetComponent<CharacterControls>())
					continue;

				// Ignore all friendly players.
				//if(collider.gameObject.GetComponent<CharacterOwner>().GetOwner().GetTeam() == casterRef.gameObject.GetComponent<CharacterOwner>().GetOwner().GetTeam())
				//	continue;
				
				affectedPlayers.Add(collider.gameObject.GetComponent<CharacterControls>());
            }
			
			// Slow each target and create an effect on it.
			foreach(CharacterControls playerToSlow in affectedPlayers) {
				playerToSlow.TakeSpeed();
				
				GameObject magicHitEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/FreezingFieldHit");
				Vector3 effectHitPosition = playerToSlow.gameObject.transform.position;
				GameObject instanceHit = Object.Instantiate(magicHitEffectPrefab, effectHitPosition, Quaternion.identity);
				instanceHit.transform.parent = playerToSlow.gameObject.transform;
			}
				 
			// Wait for next freeze check.
			timer += 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		
		// If players were frozen for the whole duration, unfreeze them once the effects ends.
		foreach(CharacterControls player in affectedPlayers)
			player.GiveSpeed();
		
		yield break;
	}
}
