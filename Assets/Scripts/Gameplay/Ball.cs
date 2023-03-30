using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour{
	[Header("Current Values")]
	[SerializeField]private Vector2 velocity;
	private float knockbackForce = 0f;
	
    void FixedUpdate() {
		GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, 0, velocity.y);
    }
	
	void OnCollisionEnter(Collision col) {
		// Reflect the ball.
		if(Mathf.Abs(col.contacts[0].normal.x) < Mathf.Abs(col.contacts[0].normal.z))
			velocity.y = -velocity.y;
		else
			velocity.x = -velocity.x;
		
		// If we hit actual wall of the other team, register a goal.
		int walls = LayerMask.NameToLayer("Walls");
		if(col.gameObject.layer == walls && Mathf.Abs(col.contacts[0].normal.x) > Mathf.Abs(col.contacts[0].normal.z)) {
			if(col.contacts[0].normal.x > 0)
				MatchController.GoalLeft();
			else
				MatchController.GoalRight();
		}
	}
	
	public void ResetBall() {
		SetVelocity(Vector2.zero);
		SetKnockbackForce(0);
		GetComponent<ColorObject>().objColor = Color.black;
		transform.position = new Vector3(0, -2.5f, 0);
	}
	
	// Changes direction and speed.
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
	
	// Adds speed to the current velocity.
	public void AddSpeed(float speed) {
		if(velocity == Vector2.zero)
			velocity = Vector2.up;
		
		velocity = velocity.normalized * (velocity.magnitude + speed);
	}
	
	public Vector2 GetVelocity() {
		return velocity;
	}
	
	public void SetKnockbackForce(float newValue) {
		knockbackForce = newValue;
	}
	
	public float GetKnockbackForce() {
		return knockbackForce;
	}
}
