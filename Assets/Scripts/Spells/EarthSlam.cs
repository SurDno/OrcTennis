using UnityEngine;
using System.Collections;

// Sends the ball away from you, no matter where you're located.
public class EarthSlam : Spell {
	protected override string pathToIcon => "Sprites/Icons/EarthSlam";
	protected override string name => "Earth Slam";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		Ball ball = Object.FindObjectOfType(typeof(Ball)) as Ball;
		
		// Get vector between the character and the ball.
		Vector3 velocityVector = ball.gameObject.transform.position - casterRef.gameObject.transform.position;
		velocityVector.y = 0;
		velocityVector = velocityVector.normalized * 6;
		
		ball.SetVelocity(new Vector2(velocityVector.x, velocityVector.z));
		
		// Create an effect on top of the player.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/EarthSlam");
		Vector3 effectPosition = casterRef.gameObject.transform.position;
		Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
		
		SoundManager.PlaySound("EarthSlam");
		
		yield break;
	}
}
