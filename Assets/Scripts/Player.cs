using UnityEngine;
using UnityEngine.InputSystem;

// Class for player that holds their name, associated color, attached cursor, and gamepad they use for controlling the game.
// Also features function for handling cursor movement and hiding.
public class Player {
	public enum Team {Unselected, Green, Red};
	
	[Header("Unique Properties")]
	private InputDevice controlledGamepad;
	private PlayerCursor controlledCursor;
	private CharacterOwner controlledCharacter;
	private Color color;
	
	[Header("Current Values")]
	private bool disconnected;
	private bool ready;
	private Team chosenTeam;
	
	public Player(InputDevice controlledGamepad, GameObject cursorPrefab, Color color, float cursorSensitivity) {
		// Get values from constructor where necessary.
		this.controlledGamepad = controlledGamepad;
		this.color = color;
		
		// Create a new cursor instance and pass the necessary values straight to it.
		this.controlledCursor = new PlayerCursor(cursorPrefab, cursorSensitivity, this);
	}
	
	// Returns the controlled gamepad.
	public InputDevice GetGamepad() {
		return controlledGamepad;
	}
	
	// Returns the controlled cursor.
	public PlayerCursor GetCursor() {
		return controlledCursor;
	}
	
	public CharacterOwner GetCharacter() {
		return controlledCharacter;
	}
	
	public void SetCharacter(CharacterOwner newValue) {
		controlledCharacter = newValue;
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
	
	public void SetReady(bool newValue) {
		ready = newValue;
	}
	
	// Returns what team the player is in.
	public Team GetTeam() {
		return chosenTeam;
	}
	
	public void SetTeam(Team newValue) {
		chosenTeam = newValue;
	}
	
	public void Disconnect() {
		controlledCursor.HideCursor();
	}
	
	public void Reconnect() {
		controlledCursor.ShowCursor();
	}
	
	// Destroys the cursor, removing the only player-associated gameobject.
	// Sets disconnected to true so that all PlayerSetup.cs can remove the reference.
	public void DeletePlayerInstance() {
		disconnected = true;
		
		controlledCursor.DestroyCursor();
		controlledCursor = null;
	}
}