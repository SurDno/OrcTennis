using UnityEngine;
using System.Collections;

public class SuperKnockback : Spell {
	protected override string pathToIcon => "Sprites/Icons/SuperKnockback";
	protected override CastType type => Spell.CastType.Hit;
	protected override bool singleUse => true;
	
	public override float minBallSpeed => 1f;
	public override float maxBallSpeed => 4f;
	public override float chargeMaxTime => 0.33f;
	public override float knockbackValue => 100;
	
	// As there's no need to apply any effects or anything, the coroutine stays empty.
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		Ball ball = Object.FindObjectOfType(typeof(Ball)) as Ball;
		ball.gameObject.GetComponent<ColorObject>().objColor = new Color32(255, 0, 0, 255);
		
		yield break;
	}
}
