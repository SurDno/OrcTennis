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
	
	// Getter methods
	public InputDevice GetGamepad() => controlledGamepad;
	public PlayerCursor GetCursor() => controlledCursor;
	public CharacterOwner GetCharacter() => controlledCharacter;
	public Color GetColor() => color;
	public bool GetDisconnected() => disconnected;
	public bool GetReady() => ready;
	public Team GetTeam() => chosenTeam;
	
	// Setter methods
	public void SetCharacter(CharacterOwner newValue) => controlledCharacter = newValue;
	public void SetReady(bool newValue) => ready = newValue;
	public void SetTeam(Team newValue) => chosenTeam = newValue;
	
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
	
	// Resets player data after the match.
	public void ResetPlayerData() {
		SetCharacter(null);
		SetReady(false);
		SetTeam(Player.Team.Unselected);
	}
}