using UnityEngine;
using System.Collections;

// Creates an electric shield in the middle of the map that acts like a one-time wall for the ball.
public class ElectricShield : Spell {
	protected override string pathToIcon => "Sprites/Icons/ElectricShield";
	protected override string name => "Electric Shield";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		GameObject.Find("Boundaries").transform.
		Find("BallWall").gameObject.SetActive(true);
		
		SoundManager.PlaySound("ElectricShieldAppear");
		
		yield break;
	}
}
