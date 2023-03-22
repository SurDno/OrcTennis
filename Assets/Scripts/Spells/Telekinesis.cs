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
		SoundManager.PlaySound("Telekinesis");
		
		yield break;
	}
}
