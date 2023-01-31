using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour{
    [SerializeField]private float speed = 2f;
	[SerializeField]private float angle = 45f;
	[SerializeField]private Vector2 velocity;

	// 
	void Start() {
        velocity.x = Mathf.Cos(angle * Mathf.Deg2Rad) * speed;
		velocity.y = Mathf.Sin(angle * Mathf.Deg2Rad) * speed;	
	}
	
    void FixedUpdate() {
		GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, 0, velocity.y);
    }
	
	void OnCollisionEnter(Collision col) {
		// Right now, we treat any collision as wall collision. However, this is not ideal and checks for layers or tags should be added later.
		Reflect(new Vector2(col.contacts[0].normal.x, col.contacts[0].normal.z));
	}
	
	// Reflects the angle of the ball upon wall collision, based on collision normal.
	void Reflect(Vector2 normal) {
		if(Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
			velocity.x = -velocity.x;
		else
			velocity.y = -velocity.y;
	}
}
