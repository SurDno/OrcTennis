using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour{
	[SerializeField]private Vector2 velocity;
	
    void FixedUpdate() {
		GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, 0, velocity.y);
    }
	
	void OnCollisionEnter(Collision col) {
		// Right now, we treat any collision as wall collision. However, this is not ideal and checks for layers or tags should be added later.
		Reflect(new Vector2(col.contacts[0].normal.x, col.contacts[0].normal.z));
	}
	
	// Reflects the angle of the ball upon wall collision, based on collision normal.
	void Reflect(Vector2 normal) {
		if(Mathf.Abs(normal.x) < Mathf.Abs(normal.y)) {
			velocity.y = -velocity.y;
			return;
		}
		
		if(normal.x > 0)
			ScoreManager.GoalLeft();
		else
			ScoreManager.GoalRight();
		
		SetVelocity(Vector2.zero);
		transform.position = new Vector3(0, -2.5f, 0);
		
		foreach(Object player in FindObjectsOfType(typeof(CharacterControls))) {
			((CharacterControls)player).Respawn();
		}
	}
	
	// Changed direction and speed.
	public void SetVelocity(Vector2 movementVector) {
		velocity = movementVector;
	}
	
	// Changes direction, maintains speed.
	public void SetDirection(Vector2 movementVector) {
		if(velocity == Vector2.zero)
			velocity = Vector2.up;
		
		velocity = movementVector.normalized * velocity.magnitude;
	}
	
	public void SetDirection(float degrees) {
        velocity = new Vector2(Mathf.Cos(degrees * Mathf.Deg2Rad), -Mathf.Sin(degrees * Mathf.Deg2Rad)) * velocity.magnitude;
	}
	
	// Changes speed, maintains direction.
	public void SetSpeed(float speed) {
		if(velocity == Vector2.zero)
			velocity = Vector2.up;
		
		velocity = velocity.normalized * speed;
	}
}
