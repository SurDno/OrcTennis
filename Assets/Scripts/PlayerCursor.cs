using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor {
	private Player owner;
	private GameObject cursorObject;
	private float cursorSensitivity;
	
	public PlayerCursor(GameObject cursorPrefab, float cursorSensitivity, Player owner) {
		// Get values from constructor where necessary.
		this.cursorSensitivity = cursorSensitivity;
		this.owner = owner;
		
		// Spawn a cursor from the given prefab.
		this.cursorObject = InstantiateCursor(cursorPrefab);
		SetCursorToTheCenterOfTheScreen();
	}
	
	// Spawns a new cursor instance from the given prefab, and gets it to the center of the screen.
	private GameObject InstantiateCursor(GameObject cursorPrefab) {
		GameObject newInst = Object.Instantiate(cursorPrefab);
		newInst.transform.SetParent(GameObject.Find("Canvas").transform);
		return newInst;
	}
	
	private void SetCursorToTheCenterOfTheScreen() {
		cursorObject.transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
	}
	
	// Controls the cursor position.
	public void UpdateCursorPosition() {
		Vector3 tempPos = cursorObject.transform.position;
		
		// Calculate position change from gamepad input and cursor sensitivity value. Make it FPS and screen width independent.
		Vector3 positionChange = (Vector3)GamepadExtensions.GetLeftStick(owner.GetGamepad()) * Time.deltaTime * cursorSensitivity * Screen.width;
		tempPos += positionChange;
			
		// Limit cursor position within screen boundaries.
		tempPos.x = Mathf.Clamp(tempPos.x, 0, Screen.width);
		tempPos.y = Mathf.Clamp(tempPos.y, 0, Screen.height);
		cursorObject.transform.position = tempPos;
	}
	
	// Returns whether cursor is currently pressed.
	public bool IsCursorPressed() {
		return GamepadExtensions.IsLeftStickPressed(owner.GetGamepad());
	}
	
	// Returns the current position of cursor in pixels.
	public Vector2 GetCursorPosition() {
		return (Vector2)cursorObject.transform.position;
	}
	
	// Checks if this fake cursor is currently on given object.
	public bool CursorOverObject(GameObject obj) {
		// Shoot a raycast
		Ray cursorRay = Camera.main.ScreenPointToRay(GetCursorPosition());
        Debug.DrawRay(cursorRay.origin, cursorRay.direction * 10f, owner.GetColor());
		RaycastHit hit; 
		
		// If we hit something, return whether it's the object we're looking for.
		if(Physics.Raycast(cursorRay, out hit, Mathf.Infinity))
			return hit.transform.gameObject == obj;
		
		// Otherwise, simply return false.
		return false;
	}
	
	// Hides cursor, making it invisible.
	public void HideCursor() {
		cursorObject.SetActive(false);
	}
	
	// Unhides cursor and sets it to the center of the screen.
	public void UnhideCursor() {
		cursorObject.SetActive(true);
		SetCursorToTheCenterOfTheScreen();
	}
}
