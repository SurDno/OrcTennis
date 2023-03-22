using UnityEngine;
using System.Collections;

// Blinks you randomly in the direction you're facing +- 30 degrees.
public class RandomBlink : Spell {
	protected override string pathToIcon => "Sprites/Icons/RandomBlink";
	protected override string name => "Random Blink";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		// Get where we're facing.
		Vector3 vectorForward = casterRef.gameObject.transform.forward;
		float degreesForward = (Mathf.Atan2(vectorForward.x, vectorForward.z) * Mathf.Rad2Deg) - 90;
		
		// Get blink direction and distance.
		float direction = degreesForward + Random.Range(-30f, 30f);
		float distance = Random.Range(1f, 25f);
		
		// Blink the character.
		casterRef.gameObject.transform.position += new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), 0, Mathf.Sin(direction * Mathf.Deg2Rad)) * distance;
		
		SoundManager.PlaySound("RandomBlink");
		
		yield break;
	}
}
