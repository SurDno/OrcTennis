using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour{
    [SerializeField]private float speed = 2f;
	[SerializeField]private float angle = 45f;
	[SerializeField]private Vector2 velocity;

	void Start() {
        velocity.x = Mathf.Cos(angle * Mathf.Deg2Rad) * speed;
		velocity.y = Mathf.Sin(angle * Mathf.Deg2Rad) * speed;	
	}
	
    void FixedUpdate() {
		
		GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, 0, velocity.y);
    }
	
	void OnCollisionEnter(Collision col) {
        // Print how many points are colliding with this transform
        Debug.Log("Points colliding: " + col.contacts.Length);

        // Print the normal of the first point in the collision.
        Debug.Log("Normal of the first point: " + col.contacts[0].normal);

        // Draw a different colored ray for every normal in the collision
        foreach (var item in col.contacts)
        {
            Debug.DrawRay(item.point, item.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
        }
		
		Reflect(col.contacts[0].normal);
	}
	
	// Reflects the angle of the ball upon wall collision, based on collision normal.
	void Reflect(Vector3 normal) {
		
		if(Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
			velocity.x = -velocity.x;
		else
			velocity.y = -velocity.y;
	}
}
