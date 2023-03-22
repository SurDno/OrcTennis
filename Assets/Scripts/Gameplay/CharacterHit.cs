using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterOwner))]
[RequireComponent(typeof(CharacterControls))]
[RequireComponent(typeof(CharacterAbilities))]
public class CharacterHit : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private GameObject chargingObj;
	[SerializeField]private GameObject chargingObjHead;
	[SerializeField]private SpriteRenderer[] chargingSprites;
	
	private CharacterOwner characterOwner;
	private CharacterControls characterControls;
	private CharacterAbilities characterAbilities;
	private Ball ball;
	
	[Header("Settings")]
	[SerializeField]private float maxHitDistance = 3.0f;
	[SerializeField]private float hitCooldown = 0.4f;
	
	[Header("Current Values")]
	[SerializeField]private float minBallSpeed;
	[SerializeField]private float maxBallSpeed;
	[SerializeField]private float chargeMaxTime;
	[SerializeField]private float knockbackValue;
	private bool charging;
	private bool onCooldown;
	private float chargeValue;

	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		characterControls = GetComponent<CharacterControls>();
		characterAbilities = GetComponent<CharacterAbilities>();
		
		ball = FindObjectOfType(typeof(Ball)) as Ball;
    }
	
    void Update() {
        if(characterOwner.GetOwner() == null)
			return;
		
		// If we press right trigger..
		if(GamepadInput.GetRightTriggerDown(characterOwner.GetOwner().GetGamepad())) {
			// Check if we can hit right now.
			if(!charging && !onCooldown && characterAbilities.GetSelectedAbility().GetCastType() == Spell.CastType.Hit)
				StartCharge();
		}
	
		if(GamepadInput.GetRightTriggerUp(characterOwner.GetOwner().GetGamepad())) {
			// Check if we can stop charging right now.
			if(charging) 
				EndCharge();
		}
    }
	
	void StartCharge() {
		// Enable charging object.
		charging = true;
		chargingObj.SetActive(true);
		foreach(SpriteRenderer sprite in chargingSprites)
			sprite.color = characterOwner.GetOwner().GetColor();
			
		// Get charging information from selected spell.
		minBallSpeed = characterAbilities.GetSelectedAbility().minBallSpeed;
		maxBallSpeed = characterAbilities.GetSelectedAbility().maxBallSpeed;
		chargeMaxTime = characterAbilities.GetSelectedAbility().chargeMaxTime;
		knockbackValue = characterAbilities.GetSelectedAbility().knockbackValue;
		
		StartCoroutine(Charge());
	}
	
	IEnumerator Charge() {
		float chargeTime = 0;
		
		while(charging) {
			// Update charge value if we stop charging right now.
			chargeTime += 0.01f;
			chargeTime = Mathf.Min(chargeTime, chargeMaxTime);
			
			// Update charge value with linear formula. 
			chargeValue = minBallSpeed + (maxBallSpeed - minBallSpeed) * (chargeTime / chargeMaxTime);
			
			// Update visual representation.
			float multiplier = 0.35f;
			chargingObj.transform.localScale = new Vector3(1, (chargeTime / chargeMaxTime) * multiplier, 1);
			chargingObjHead.transform.localScale = new Vector3(1, 1 / chargingObj.transform.localScale.y, 1);
			
			// Wait for next update.
			yield return new WaitForSeconds(0.01f);
		}
	}
	
	void EndCharge() {
		characterAbilities.DestroySelectedAbility();
		charging = false;
		chargingObj.SetActive(false);
		
		// Start hit cooldown.
		StartCoroutine(Cooldown());
		
		// Calculate distance to the ball.
		float dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(ball.gameObject.transform.position.x, 0, ball.gameObject.transform.position.z));
			
		// If we're too far, ignore hit.
		if(dist > maxHitDistance)
			return;
			
        // Check if we're facing the ball.
        Vector3 toBall = ball.gameObject.transform.position - transform.position;
        float dot = Vector3.Dot(toBall.normalized, transform.forward);

        // If we're not facing the ball, ignore hit.
		// With 1 for same direction and -1 for opposite direction, 0.5f value effectively gives us a 120 degree window (cos 60 = 0.5)
        if (dot < 0.5f)
			return;
		
		// Apply knockback to the player.
		Vector2 knockbackDirection = new Vector2(Mathf.Cos((transform.eulerAngles.y - 270) * Mathf.Deg2Rad), -Mathf.Sin((transform.eulerAngles.y - 270) * Mathf.Deg2Rad));
		Vector2 knockbackVector = ball.GetKnockbackForce() * knockbackDirection;
		characterControls.SetKnockback(knockbackVector);
		
        // Hit the ball, apply the new knockback settings, "cast" the hit.
        ball.SetSpeed(chargeValue);
        ball.SetDirection(transform.eulerAngles.y - 90);
		ball.SetKnockbackForce(knockbackValue);
		StartCoroutine(characterAbilities.GetSelectedAbility().Cast(this.gameObject.GetComponent<CharacterAbilities>()));
	}
	
	IEnumerator Cooldown() {
		onCooldown = true;
		
		yield return new WaitForSeconds(hitCooldown);
		
		onCooldown = false;
	}
	
	public bool GetCharging() {
		return charging;
	}
	
	public GameObject GegChargingObj() {
		return chargingObj;
	}
	
	public GameObject GegChargingObjHead() {
		return chargingObjHead;
	}
	
	public SpriteRenderer[] GetChargingSprites() {
		return chargingSprites;
	}
}
