using UnityEngine;
using System.Collections;

// Instantly stops the ball wherever it is located.
public class Telekinesis : Spell {
	protected override string pathToIcon => "Sprites/Icons/Telekinesis";
	protected override string name => "Telekinesis";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		Ball ball = Object.FindObjectOfType(typeof(Ball)) as Ball;
		ball.SetSpeed(0);
		
		// Create an effect on top of the ball.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/TelekinesisBubble");
		Vector3 effectPosition = ball.gameObject.transform.position;
		GameObject instance = Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
		
		SoundManager.PlaySound("Telekinesis");
		
		yield break;
	}
}
