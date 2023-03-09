using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterOwner))]
[RequireComponent(typeof(CharacterHit))]
public class CharacterControls : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	private CharacterOwner characterOwner;
	private CharacterHit characterHit;
	private Camera sharedCamera;
	
	[Header("Settings")]
	[SerializeField]private float speedInUnitsPerSec;
	
	[Header("Current Values")]
	[SerializeField]private bool moving;
	private Vector3 initPosition;
	
	// Initializing cached GameObjects and Components.
    void Start() {
        sharedCamera = Camera.main;
		characterOwner = GetComponent<CharacterOwner>();
		characterHit = GetComponent<CharacterHit>();
		initPosition = transform.position;
    }
	
    void FixedUpdate() {
        if(characterOwner.GetOwner() == null)
			return;
		
		// If we use left stick and the cursor is hidden, use that for controls.
		Vector2 leftStickInput = GamepadInput.GetLeftStick(characterOwner.GetOwner().GetGamepad()).normalized;
		if(characterOwner.GetOwner().GetCursor().GetCursorHidden() && GamepadExtensions.InputMoreThanDeadzone(leftStickInput)) {
			if(!characterHit.GetCharging()) {
				// If we're not charging, left stick is used for movement.
				Vector3 targetPos = transform.position + new Vector3(leftStickInput.x, 0, leftStickInput.y) * (speedInUnitsPerSec * Time.fixedDeltaTime);
				
				//Set max position values.
				targetPos.x = Mathf.Min(targetPos.x, 14f);
				targetPos.x = Mathf.Max(targetPos.x, -14f);
				targetPos.z = Mathf.Min(targetPos.z, 6.5f);
				targetPos.z = Mathf.Max(targetPos.z, -6.5f);
					
				// Face a direction we're going.
				transform.LookAt(targetPos);
				transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
				
				transform.position = targetPos;
			} else {
				// Otherwise, it is used for rotation.
				float rotationAngle = Mathf.Atan2(leftStickInput.x, leftStickInput.y) * Mathf.Rad2Deg;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationAngle, transform.eulerAngles.z);
			}
		}
		
		moving = GamepadExtensions.InputMoreThanDeadzone(leftStickInput);
		
		// TODO: Move that to player start.
		characterOwner.GetOwner().GetCursor().HideCursor();
		
    }
	
	public void Respawn() {
		transform.position = initPosition;
	}
	
	public bool GetMoving() {
		return moving;
	}
}
