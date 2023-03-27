using UnityEngine;
using System.Collections;

// Gives you a very strong version of knockback ability.
public class SuperKnockback : Spell {
	protected override string pathToIcon => "Sprites/Icons/SuperKnockback";
	protected override string name => "Super Knockback";
	protected override CastType type => Spell.CastType.Hit;
	protected override bool singleUse => true;
	
	public override float minBallSpeed => 1f;
	public override float maxBallSpeed => 4f;
	public override float chargeMaxTime => 0.33f;
	public override float knockbackValue => 80;
	
	// As there's no need to apply any effects or anything, the coroutine stays empty.
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		Ball ball = Object.FindObjectOfType(typeof(Ball)) as Ball;
		ball.gameObject.GetComponent<ColorObject>().objColor = new Color32(100, 0, 0, 255);
		
		// Create an effect on top of the hit point.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/ImpactRed");
		Vector3 effectPosition = ball.gameObject.transform.position;
		Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
		
		SoundManager.PlaySound("SuperKnockback", 0.8f);
		
		yield break;
	}
}
