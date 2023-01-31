using UnityEngine;

public class CharacterControls : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private GameObject walkableGround;
	[SerializeField]private Camera sharedCamera;
	
	[Header("Settings")]
    [SerializeField]private Player owner;
	[SerializeField]private PlayerCursor ownerCursor;
	
	// Initializing cached GameObjects and Components.
    void Start() {
        sharedCamera = Camera.main;
    }
	
    void FixedUpdate() {
		// As long as we have no owner, do nothing as no commands can be issued.
        if(owner == null)
			return;
		
		// When a new command is being issued, analyze what the cursor is looking at.
		if(ownerCursor.IsCursorPressed()) {
			// If we're looking at walkableGround, walk there.
			if(ownerCursor.CursorOverObject(walkableGround))
				MoveToSpot(ownerCursor.GetCursorPosition());
		}
    }
	
	// Gets world position from cursor position and walks there.
	public void MoveToSpot(Vector2 cursorPosition) {
		Vector3 worldPos = sharedCamera.ScreenToWorldPoint(cursorPosition);
		
		transform.position = worldPos;
	}
	
	
	// Used to give ownership of the unit to any specific player.
	public void TransferOwnership(Player newOwner) {
		owner = newOwner;
		
		if(owner != null)
			ownerCursor = owner.GetCursor();
		else 
			ownerCursor = null;
	}
}
