using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterOwner))]
[RequireComponent(typeof(CharacterHit))]
public class CharacterControls : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	private CharacterOwner characterOwner;
	private CharacterHit characterHit;
	private Camera sharedCamera;
	private Rigidbody rb;
	
	[Header("Settings")]
	[SerializeField]private float defaultSpeed = 5f;
	[SerializeField]private float hasteSpeed = 8f;
	[SerializeField]private float hinderedSpeed = 2.5f;
	[SerializeField]private float knockbackRecoverValue = 0.05f;
	
	[Header("Current Values")]
	[SerializeField]private bool moving;
	private int effects;
	private float currentSpeed;
	private Vector3 initPosition;
	private Vector2 knockbackVelocity;
	private Vector2 dashVelocity;
	
	// Initializing cached GameObjects and Components.
    void Start() {
        sharedCamera = Camera.main;
		characterOwner = GetComponent<CharacterOwner>();
		characterHit = GetComponent<CharacterHit>();
		rb = GetComponent<Rigidbody>();
		initPosition = transform.position;
		currentSpeed = defaultSpeed;
    }
	
    void FixedUpdate() {
        if(characterOwner.GetOwner() == null)
			return;
		
		// Determine speed from effects.
		currentSpeed = (effects == 0) ? defaultSpeed : (effects > 0) ? hasteSpeed : hinderedSpeed;
		
		// Use left stick input for either movement or rotation, depending on whether we're charging or not. 
		Vector2 leftStickInput = GamepadInput.GetLeftStick(characterOwner.GetOwner().GetGamepad()).normalized;
		if(characterOwner.GetOwner().GetCursor().GetCursorHidden() && GamepadExtensions.InputMoreThanDeadzone(leftStickInput)) {
			if(!characterHit.GetCharging()) {
				// If we're not charging, left stick is used for movement.
				Vector3 targetPos = transform.position + new Vector3(leftStickInput.x, 0, leftStickInput.y) * (currentSpeed * Time.fixedDeltaTime);

				// Face a direction we're going.
				transform.LookAt(targetPos);
				transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
				
				rb.MovePosition(targetPos);
			} else {
				// Otherwise, it is used for rotation.
				float rotationAngle = Mathf.Atan2(leftStickInput.x, leftStickInput.y) * Mathf.Rad2Deg;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationAngle, transform.eulerAngles.z);
			}
		}
		
		moving = GamepadExtensions.InputMoreThanDeadzone(leftStickInput);
		
		ApplyKnockback();
		
						
		//Set max position values.
		Vector3 temp = transform.position;
		temp.x = Mathf.Min(temp.x, 14f);
		temp.x = Mathf.Max(temp.x, -14f);
		temp.z = Mathf.Min(temp.z, 6.5f);
		temp.z = Mathf.Max(temp.z, -6.5f);
		transform.position = temp;
		
		// TODO: Move that to player start.
		characterOwner.GetOwner().GetCursor().HideCursor();
		
    }
	
	// Decreases the knockbackVelocity by knockbackRecoverValue and then applies it.
	void ApplyKnockback() {
		// Calculate knockback decay.
		float newKnockbackMagnitude = Mathf.Max(knockbackVelocity.magnitude - knockbackRecoverValue, 0);
		knockbackVelocity = knockbackVelocity.normalized * newKnockbackMagnitude;
		
		// Calculate dash decay.
		float newDashMagnitude = Mathf.Max(dashVelocity.magnitude - knockbackRecoverValue, 0);
		dashVelocity = dashVelocity.normalized * newDashMagnitude;
		
		// Apply a sum of those values.
		rb.velocity = new Vector3(knockbackVelocity.x + dashVelocity.x, 0, knockbackVelocity.y + dashVelocity.y);
	}
	
	// Applying new knockback value externally.
	public void SetKnockback(Vector2 newVelocity, bool dash) {
		if(!dash)
			knockbackVelocity += newVelocity;
		else
			dashVelocity += newVelocity;
	}
	
	// Get knockback value to zero.
	public void ResetKnockback(bool keepDash) {
		knockbackVelocity = Vector2.zero;
		if(!keepDash)
			dashVelocity = Vector2.zero;
	}
	
	// Knockback reflection on hitting walls.
	void OnCollisionEnter(Collision col) {
		Vector2 normal = new Vector2(col.contacts[0].normal.x, col.contacts[0].normal.z);
		
		if(Mathf.Abs(normal.x) < Mathf.Abs(normal.y)) {
			knockbackVelocity.y = -knockbackVelocity.y;
			dashVelocity.y = -dashVelocity.y;
		} else {
			knockbackVelocity.x = -knockbackVelocity.x;
			dashVelocity.x = -dashVelocity.x;
		}
	}
	
	
	public void Respawn() {
		transform.position = initPosition;
		ResetKnockback(false);
	}
	
	public void GiveSpeed() {
		effects += 1;
	}
	
	public void TakeSpeed() {
		effects -= 1;
	}
}
