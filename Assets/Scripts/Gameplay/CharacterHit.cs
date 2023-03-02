using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterOwner))]
[RequireComponent(typeof(CharacterControls))]
public class CharacterHit : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private GameObject chargingObj;
	[SerializeField]private GameObject chargingObjHead;
	[SerializeField]private SpriteRenderer[] chargingSprites;
	
	private CharacterOwner characterOwner;
	private CharacterControls characterControls;
	private Ball ball;
	
	[Header("Settings")]
	[SerializeField]private float maxHitDistance = 3.0f;
	[SerializeField]private float minBallSpeed = 5.0f;
	[SerializeField]private float maxBallSpeed = 12.0f;
	[SerializeField]private float chargeMaxTime = 1.5f;
	[SerializeField]private float hitCooldown = 0.4f;
	
	[Header("Current Values")]
	private bool charging;
	private bool onCooldown;
	private float chargeValue;

	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		characterControls = GetComponent<CharacterControls>();
		ball = FindObjectOfType(typeof(Ball)) as Ball;
    }
	
    void Update() {
        if(characterOwner.GetOwner() == null)
			return;
		
		// If we press right trigger..
		if(GamepadInput.GetRightTriggerDown(characterOwner.GetOwner().GetGamepad())) {
			// Check if we can hit right now.
			if(!charging && !onCooldown)
				StartCharge();
		}
	
		if(GamepadInput.GetRightTriggerUp(characterOwner.GetOwner().GetGamepad())) {
			// Check if we can stop charging right now.
			if(charging) 
				EndCharge();
		}
    }
	
	void StartCharge() {
		charging = true;
		chargingObj.SetActive(true);
		foreach(SpriteRenderer sprite in chargingSprites)
			sprite.color = characterOwner.GetOwner().GetColor();
		
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
			float multiplier = 0.5f;
			chargingObj.transform.localScale = new Vector3(1, (chargeTime / chargeMaxTime) * multiplier, 1);
			chargingObjHead.transform.localScale = new Vector3(1, 1 / chargingObj.transform.localScale.y, 1);
			
			// Wait for next update.
			yield return new WaitForSeconds(0.01f);
		}
	}
	
	void EndCharge() {
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
		
        // Otherwise, change ball direction with random speed.
        ball.SetSpeed(chargeValue);
        ball.SetDirection(transform.eulerAngles.y - 90);
	}
	
	IEnumerator Cooldown() {
		onCooldown = true;
		
		yield return new WaitForSeconds(hitCooldown);
		
		onCooldown = false;
	}
	
	public bool GetCharging() {
		return charging;
	}
}
