using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	public GameObject cameraMoving;
	public GameObject SetupGameObject;
	public GameObject MainMenuGameObject;
	public GameObject ControlsGameObject;
	private static PlayerCursor mainMenuCursor;
	
	[Header("Settings")]
	private Vector3 setupCameraPos = new Vector3(63.45f, 2.45f, 25.25f);
	private Vector3 setupCameraRot = new Vector3(85, -108, 0);
	private Vector3 mainMenuCameraPos = new Vector3(66.5f, 5.5f, 17);
	private Vector3 mainMenuCameraRot = new Vector3(11, -2, 5);
	
	[Header("Current Values")]
	private bool setupMenuEnabled;
	private float cameraLerpValue;
	
    void Start() {
        SoundManager.PlayMusic("Menu", 0.3f, true);
		
		// If we don't have a cursor saved from previous time we opened main menu, create a new one.
		if(mainMenuCursor == null)
			mainMenuCursor = new PlayerCursor(Resources.Load<GameObject>("Prefabs/Cursor"), 0.7f, null);
		
		GoBackToMainMenu();
    }

    // Update is called once per frame
    void Update() {
		// Lerp camera between two positions
		cameraLerpValue += setupMenuEnabled ? Time.deltaTime : -Time.deltaTime;
		cameraLerpValue = Mathf.Clamp(cameraLerpValue, 0f, 1f);
		cameraMoving.transform.position = Vector3.Lerp(mainMenuCameraPos, setupCameraPos, cameraLerpValue);
		cameraMoving.transform.eulerAngles = Vector3.Lerp(mainMenuCameraRot, setupCameraRot, cameraLerpValue);
		
		mainMenuCursor.UpdateCursorPosition();
		
		if(Input.GetKey(KeyCode.Escape))
			GoBackToMainMenu();
    }
	
	public void GoToSetup() {
		setupMenuEnabled = true;
		MainMenuGameObject.SetActive(false);
		SetupGameObject.SetActive(true);
		
		// Hide main cursor, display player cursors.
		SwitchCursors();
	}
	
	public void GoBackToMainMenu() {
		setupMenuEnabled = false;
		MainMenuGameObject.SetActive(true);
		SetupGameObject.SetActive(false);
		ControlsGameObject.SetActive(false);
		
		PlayerSetup.ResetSetupData();
		
		// Display main cursor if we've got connected gamepads, hide player cursors.
		SwitchCursors();
	}
	
	public void GoToControls() {
		MainMenuGameObject.SetActive(false);
		ControlsGameObject.SetActive(true);
	}
	
	public void Quit() {
		Application.Quit();
	}
	
	// Call the function to see if we need to show gamepad cursor upon any device connection and disconnection.
	void OnEnable() {
		InputSystem.onDeviceChange += SwitchCursors;
	}

	void OnDisable() {
		InputSystem.onDeviceChange -= SwitchCursors;
	}
	
	// Overload needed for proper OnDeviceChange subscription.
	void SwitchCursors(InputDevice device, InputDeviceChange change) {
		SwitchCursors();
	}
	
	void SwitchCursors() {
		if(setupMenuEnabled) {
			foreach (Player player in PlayerHolder.GetPlayers())
				player.GetCursor().ShowCursor();
				
			mainMenuCursor.HideCursor();
		} else {
			foreach (Player player in PlayerHolder.GetPlayers())
				player.GetCursor().HideCursor();
				
			if(InputSystem.devices.Any(device => device is Gamepad))
				mainMenuCursor.ShowCursor();
			else
				mainMenuCursor.HideCursor();
		}
	}
}