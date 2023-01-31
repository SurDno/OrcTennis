using UnityEngine;
using UnityEngine.InputSystem;

// Class for player that holds their name, associated color, attached cursor, and gamepad they use for controlling the game.
// Also features function for handling cursor movement and hiding.
public class Player {
	[Header("Unique Properties")]
	private Gamepad controlledGamepad;
	private PlayerCursor controlledCursor;
	private Color color;
	
	[Header("Current Values")]
	private bool disconnected;
	private bool ready;
	private bool teamGreen;
	private bool teamRed;
	
	public Player(Gamepad controlledGamepad, GameObject cursorPrefab, Color color, float cursorSensitivity) {
		// Get values from constructor where necessary.
		this.controlledGamepad = controlledGamepad;
		this.color = color;
		
		// Create a new cursor instance and pass the necessary values straight to it.
		this.controlledCursor = new PlayerCursor(cursorPrefab, cursorSensitivity, this);
	}
	
	// Returns the controlled gamepad.
	public Gamepad GetGamepad() {
		return controlledGamepad;
	}
	
	// Returns the controlled cursor.
	public PlayerCursor GetCursor() {
		return controlledCursor;
	}
	
	// Returns the player cursor.
	public Color GetColor() {
		return color;
	}
	
	// Returns if the player is disconnected.
	public bool GetDisconnected() {
		return disconnected;
	}
	
	// Returns if the player is ready.
	public bool GetReady() {
		return ready;
	}
	
	public void Disconnect() {
		disconnected = true; 
		
		// Hide cursor, making it invisible.
		controlledCursor.HideCursor();
	}
	
	public void Reconnect() {
		disconnected = false; 
		
		// Unhide cursor and set it to the center of the screen.
		controlledCursor.UnhideCursor();
	}
	
	// Destroys the cursor, removing the only player-associated gameobject.
	public void DeletePlayerInstance() {
		controlledCursor.DestroyCursor();
	}
}