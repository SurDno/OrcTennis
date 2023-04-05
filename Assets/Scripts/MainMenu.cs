using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	public GameObject cameraMoving;
	public GameObject SetupGameObject;
	public GameObject MainMenuGameObject;
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
		
		if(setupMenuEnabled && Input.GetKey(KeyCode.Escape))
			GoBackToMainMenu();
    }
	
	public void GoToSetup() {
		setupMenuEnabled = true;
		MainMenuGameObject.SetActive(false);
		SetupGameObject.SetActive(true);
		
		// Display main cursor, hide player cursors.
		mainMenuCursor.HideCursor();
        foreach (Player player in PlayerHolder.GetPlayers())
            player.GetCursor().ShowCursor();
	}
	
	public void GoBackToMainMenu() {
		setupMenuEnabled = false;
		MainMenuGameObject.SetActive(true);
		SetupGameObject.SetActive(false);
		
		// Hide main cursor, display player cursors.
		mainMenuCursor.ShowCursor();
        foreach (Player player in PlayerHolder.GetPlayers())
            player.GetCursor().HideCursor();
	}
}
