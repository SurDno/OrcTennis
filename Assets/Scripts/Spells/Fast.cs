using UnityEngine;
using System.Collections;

public class Fast : Spell {
	protected override string pathToIcon => "Sprites/Icons/Fast";
	protected override string name => "Fast";
	protected override CastType type => Spell.CastType.Hit;
	protected override bool singleUse => false;
	
	public override float minBallSpeed => 6f;
	public override float maxBallSpeed => 9f;
	public override float chargeMaxTime => 0.12f;
	public override float knockbackValue => 5;
	
	// As there's no need to apply any effects or anything, the coroutine stays empty.
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		Ball ball = Object.FindObjectOfType(typeof(Ball)) as Ball;
		ball.gameObject.GetComponent<ColorObject>().objColor = new Color32(0, 50, 0, 255);
		
		// Disable all previous effects.
		ball.gameObject.transform.Find("BallEffectKnockback").gameObject.SetActive(false);
		ball.gameObject.transform.Find("BallEffectFast").gameObject.SetActive(false);
		ball.gameObject.transform.Find("BallEffectSuperKnockback").gameObject.SetActive(false);
		ball.gameObject.transform.Find("BallEffectSuperFast").gameObject.SetActive(false);
		
		// Enable the one we'd like the ball to have.
		ball.gameObject.transform.Find("BallEffectFast").gameObject.SetActive(true);
		
		// TODO: Replace with unique sound.
		SoundManager.PlaySound("Knockback", 0.2f);
		
		yield break;
	}
}
