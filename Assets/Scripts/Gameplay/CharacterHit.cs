using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterOwner))]
public class CharacterHit : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	private CharacterOwner characterOwner;
	private CharacterControls characterControls;
	private Ball ball;
	
	[Header("Settings")]
	[SerializeField]private float maxHitDistance = 3.0f;

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
			// Only hit when we're standing still.
			if(characterControls.GetMoving())
				return;
			
			// Calculate distance to the ball.
			float dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(ball.gameObject.transform.position.x, 0, ball.gameObject.transform.position.z));
			
			// If we're too far, ignore hit.
			if(dist > maxHitDistance)
				return;
			
			// Otherwise, change ball direction with random speed.
			ball.SetSpeed(Random.Range(5, 12));
			ball.SetDirection(transform.eulerAngles.y - 90);
		}
    }
}
