using UnityEngine;
using System.Collections;

public class CharacterControls : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private GameObject walkableGround;
	private CharacterOwner characterOwner;
	private Camera sharedCamera;
	
	[Header("Settings")]
	[SerializeField]private float speedInUnitsPerSec;
	
	[Header("Current Values")]
	private Coroutine movementCoroutine;
	
	// Initializing cached GameObjects and Components.
    void Start() {
        sharedCamera = Camera.main;
		characterOwner = GetComponent<CharacterOwner>();
    }
	
    void FixedUpdate() {
		// When a new command is being issued, analyze what the cursor is looking at.
		if(characterOwner.GetCursor().IsCursorPressed()) {
			// If we're looking at walkableGround, walk there.
			if(characterOwner.GetCursor().CursorOverObject(walkableGround))
				MoveToSpot(characterOwner.GetCursor().GetCursorPosition());
		}
		
		// If we use the right stick, start rotating.
		Vector2 rightStickInput = GamepadExtensions.GetRightStick(characterOwner.GetOwner().GetGamepad());
		if(Mathf.Abs(rightStickInput.x) > 0.2f || Mathf.Abs(rightStickInput.y) > 0.2f) {
			// Stop any earlier movement coroutines;
			if(movementCoroutine != null)
				StopCoroutine(movementCoroutine);
			
			// Get the rotationg angle from input.
			float rotationAngle = Mathf.Atan2(rightStickInput.x, rightStickInput.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotationAngle, transform.rotation.eulerAngles.z);
		}
		
    }
	
	// Gets world position from cursor position and walks there.
	public void MoveToSpot(Vector2 cursorPosition) {
		// Stop any earlier movement coroutines;
		if(movementCoroutine != null)
			StopCoroutine(movementCoroutine);
		
		// Get new point from cursor position.
		Vector3 worldCursorPosition = cursorPosition;
		worldCursorPosition.z = Vector3.Distance(sharedCamera.gameObject.transform.position, walkableGround.transform.position);
		Vector3 targetPos = sharedCamera.ScreenToWorldPoint(worldCursorPosition);
		
		// Face a direction we're going.
		Vector3 lookVector = targetPos - transform.position;
		lookVector.y = 0;
		transform.LookAt(targetPos);
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
		
		movementCoroutine = StartCoroutine(SlowlyMoveToPoint(targetPos));
	}
	
	IEnumerator SlowlyMoveToPoint(Vector3 targetPoint) {
		Vector3 startPos = transform.position;
		
		Vector3 movementVector = (targetPoint - startPos);
		movementVector.y = 0;
		movementVector = movementVector.normalized;
		Debug.Log(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPoint.x, 0, targetPoint.z)) + " " + (speedInUnitsPerSec / 100) * 2);
		while(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPoint.x, 0, targetPoint.z)) > (speedInUnitsPerSec / 100) * 2) {
			transform.position += movementVector * (speedInUnitsPerSec / 100);
			yield return new WaitForSeconds(0.01f);
		}
		
		transform.position = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
	}
}
