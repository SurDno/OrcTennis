using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// Adds a new player every time a new gamepad is connected. Updates their cursors.
public class PlayerHolder : MonoBehaviour {
	[Header("Settings and Prefabs")]
	[SerializeField] private Color[] playerColors;
	[SerializeField] private GameObject playerCursor; 
	[SerializeField] public float cursorSensitivity = 1.0f;
	
	[Header("Currrent Player Information")]
    [SerializeField] private int lowestAvailablePlayerIndex;
	[SerializeField] private static Player[] players = new Player[6];
	
	// Singleton pattern.
    void Awake() {
        PlayerHolder[] objs = GameObject.FindObjectsOfType<PlayerHolder>();

        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
	
    void Start() {
		// Get amount of gamepads, and generate a player instance for each connected gamepad.
        IEnumerable<InputDevice> devices = InputSystem.devices;
		foreach(InputDevice device in devices)
			if(device is Gamepad || device is Joystick) {
				CreateNewPlayer(device);
				lowestAvailablePlayerIndex++;
			}
		CreateNewPlayer(Keyboard.current);
		players[lowestAvailablePlayerIndex].SetReady(true);
		lowestAvailablePlayerIndex++;
		
    }

	void Update() {
		// Update each existing player cursor position every frame.
		foreach(Player player in players)
			if(player != null)
				player.GetCursor().UpdateCursorPosition();
	}
	
	void CreateNewPlayer(InputDevice controlledGamepad) {
		Color colorToUse = playerColors[lowestAvailablePlayerIndex];
		
		players[lowestAvailablePlayerIndex] = new Player(controlledGamepad, playerCursor, colorToUse, cursorSensitivity);

	}
	
	// Deletes the player instance by removing its cursor from the game and removing the reference to it from the players array.
	void DeletePlayer(int index) {
		players[index].DeletePlayerInstance();
		players[index] = null;
		
		if(index < lowestAvailablePlayerIndex)
			lowestAvailablePlayerIndex = index;
	}
	
	// Returns an array of non-null player entries.
	public static Player[] GetPlayers() {
		return players.Where(player => player != null).ToArray();
	}
	
	// Looks through the players array, find the first null value and returns it as earliest player slot that can be occupied by a new connection.
	void GetNewLowestAvailableIndexValue() {
		// Get new lowest available index value.
		for(int i = 0; i < players.Length; i++)
			if(players[i] == null) {
				lowestAvailablePlayerIndex = i;
				break;
			}
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
		if(!(device is Gamepad || device is Joystick))
			return;
		
		// Call more specific functions depending on what the change is.
        switch (change) {
            case InputDeviceChange.Added:
                OnGamepadConnected(device);
                break;
            case InputDeviceChange.Disconnected:
                OnGamepadDisconnected(device);
                break;
		}
    }
	
	void OnGamepadConnected(InputDevice connectedGamepad) {
		if(connectedGamepad == null) {
			Debug.LogError("Received gamepad ID is null. Reconnect gamepad.");
			return;
		}
		
		// Create a new player instance for that gamepad.
		CreateNewPlayer(connectedGamepad);
		
		// Find new lowest available player index value.
		GetNewLowestAvailableIndexValue();
	}
	
	// TODO: Do not delete player instance if the game is already going. Instead, leave it without a gamepad and wait for the first new connected gamepad to take that player.
	void OnGamepadDisconnected(InputDevice disconnectedGamepad) {
		if(disconnectedGamepad != null) {
			// Find if there is a player who used that gamepad before, and delete them.
			for(int i = 0; i < players.Length; i++)
				if(players[i] != null && players[i].GetGamepad() == disconnectedGamepad)
					DeletePlayer(i);
		} else {
			// If gamepad instnace is null (should be impossible but hey it's Unity!) we should just iterate through all players and remove those who don't have gamepads.
			for(int i = 0; i < players.Length; i++)
				if(players[i] != null && (players[i].GetGamepad() == null || !GamepadExtensions.IsGamepadConnected(players[i].GetGamepad())))
					DeletePlayer(i);
		}
	}
}
