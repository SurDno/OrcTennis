using UnityEngine;
using System.Collections;

public class Knockback : Spell {
	protected override string pathToIcon => "Sprites/Icons/Knockback";
	protected override CastType type => Spell.CastType.Hit;
	protected override bool singleUse => false;
	
	public override float minBallSpeed => 3f;
	public override float maxBallSpeed => 6f;
	public override float chargeMaxTime => 0.12f;
	public override float knockbackValue => 20;
	
	// As there's no need to apply any effects or anything, the coroutine stays empty.
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		Ball ball = Object.FindObjectOfType(typeof(Ball)) as Ball;
		ball.gameObject.GetComponent<ColorObject>().objColor = new Color32(50, 0, 0, 255);
		
		yield break;
	}
}
