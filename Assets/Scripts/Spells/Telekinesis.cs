using UnityEngine;
using System.Collections;

// VERY BROKEN TELEKINESIS DO NOT USE
public class Telekinesis : Spell {
	protected override string pathToIcon => "Sprites/Icons/BeastMaster19";
	protected override CastType type => Spell.CastType.Instant;
	protected override bool singleUse => true;
	
	public override IEnumerator Cast(CharacterAbilities casterRef) {
		// Get references we'll need later
		GameObject playerCharacter = casterRef.gameObject;
		CharacterHit characterHit = playerCharacter.GetComponent<CharacterHit>();
		CharacterOwner characterOwner = playerCharacter.GetComponent<CharacterOwner>();
		GameObject chargingObj = characterHit.GegChargingObj();
		GameObject chargingObjHead = characterHit.GegChargingObjHead();
		SpriteRenderer[] chargingSprites = characterHit.GetChargingSprites();
		Ball ball = Object.FindObjectOfType(typeof(Ball)) as Ball;
		
		// Move charging object to ball and enable it.
		chargingObj.transform.parent = ball.transform;
		chargingObj.transform.localPosition = new Vector3(0, -0.82f, 0);
		chargingObj.SetActive(true);
		foreach(SpriteRenderer sprite in chargingSprites)
			sprite.color = characterOwner.GetOwner().GetColor();
			
		// Rotate ball from left stick input until we get right stick up
		while(!GamepadInput.GetRightTriggerUp(characterOwner.GetOwner().GetGamepad())) {
			Vector2 leftStickInput = GamepadInput.GetLeftStick(characterOwner.GetOwner().GetGamepad()).normalized;
			float rotationAngle = Mathf.Atan2(leftStickInput.x, leftStickInput.y) * Mathf.Rad2Deg;
			ball.gameObject.transform.eulerAngles = new Vector3(ball.gameObject.transform.eulerAngles.x, rotationAngle, ball.gameObject.transform.eulerAngles.z);
			yield return new WaitForFixedUpdate();
		}
		
		
		// Move charging object back to the player and disable it.
		chargingObj.transform.parent = playerCharacter.transform;
		chargingObj.transform.localPosition = new Vector3(0, -0.82f, 0);
		chargingObj.transform.eulerAngles = new Vector3(90, 0, 0);
			
		yield break;
	}
}
