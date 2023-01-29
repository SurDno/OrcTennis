using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Adds a new player every time a new gamepad is connected.
public class PlayerHolder : MonoBehaviour {
	[Header("Settings and Prefabs")]
	[SerializeField] private Color[] playerColors;
	[SerializeField] private GameObject[] playerCursors; 
	[SerializeField] public float cursorSensitivity = 1.0f;
	
	[Header("Currrent Player Information")]
    [SerializeField] private int lowestAvailablePlayerIndex;
	[SerializeField] private Player[] players = new Player[6];
	
    void Start() {
		// Get amount of gamepads, and generate a player instance for each connected gamepad.
        IEnumerable<InputDevice> devices = InputSystem.devices;
		foreach(InputDevice device in devices)
			if(device is Gamepad) {
				players[lowestAvailablePlayerIndex] = new Player((Gamepad)device, playerCursors[lowestAvailablePlayerIndex], playerColors[lowestAvailablePlayerIndex], cursorSensitivity);
				lowestAvailablePlayerIndex++;
			}
    }

	void Update() {
		foreach(Player player in players)
			if(player != null)
				player.UpdateCursorPosition();
	}
	
	// Ensures we get updated about new gamepad connections.
	void OnEnable() {
		InputSystem.onDeviceChange += OnDeviceChange;
	}

	void OnDisable() {
		InputSystem.onDeviceChange -= OnDeviceChange;
	}
	
	void OnDeviceChange(InputDevice device, InputDeviceChange change) {
		// We're not interested in any non-gamepad related changes.
		if(!(device is Gamepad))
			return;
		
		// Call more specific functions depending on what the change is.
        switch (change) {
            case InputDeviceChange.Added:
                OnGamepadConnected((Gamepad)device);
                break;
            case InputDeviceChange.Disconnected:
                OnGamepadDisconnected((Gamepad)device);
                break;
		}
    }
	
	void OnGamepadConnected(Gamepad connectedGamepad) {
		// Find if there is a player who used that gamepad before.
		Player playerForGamepad = Array.Find(players, player => player != null && player.GetGamepad() == connectedGamepad);
		
		// If there was, simply unhide their cursor.
		if(playerForGamepad != null) {
			playerForGamepad.Reconnect();
			return;
		}
		
		// If there was not, create a new player instance for that gamepad.
		players[lowestAvailablePlayerIndex] = new Player(connectedGamepad, playerCursors[lowestAvailablePlayerIndex], playerColors[lowestAvailablePlayerIndex], cursorSensitivity);
		lowestAvailablePlayerIndex++;
	}
	
	void OnGamepadDisconnected(Gamepad disconnectedGamepad) {
		// Find if there is a player who used that gamepad before, and hide their cursor.
		Player playerForGamepad = Array.Find(players, player => player != null && player.GetGamepad() == disconnectedGamepad);
		playerForGamepad.Disconnect();
	}
}
