using UnityEngine;
using UnityEngine.InputSystem;

// Class for player that holds their name, associated color, attached cursor, and gamepad they use for controlling the game.
// Also features function for handling cursor movement and hiding.
public class Player {
	private string gameName;
	private Gamepad controlledGamepad;
	private GameObject cursor;
	private Color color;
	private bool disconnected;
	
	private float cursorSensitivity;
	
	public Player(Gamepad controlledGamepad, GameObject cursorPrefab, Color color, float cursorSensitivity) {
		this.gameName = "";
		this.controlledGamepad = controlledGamepad;
		this.cursor = InstantiateCursor(cursorPrefab);
		this.color = color;
		this.cursorSensitivity = cursorSensitivity;
	}
	
	public Gamepad GetGamepad() {
		return controlledGamepad;
	}
		
	// Spawns a new cursor instance from the given prefab, and gets it to the center of the screen.
	private GameObject InstantiateCursor(GameObject cursorPrefab) {
		GameObject newInst = Object.Instantiate(cursorPrefab);
		newInst.transform.SetParent(GameObject.Find("Canvas").transform);
		newInst.transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
		return newInst;
	}
		
	
	public void UpdateCursorPosition() {
		Vector3 tempPos = cursor.transform.position;
		
		// Calculate position change from gamepad input and cursor sensitivity value. Make it FPS and screen width independent.
		Vector3 positionChange = (Vector3)GamepadExtensions.GetRightStick(controlledGamepad) * Time.deltaTime * cursorSensitivity * Screen.width;
		tempPos += positionChange;
			
		// Limit cursor position within screen boundaries.
		tempPos.x = Mathf.Clamp(tempPos.x, 0, Screen.width);
		tempPos.y = Mathf.Clamp(tempPos.y, 0, Screen.height);
		cursor.transform.position = tempPos;
	}
	
	public void Disconnect() {
		disconnected = true; 
		
		// Hide cursor, making it invisible.
		cursor.SetActive(false);
	}
	
	public void Reconnect() {
		disconnected = false; 
		
		// Unhide cursor and set it to the center of the screen.
		cursor.SetActive(true);
		cursor.transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
	}
}