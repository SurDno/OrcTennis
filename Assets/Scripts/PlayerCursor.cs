using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerCursor {
	[Header("Prefabs and Cached Objects")]
	private EventSystem eventSystem;
	private GraphicRaycaster graphicRaycaster;
	
	[Header("Unique Properties")]
	private Player owner;
	private Image cursorImage;
	private float cursorSensitivity;
	
	public PlayerCursor(GameObject cursorPrefab, float cursorSensitivity, Player owner) {
		// Get values from constructor where necessary.
		this.cursorSensitivity = cursorSensitivity;
		this.owner = owner;
		
		// Spawn a cursor from the given prefab.
		this.cursorImage = InstantiateCursor(cursorPrefab);
		SetCursorToTheCenterOfTheScreen();
		
		// Cash references we're gonna use latter.
		CacheReferences();
	}
	
	// Caches EventSystem and GraphicRaycaster for UI raycast.
	void CacheReferences() {
		GameObject eventSystemGO = GameObject.Find("EventSystem");
		if(eventSystemGO != null)
			eventSystem = eventSystemGO.GetComponent<EventSystem>();
		
		GameObject graphicRaycasterGO = GameObject.Find("UICanvas");
		if(graphicRaycasterGO != null)
			graphicRaycaster = graphicRaycasterGO.GetComponent<GraphicRaycaster>();
	}
	
	// Spawns a new cursor instance from the given prefab, and gets it to the center of the screen.
	private Image InstantiateCursor(GameObject cursorPrefab) {
		GameObject newInst = Object.Instantiate(cursorPrefab);
		newInst.transform.SetParent(GameObject.Find("CursorCanvas").transform);
		Object.DontDestroyOnLoad(newInst.transform.root.gameObject);
		
		Image newInstImage = newInst.GetComponent<Image>();
		newInstImage.color = owner.GetColor();
		
		return newInstImage;
	}
	
	private void SetCursorToTheCenterOfTheScreen() {
		cursorImage.gameObject.transform.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);
	}
	
	// Controls the cursor position.
	public void UpdateCursorPosition() {
		if(GetCursorHidden())
			return;
		
		Vector3 tempPos = cursorImage.gameObject.transform.position;
		
		// Calculate position change from gamepad input and cursor sensitivity value. Make it FPS and screen width independent.
		Vector3 positionChange = (Vector3)GamepadInput.GetLeftStick(owner.GetGamepad()) * Time.deltaTime * cursorSensitivity * Screen.width;
		tempPos += positionChange;
			
		// Limit cursor position within screen boundaries.
		tempPos.x = Mathf.Clamp(tempPos.x, 0, Screen.width);
		tempPos.y = Mathf.Clamp(tempPos.y, 0, Screen.height);
		cursorImage.gameObject.transform.position = tempPos;
	}
	
	// Returns whether cursor is currently pressed.
	public bool IsCursorPressed() {
		return GamepadInput.GetLeftStickButtonDown(owner.GetGamepad()) && !GetCursorHidden();
	}
	
	// Returns the current position of cursor in pixels.
	public Vector2 GetCursorPosition() {
		return (Vector2)cursorImage.gameObject.transform.position;
	}
	
	// Checks if this fake cursor is currently on given 3D mesh.
	public bool CursorOverObject(GameObject obj) {
		//If we're disconnected, always return false.
		if(owner.GetDisconnected())
			return false;
		
		// Shoot a raycast from cursor position.
		Ray cursorRay = Camera.main.ScreenPointToRay(GetCursorPosition());
        Debug.DrawRay(cursorRay.origin, cursorRay.direction * 10f, owner.GetColor());
		RaycastHit hit; 
		
		// If we hit something, return whether it's the object we're looking for.
		if(Physics.Raycast(cursorRay, out hit, Mathf.Infinity))
			return hit.transform.gameObject == obj;
		
		// Otherwise, simply return false.
		return false;
	}
	
	// Checks if this fake cursor is currently on given UI object.
	public bool CursorOverUI(GameObject obj) {
		//If we're disconnected, always return false.
		if(owner.GetDisconnected())
			return false;
		
		// Check if we need to recache references.
		if(graphicRaycaster == null || eventSystem == null)
			CacheReferences();
		
		// Shoot a UI raycast from cursor position.
		PointerEventData cursorData = new PointerEventData(eventSystem);
		cursorData.position = GetCursorPosition();
        List<RaycastResult> results = new List<RaycastResult>();
		graphicRaycaster.Raycast(cursorData, results);
		
		// Check if at least one UI Raycast hits is for the right object. Return true if so.
		foreach (RaycastResult result in results)
			if(result.gameObject == obj)
				return true;
		
		// Otherwise, return false.
		return false;
	}
	
	// Destroys the cursor, removing it from the game.
	public void DestroyCursor() {
		Object.Destroy(cursorImage.gameObject);
	}
	
	public void HideCursor() {
		cursorImage.enabled = false;
	}
	
	public void ShowCursor() {
		cursorImage.enabled = true;
		SetCursorToTheCenterOfTheScreen();
	}
	
	public bool GetCursorHidden() {
		return !cursorImage.enabled;
	}
}